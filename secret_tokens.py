import socket, ssl, re, os, random, string, urllib.parse, tempfile, time, shutil
from typing import Dict, Tuple, List

CRLF = '\r\n'
STEP_RE = re.compile(r"<h1>\s*Шаг\s*#(\d+)\s*\(из множества\)\s*</h1>", re.I)


def parse_set_cookie(header_value: str) -> Tuple[str, str]:
    pair = header_value.split(';', 1)[0]

    if "=" in pair:
        k, v = pair.split('=', 1)
        return k.strip(), v.strip()

    return "", ""


def join_cookies(jar: Dict[str, str]) -> str:
    return "; ".join(f"{k}={v}" for k, v in jar.items())


def make_boundary() -> str:
    return "----py" + "".join(random.choices(string.ascii_letters + string.digits, k=24))


def build_multipart(fields: Dict[str, str], files: List[Tuple[str, str]]) -> Tuple[str, bytes]:
    boundary = make_boundary()
    parts: List[bytes] = []

    for name, value in fields.items():
        parts.append(
            (f"--{boundary}{CRLF}"
             f'Content-Disposition: form-data; name="{name}"{CRLF}{CRLF}'
             f"{value}{CRLF}").encode("utf-8")
        )

    for field, path in files:
        filename = os.path.basename(path)
        with open(path, "rb") as file:
            data = file.read()

        parts.append(
            (f"--{boundary}{CRLF}"
             f'Content-Disposition: form-data; name="{field}"; filename="{filename}"{CRLF}'
             f"Content-Type: application/octet-stream{CRLF}{CRLF}").encode("utf-8")
            + data + CRLF.encode("utf-8")
        )

    parts.append(f"--{boundary}--{CRLF}".encode("utf-8"))
    body = b"".join(parts)

    return boundary, body


def recv_until(sock: socket.socket, delimiter: bytes) -> bytes:
    buf = bytearray()
    while True:
        chunk = sock.recv(1)
        if not chunk:
            break
        buf += chunk
        if buf.endswith(delimiter):
            break

    return bytes(buf)


def recv_exact(sock: socket.socket, n: int) -> bytes:
    buf = bytearray()
    while len(buf) < n:
        chunk = sock.recv(n - len(buf))
        if not chunk:
            break
        buf += chunk

    return bytes(buf)


def read_chunked(sock: socket.socket) -> bytes:
    body = bytearray()
    while True:
        line = recv_until(sock, b"\r\n")
        if not line:
            break

        size_str = line.strip().split(b";", 1)[0]
        try:
            size = int(size_str, 16)
        except ValueError:
            size = 0

        if size == 0:
            recv_until(sock, b"\r\n")
            break

        body += recv_exact(sock, size)
        recv_until(sock, b"\r\n")

    return bytes(body)


def parse_headers(raw_headers: bytes) -> Dict[str, str]:
    headers = {}
    for line in raw_headers.decode("iso-8859-1").split("\r\n"):
        if not line or ":" not in line:
            continue

        k, v = line.split(":", 1)
        headers[k.strip().lower()] = v.strip()

    return headers


class RawHTTPClient:
    def __init__(self, base_url: str, user_id_cookie: str):
        u = urllib.parse.urlparse(base_url)
        if not u.scheme or not u.netloc:
            raise ValueError("Enter URL, for example https://example.com")

        self.scheme = u.scheme.lower()
        self.host = u.hostname
        self.port = u.port or (443 if self.scheme == "https" else 80)
        self.base_path = u.path if u.path else "/"
        self.use_ssl = self.scheme == "https"
        self.cookies: Dict[str, str] = {"user": user_id_cookie}


    def connect(self) -> socket.socket:
        raw = socket.create_connection((self.host, self.port), timeout=15)
        if self.use_ssl:
            ctx = ssl.create_default_context()
            return ctx.wrap_socket(raw, server_hostname=self.host)

        return raw


    def send_request(self, method: str, path: str, headers: Dict[str, str], body: bytes = b"") -> Tuple[int, Dict[str, str], bytes]:
        if not path.startswith("/"):
            path = urllib.parse.urljoin(self.base_path, path)

        headers_lower = {k.lower(): v for k, v in headers.items()}
        if "host" not in headers_lower:
            headers["Host"] = self.host
        if "connection" not in headers_lower:
            headers["Connection"] = "close"

        if "cookie" not in headers_lower:
            cookie_line = join_cookies(self.cookies)
            if len(cookie_line.encode("utf-8")) > 7500:
                u = self.cookies.get("user")
                self.cookies = {"user": u} if u is not None else {}
                cookie_line = join_cookies(self.cookies)

            if cookie_line:
                headers["Cookie"] = cookie_line

        if body and "content-length" not in headers_lower and "transfer-encoding" not in headers_lower:
            headers["Content-Length"] = str(len(body))

        request_lines = [f"{method} {path} HTTP/1.1"]
        for k, v in headers.items():
            request_lines.append(f"{k}: {v}")

        request_lines.append("")
        request_raw = (CRLF.join(request_lines) + CRLF).encode("utf-8")
        request_str = request_raw + body

        s = self.connect()
        try:
            s.sendall(request_str)
            status_line = recv_until(s, b"\r\n")
            if not status_line:
                return 0, {}, b""

            m = re.match(rb"HTTP/\d\.\d\s+(\d{3})", status_line)
            status = int(m.group(1)) if m else 0
            raw_headers = recv_until(s, b"\r\n\r\n")
            headers_dict = parse_headers(raw_headers)

            for m in re.finditer(rb'(?i)^Set-Cookie:\s*([^\r\n]+)', raw_headers, flags=re.M):
                sc_line = m.group(1).decode('iso-8859-1', errors='ignore')
                k, v = parse_set_cookie(sc_line)
                if k:
                    if k == "user":
                        continue
                    self.cookies[k] = v

            body_bytes = b""
            te = headers_dict.get("transfer-encoding", "")
            if "chunked" in te.lower():
                body_bytes = read_chunked(s)
            else:
                cl = headers_dict.get("content-length")
                if cl is not None and cl.isdigit():
                    body_bytes = recv_exact(s, int(cl))
                else:
                    chunk = s.recv(4096)
                    while chunk:
                        body_bytes += chunk
                        chunk = s.recv(4096)

            return status, headers_dict, body_bytes
        finally:
            s.close()


    def get(self, path: str) -> Tuple[int, Dict[str, str], bytes]:
        return self.send_request("GET", path, {
            "User-Agent": "rawpy/1.0",
            "Accept": "*/*",
        })


    def post_multipart(self, path: str, fields: Dict[str, str], files: List[Tuple[str, str]]):
        boundary, body = build_multipart(fields, files)
        headers = {
            "User-Agent": "auto-client/1.0",
            "Accept": "*/*",
            "Content-Type": f"multipart/form-data; boundary={boundary}",
        }
        return self.send_request("POST", path, headers, body)


def extract_code_sequence(html_text: str):
    return re.findall(r'<code>(.*?)</code>', html_text, flags=re.S)


def find_href(html_text: str):
    m = re.search(r'href=[\'"]([^\'"]+)[\'"]', html_text)
    return m.group(1) if m else None


def find_path_after_keyword(html_text: str, keyword: str):
    idx = html_text.find(keyword)
    if idx == -1:
        return None

    m = re.search(r'<code>(/[^<\s]*)</code>', html_text[idx:], flags=re.S)
    return m.group(1) if m else None


def slice_first_table_after(html_text: str, keyword: str) -> str:
    idx = html_text.find(keyword)
    if idx == -1:
        return ""

    t_start = html_text.find("<table", idx)
    if t_start == -1:
        return ""

    t_end = html_text.find("</table>", t_start)
    if t_end == -1:
        return ""

    return html_text[t_start:t_end]


def pairs_after_keyword(html_text: str, keyword: str):
    table_html = slice_first_table_after(html_text, keyword)
    if not table_html:
        return []

    codes = extract_code_sequence(table_html)
    pairs = []

    for i in range(0, len(codes), 2):
        if i + 1 < len(codes):
            k = codes[i].strip()
            v = codes[i+1].strip()
            pairs.append((k, v))

    return pairs


def files_after_keyword(html_text: str, keyword: str):
    table_html = slice_first_table_after(html_text, keyword)
    if not table_html:
        return []

    codes = extract_code_sequence(table_html)
    files = []

    for i in range(0, len(codes), 2):
        if i + 1 < len(codes):
            files.append((codes[i].strip(), codes[i+1]))

    return files


def sniff_token_simple(html_text: str):
    m = re.search(r'["\']token["\']\s*:\s*["\']([0-9a-fA-F]{32,128})["\']', html_text)
    if m:
        return m.group(1).lower()

    m = re.search(
        r'Поздрав[^<]{0,500}секрет\w*\s+(?:ключ|токен)[^<]{0,200}<code>\s*([0-9a-fA-F]{32,128})\s*</code>',
        html_text, flags=re.I | re.S
    )

    if m:
        return m.group(1).lower()

    codes = re.findall(r'<code>\s*([^<\s][^<]*?)\s*</code>', html_text, flags=re.S)
    candidates = [c.strip() for c in codes
                  if re.fullmatch(r'[0-9a-fA-F]{32,128}', c.strip())]
    if len(candidates) == 1:
        return candidates[0].lower()

    return None


def parse_step_no(html: str) -> int | None:
    m = STEP_RE.search(html)
    return int(m.group(1)) if m else None

def build_query(params: list[tuple[str,str]]) -> str:
    if not params:
        return ""
    return urllib.parse.urlencode(params, doseq=True, encoding="utf-8")

def decide_next_action(html: str) -> dict:
    text = html
    upload_path = find_path_after_keyword(text, "Загрузите файлы")
    files_list  = files_after_keyword(text, "Загрузите файлы")
    if upload_path and files_list:
        q = build_query(pairs_after_keyword(text, "При переходе выставьте следующие параметры"))
        if q:
            upload_path = f"{upload_path}?{q}"

        return {
            "method": "POST",
            "path": upload_path,
            "headers": dict(pairs_after_keyword(text, "Запрос должен иметь следующие заголовки")),
            "cookies": dict(pairs_after_keyword(text, "В запросе должны быть выставлены cookie")),
            "query": "",
            "form": [],
            "files": files_list,
        }

    post_path = find_path_after_keyword(text, "Отправьте POST")
    if post_path:
        q = build_query(pairs_after_keyword(text, "При переходе выставьте следующие параметры"))
        if q:
            post_path = f"{post_path}?{q}"

        return {
            "method": "POST",
            "path": post_path,
            "headers": dict(pairs_after_keyword(text, "Запрос должен иметь следующие заголовки")),
            "cookies": dict(pairs_after_keyword(text, "В запросе должны быть выставлены cookie")),
            "query": "",
            "form": pairs_after_keyword(text, "Запрос должен иметь следующие данные формы"),
            "files": [],
        }

    base_path = None
    if "Перейдите по" in text or "Перейдите" in text:
        base_path = find_href(text)
    if not base_path:
        base_path = find_path_after_keyword(text, "Отправьте GET")

    if base_path:
        q = build_query(pairs_after_keyword(text, "При переходе выставьте следующие параметры"))
        if q:
            base_path = f"{base_path}?{q}"

        return {
            "method": "GET",
            "path": base_path,
            "headers": dict(pairs_after_keyword(text, "Запрос должен иметь следующие заголовки")),
            "cookies": dict(pairs_after_keyword(text, "В запросе должны быть выставлены cookie")),
            "query": "",
            "form": [],
            "files": [],
        }

    href = find_href(text)
    if href:
        return {"method": "GET", "path": href, "headers": {}, "cookies": {}, "query": "", "form": [], "files": []}
    return {"method": "GET", "path": None, "headers": {}, "cookies": {}, "query": "", "form": [], "files": []}


def execute_action(client: RawHTTPClient, action: dict, verbose: bool):
    method = action["method"]
    path   = action["path"]
    hdrs   = {"User-Agent": "auto-client/1.0", "Accept": "*/*", **action.get("headers", {})}

    original_jar = client.cookies.copy()
    per_request_jar = original_jar.copy()
    if action.get("cookies"):
        per_request_jar.update(action["cookies"])

    client.cookies = per_request_jar
    try:
        if method == "GET":
            if verbose:
                print(f"→ GET {path}")
            return client.send_request("GET", path, hdrs)

        if method == "POST" and action.get("files"):
            tmpdir, files_to_send = write_temp_files(action["files"])
            boundary, body = build_multipart({}, files_to_send)
            hdrs["Content-Type"] = f"multipart/form-data; boundary={boundary}"
            if verbose:
                names = [os.path.basename(p) for _, p in files_to_send]
                print(f"→ POST (multipart) {path} files={names}")
            st, h, b = client.send_request("POST", path, hdrs, body)
            shutil.rmtree(tmpdir, ignore_errors=True)
            return st, h, b

        if method == "POST":
            pairs = action.get("form", [])
            body_str = urllib.parse.urlencode(pairs, doseq=True, encoding="utf-8") if pairs else ""
            if "content-type" not in {k.lower(): v for k, v in hdrs.items()}:
                hdrs["Content-Type"] = "application/x-www-form-urlencoded; charset=utf-8"
            if verbose:
                print(f"→ POST {path} fields={len(pairs)}")
            return client.send_request("POST", path, hdrs, body_str.encode("utf-8"))

        if verbose:
            print(f"→ {method} {path} (unsupported method, treated as GET)")
        return client.send_request("GET", path, hdrs)

    finally:
        tmp_keys = set(action.get("cookies", {}).keys())
        current_after = client.cookies.copy()

        for k in tmp_keys:
            if k != "user":
                current_after.pop(k, None)

        if "user" in original_jar:
            current_after["user"] = original_jar["user"]

        client.cookies = current_after


def auto_solve(client, start_path="/", max_steps=1000, delay=0.0, verbose=False):
    pending_action = {"method": "GET", "path": start_path, "headers": {}, "cookies": {}, "query": "", "form": [], "files": []}
    for step in range(1, max_steps + 1):
        if verbose:
            print(f"\n=== Шаг {step}: {pending_action['method']} {pending_action['path']} ===")

        st, headers, body = execute_action(client, pending_action, verbose=verbose)
        text = body.decode("utf-8", errors="ignore")

        if verbose:
            ln = len(body)
            step_no = parse_step_no(text)
            tinfo = f" (от сервера: шаг #{step_no})" if step_no else ""
            print(f"HTTP {st}, body={ln}{tinfo}")

        token = sniff_token_simple(text)
        if token:
            return token

        loc = headers.get("location")
        if loc:
            p = urllib.parse.urlparse(loc)
            pending_action = {"method": "GET", "path": (p.path or "/") + (("?" + p.query) if p.query else ""),
                              "headers": {}, "cookies": {}, "query": "", "form": [], "files": []}
            time.sleep(delay)
            continue

        pending_action = decide_next_action(text)
        if not pending_action["path"]:
            href = find_href(text)
            if not href:
                if verbose:
                    print("Не удалось распознать инструкцию на странице; выхожу.")
                return None
            pending_action = {"method": "GET", "path": href, "headers": {}, "cookies": {}, "query": "", "form": [], "files": []}

        time.sleep(delay)

    if verbose:
        print("Превышен лимит шагов без нахождения токена.")
    return None


def write_temp_files(files_list):
    tmpdir = tempfile.mkdtemp(prefix="auto_upload_")
    files_to_send = []

    for fname, content in files_list:
        safe_path = os.path.join(tmpdir, fname)
        os.makedirs(os.path.dirname(safe_path), exist_ok=True)
        with open(safe_path, "w", encoding="utf-8") as f:
            f.write(content)
        files_to_send.append(("file", safe_path))

    return tmpdir, files_to_send


def main():
    base_url = "http://hw1.alexbers.com/"
    user_cookie = "890848b3bf27817584c67d924a3fd00a"
    client = RawHTTPClient(base_url, user_cookie)

    verbose = True
    token = auto_solve(client, start_path="/", max_steps=1000, delay=0.0, verbose=verbose)
    print(token if token else "Token not found")


if __name__ == "__main__":
    main()
