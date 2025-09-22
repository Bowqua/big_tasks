using System.Runtime.CompilerServices;

namespace lecture_C_;

/*
//1
public class NumberOfRoutes
{
    private static int CalculateRoutes(int n)
    {
        var (a, b) = (1, 0);
        for (var i = 1; i <= n; i++)
            (a, b) = (b, a + b);
        
        return b;
    }

    private static void Main()
    {
        var n = int.Parse(Console.ReadLine()!);
        Console.WriteLine(CalculateRoutes(n));
    }
}
*/

/*
//2
public class NumberOfRoutes
{
    private static (long, long) CalculateFib(long n, long p)
    {
        if (n == 0)
            return (0, 1);
        
        var (a, b) = CalculateFib(n / 2, p);
        var c = a * ((2 * b % p - a + p) % p) % p;
        var d = (a * a % p + b * b % p) % p;

        return n % 2 == 0 ? (c, d) : (d, (c + d) % p);
    }

    private static void Main()
    {
        var numbers = Console.ReadLine()!.Split(" ");
        var (fn, _) = CalculateFib(long.Parse(numbers[0]), long.Parse(numbers[1]));
        
        Console.WriteLine(fn % long.Parse(numbers[1]));
    }
}
*/

/*
//3
public class NumberOfRoutes
{
    private static double CalculatePaths(double[,] A, int n)
    {
        var k = n;
        var w = Enumerable.Repeat(1.0, k).ToArray();
        double ratio = 0;

        for (var i = 0; i < 200; i++)
        {
            var wNew = new double[k];
            for (var j = 0; j < k; j++)
                for (var l = 0; l < k; l++)
                    wNew[j] += A[j, l] * w[l];
            
            var numerator = wNew.Sum();
            var denominator = w.Sum();
            
            ratio = numerator / denominator;
            
            w = wNew.Select(x => x / numerator).ToArray();
        }
        
        return ratio;
    }

    private static void Main()
    {
        var number = int.Parse(Console.ReadLine()!);
        var data = new double[number, number];

        for (var i = 0; i < number; i++)
        {
            var row = Console.ReadLine()!.Split(' ').Select(double.Parse).ToArray();
            for (var j = 0; j < number; j++)
                data[i, j] = row[j];
        }
        
        Console.WriteLine(CalculatePaths(data, number));
    }
}
*/

//4
public static class Program
{
    private const int Mod = 1_000_000_007;
    private const double Neg = double.NegativeInfinity;
    private const double Thr = 30.0;
    private const double Step = 0.01;
    const double Invstep = 1.0 / Step;
    private static readonly double[] Log1Pexp = BuildLog1pExp();

    private static double[] BuildLog1pExp()
    {
        var size = (int)(Thr / Step) + 2;
        var t = new double[size];
        
        for (var i = 0; i < size; i++)
        {
            var d = i * Step;
            t[i] = Math.Log(1.0 + Math.Exp(-d));
        }
        
        return t;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double LogSum(double a, double b)
    {
        if (double.IsNegativeInfinity(a)) 
            return b;
        
        if (double.IsNegativeInfinity(b)) 
            return a;

        if (a < b)
            (a, b) = (b, a);
        
        var diff = a - b;
        if (diff >= Thr) 
            return a;
        
        var idx = (int)(diff * Invstep + 0.5);
        if (idx >= Log1Pexp.Length) 
            idx = Log1Pexp.Length - 1;
        
        return a + Log1Pexp[idx];
    }

    private sealed class Matrix
    {
        public readonly int n;
        public readonly long[] m;     
        public readonly double[] lg;  

        public Matrix(int n)
        {
            this.n = n;
            m = new long[n * n];
            lg = new double[n * n];
            Array.Fill(lg, Neg);
        }
    }

    private static Matrix CalculateIdentity(int n)
    {
        var I = new Matrix(n);
        for (var i = 0; i < n; i++)
        {
            var idx = i * n + i;
            I.m[idx] = 1;
            I.lg[idx] = 0.0;
        }
        
        return I;
    }

    private static Matrix Multiply(Matrix firstMatrix, Matrix secondMatrix)
    {
        var n = firstMatrix.n;
        var c = new Matrix(n);

        var Am = firstMatrix.m; 
        var Al = firstMatrix.lg;
        var Bm = secondMatrix.m; 
        var Bl = secondMatrix.lg;
        var Cm = c.m; 
        var Cl = c.lg;

        for (var i = 0; i < n; i++)
        {
            var ioff = i * n;
            for (var t = 0; t < n; t++)
            {
                var aMod = Am[ioff + t];
                var aLog = Al[ioff + t];

                if (aMod == 0 && double.IsNegativeInfinity(aLog))
                    continue;

                var troff = t * n;
                for (var j = 0; j < n; j++)
                {
                    var add = aMod * Bm[troff + j] % Mod;
                    var v = Cm[ioff + j] + add;
                    
                    if (v >= Mod) 
                        v -= Mod;
                    
                    Cm[ioff + j] = v;

                    var x = aLog + Bl[troff + j];
                    if (!double.IsNegativeInfinity(x))
                        Cl[ioff + j] = LogSum(Cl[ioff + j], x);
                }
            }
        }

        return c;
    }

    private static Matrix Power(Matrix matrix, long e)
    {
        var r = CalculateIdentity(matrix.n);
        while (e > 0)
        {
            if ((e & 1) != 0) 
                r = Multiply(r, matrix);
            
            matrix = Multiply(matrix, matrix);
            e >>= 1;
        }
        
        return r;
    }

    private sealed class FastScanner
    {
        private readonly Stream s = Console.OpenStandardInput();
        private readonly byte[] b = new byte[1 << 16];
        int len, ptr;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int Read()
        {
            if (ptr >= len)
            {
                len = s.Read(b, 0, b.Length);
                ptr = 0;
                
                if (len == 0) 
                    return -1;
            }
            
            return b[ptr++];
        }

        public int NextInt()
        {
            int c, sign = 1, x = 0;

            do c = Read();
            while (c <= 32);
            if (c == '-')
                sign = -1; c = Read();
            
            for (; c > 32; c = Read())
                x = x * 10 + (c - '0');

            return x * sign;
        }

        public long NextLong()
        {
            int c, sign = 1;
            long x = 0;

            do c = Read(); 
            while (c <= 32);
            if (c == '-')
                sign = -1; c = Read();
            
            for (; c > 32; c = Read())
                x = x * 10 + (c - '0');

            return x * sign;
        }
    }

    public static void Main()
    {
        var fs = new FastScanner();
        var l = fs.NextLong();
        var k = fs.NextInt();

        var matrix = new Matrix(k);
        for (var i = 0; i < k; i++)
        {
            var rowOff = i * k;
            for (var j = 0; j < k; j++)
            {
                var x = fs.NextInt();         
                matrix.m[rowOff + j] = x;
                matrix.lg[rowOff + j] = (x == 0) ? Neg : 0.0;
            }
        }

        var m = Power(matrix, l);
        const double eps = 1e-12;
        var bestLog = Neg;
        long bestMod = 0;
        int bi = 0, bj = 0;

        for (var i = 0; i < k; i++)
        {
            var off = i * k;
            for (var j = 0; j < k; j++)
            {
                var cur = m.lg[off + j];
                if (cur > bestLog + eps)
                {
                    bestLog = cur;
                    bestMod = m.m[off + j];
                    bi = i; bj = j;
                }
                
                else if (Math.Abs(cur - bestLog) <= eps)
                {
                    if (i < bi || (i == bi && j < bj))
                    {
                        bestMod = m.m[off + j];
                        bi = i; bj = j;
                    }
                }
            }
        }

        Console.WriteLine(bestMod % Mod);
        Console.WriteLine($"{bi} {bj}");
    }
}