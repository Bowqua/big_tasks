namespace lecture_C_;

/*
//1
public class Codeforce4
{
    private static void Main()
    {
        var number = int.Parse(Console.ReadLine()!);
        var dictionary = new Dictionary<string, HashSet<string>>();

        for (var i = 0; i < number; i++)
        {
            var text = Console.ReadLine()!;
            var parts = text.Split(":");
            var objectName = parts[0].Trim();
            var properties = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            dictionary[objectName] = [..properties];
        }
        
        var count = int.Parse(Console.ReadLine()!);
        for (var i = 0; i < count; i++)
        {
            var objects = Console.ReadLine()!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var result = new HashSet<string>(dictionary[objects[0]]);

            for (var j = 1; j < objects.Length; j++)
            {
                result.IntersectWith(dictionary[objects[j]]);
                if (result.Count == 0)
                    break;
            }
            
            if (result.Count == 0)
                Console.WriteLine("No solution.");
            else
            {
                var list = result.ToList();
                list.Sort();
                Console.WriteLine(string.Join(" ", list));
            }
        }
    }
}
*/

/*
//2
public class Codeforce4
{
    private static void Main()
    {
        var number = int.Parse(Console.ReadLine()!);
        var costs = Console.ReadLine()!.Split().Select(int.Parse).ToArray();
        var radios = new string[number];
        var indexByNumber = new Dictionary<string, int>(number);

        for (var i = 0; i < number; i++)
        {
            radios[i] = Console.ReadLine()!;
            indexByNumber[radios[i]] = i;
        }
        
        var distance = new long[number];
        var parent = new int[number];
        
        Array.Fill(distance, long.MaxValue);
        Array.Fill(parent, -1);
        distance[0] = 0;

        var queue = new PriorityQueue<int, long>();
        queue.Enqueue(0, 0);

        while (queue.Count > 0)
        {
            queue.TryDequeue(out var current, out var currentDistance);
            if (currentDistance != distance[current])
                continue;
            
            if (current == number - 1)
                break;

            foreach (var next in GetNeighbors(current, radios, indexByNumber))
            {
                var prefix = GetPrefixLength(radios[current], radios[next]);
                var weight = costs[prefix];
                var newDistance = distance[current] + weight;

                if (newDistance < distance[next])
                {
                    distance[next] = newDistance;
                    parent[next] = current;
                    queue.Enqueue(next, newDistance);
                }
            }
        }

        if (distance[number - 1] == long.MaxValue)
        {
            Console.WriteLine(-1);
            return;
        }

        var path = new List<int>();
        for (var edge = number - 1; edge != -1; edge = parent[edge])
            path.Add(edge + 1);
        
        path.Reverse();
        Console.WriteLine(distance[number - 1]);
        Console.WriteLine(path.Count);
        Console.WriteLine(string.Join(" ", path));
    }

    private static IEnumerable<int> GetNeighbors(int index, string[] radios, Dictionary<string, int> indexByNumber)
    {
        var letters = radios[index].ToCharArray();
        var used = new HashSet<int>();

        for (var i = 0; i < 10; i++)
        {
            var oldLetter = letters[i];
            for (var digit = '0'; digit <= '9'; digit++)
            {
                if (digit == oldLetter)
                    continue;
                
                letters[i] = digit;
                var candidate = new string(letters);
                
                if (indexByNumber.TryGetValue(candidate, out var nextIndex) && nextIndex != index && 
                    used.Add(nextIndex)) 
                    yield return nextIndex;
            }
            
            letters[i] = oldLetter;
        }

        for (var i = 0; i < 10; i++)
            for (var j = i + 1; j < 10; j++)
            {
                if (letters[i] == letters[j])
                    continue;
                
                (letters[i], letters[j]) = (letters[j], letters[i]);
                var candidate = new string(letters);
                
                if (indexByNumber.TryGetValue(candidate, out var nextIndex) && nextIndex != index 
                    && used.Add(nextIndex))
                    yield return nextIndex;
                
                (letters[i], letters[j]) = (letters[j], letters[i]);
            }
        
    }

    private static int GetPrefixLength(string first, string second)
    {
        var length = 0;
        while (length < 10 && first[length] == second[length])
            length++;
        
        return length;
    }
}
*/

/*
//3
public class Codeforce4
{
    private static void Main()
    {
        var number = int.Parse(Console.ReadLine()!);
        var records = new List<(string Name, int Difference)>();
        var finalScores = new Dictionary<string, int>();

        for (var i = 0; i < number; i++)
        {
            var parts = Console.ReadLine()!.Split();
            var name = parts[0];
            var difference = int.Parse(parts[1]);
            
            records.Add((name, difference));
            finalScores.TryAdd(name, 0);
            finalScores[name] += difference;
        }
        
        var maxScore = finalScores.Select(pair => pair.Value).Prepend(int.MinValue).Max();
        var currentScores = new Dictionary<string, int>();
        
        foreach (var record in records)
        {
            currentScores.TryAdd(record.Name, 0);
            currentScores[record.Name] += record.Difference;

            if (currentScores[record.Name] < maxScore || finalScores[record.Name] < maxScore) 
                continue;
            
            Console.WriteLine(record.Name);
            return;
        }
    }
}
*/

/*
//4
public class Codeforce4
{
    private class DisjointSetUnion
    {
        private readonly int[] parents;
        private readonly int[] size;

        public DisjointSetUnion(int number)
        {
            parents = new int[number];
            size = new int[number];

            for (var i = 0; i < number; i++)
            {
                parents[i] = i;
                size[i] = 1;
            }
        }

        public int Find(int vertex)
        {
            if (parents[vertex] == vertex)
                return vertex;
            
            parents[vertex] = Find(parents[vertex]);
            return parents[vertex];
        }

        public void Union(int firstElement, int secondElement)
        {
            firstElement = Find(firstElement);
            secondElement = Find(secondElement);
            
            if (firstElement == secondElement)
                return;
            
            if (size[firstElement] < size[secondElement])
                (firstElement, secondElement) = (secondElement, firstElement);
            
            parents[secondElement] = firstElement;
            size[firstElement] += size[secondElement];
        }
    }

    private static void Main()
    {
        var number = int.Parse(Console.ReadLine()!);
        var unionsCount = number * (number - 1) / 2;
        var together = new int[201, 201];
        var exists = new bool[201];

        if (number == 2)
        {
            var numbers = Console.ReadLine()!.Split().Select(int.Parse).ToArray();
            var size = numbers[0];
            var elements = new int[size];
            
            for (var i = 0; i < size; i++)
                elements[i] = numbers[i + 1];
            
            Console.WriteLine("1 " + elements[0]);
            Console.Write(size - 1);
            
            for (var i = 1; i < size; i++)
                Console.Write(" " + elements[i]);
            Console.WriteLine();
            
            return;
        }

        for (var i = 0; i < unionsCount; i++)
        {
            var numbers = Console.ReadLine()!.Split().Select(int.Parse).ToArray();
            var size = numbers[0];
            var elements = new int[size];

            for (var j = 0; j < size; j++)
            {
                elements[j] = numbers[j + 1];
                exists[elements[j]] = true;
            }
            
            for (var s = 0; s < size; s++)
                for (var t = s + 1; t < size; t++)
                {
                    var x = elements[s];
                    var y = elements[t];
                    
                    together[x, y]++;
                    together[y, x]++;
                }
        }
        
        var disjointSetUnion = new DisjointSetUnion(201);
        for (var i = 0; i < 201; i++)
        {
            if (!exists[i])
                continue;

            for (var j = i + 1; j < 201; j++)
            {
                if (!exists[j])
                    continue;
                
                if (together[i, j] == number - 1)
                    disjointSetUnion.Union(i, j);
            }
        }
        
        var groups = new Dictionary<int, List<int>>();
        for (var i = 1; i < 201; i++)
        {
            if (!exists[i])
                continue;

            var root = disjointSetUnion.Find(i);
            if (!groups.ContainsKey(root))
                groups[root] = [];
            
            groups[root].Add(i);
        }

        foreach (var group in groups.Values)
        {
            Console.Write(group.Count);
            foreach (var value in group)
                Console.Write(" " + value);
            
            Console.WriteLine();
        }
    }
}
*/

/*
//5
public class Codeforce4
{
    private static void Main()
    {
        var text = Console.ReadLine()!;
        var number = int.Parse(Console.ReadLine()!);
        var dictionary = new Dictionary<string, string>();

        for (var i = 0; i < number; i++)
        {
            var word = Console.ReadLine()!;
            var key = GetKey(word);
            
            dictionary.TryAdd(key, word);
        }

        var words = text[..^1].Split(' ');
        var answer = new string[words.Length];

        for (var i = 0; i < words.Length; i++)
        {
            var key = GetKey(words[i]);
            if (!dictionary.TryGetValue(key, out var allowedWord))
            {
                Console.WriteLine("No solution");
                return;
            }
            
            answer[i] = allowedWord;
        }
        
        Console.WriteLine(string.Join(" ", answer) + '.');
    }

    private static string GetKey(string word)
    {
        if (word.Length <= 2)
            return word.Length + "|" + word[0] + "|" + word[^1] + "|";
        
        var middle = word.Substring(1, word.Length - 2).ToCharArray();
        Array.Sort(middle);
        
        return word.Length + "|" + word[0] + "|" + word[^1] + "|" + new string(middle);
    }
}
*/