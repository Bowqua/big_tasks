using System.Numerics;

namespace lecture_C_;

/*
//1
public class Permutation
{
    private static void Main()
    {
        var number = int.Parse(Console.ReadLine()!);
        var numbers = Console.ReadLine()!.Split().Select(int.Parse).ToArray();
        var pivot = -1;

        if (number == numbers.Length)
        {
            for (var i = numbers.Length - 2; i >= 0; i--)
            {
                if (numbers[i] < numbers[i + 1])
                {
                    pivot = i;
                    break;
                }
            }

            if (pivot == -1)
            {
                Array.Sort(numbers);
                Console.WriteLine(string.Join(" ", numbers));
                return;
            }

            for (var j = numbers.Length - 1; j > pivot; j--)
            {
                if (numbers[j] > numbers[pivot])
                {
                    (numbers[pivot], numbers[j]) = (numbers[j], numbers[pivot]);
                    break;
                }
            }

            Array.Reverse(numbers, pivot + 1, numbers.Length - pivot - 1);
            Console.WriteLine(string.Join(" ", numbers));
        }
    }
}
*/

/*
//2
public static class Permutation
{
    private static int[] CalculatePermutation(int n, BigInteger t)
    {
        var r = t - BigInteger.One;
        var pool = new List<int>(Enumerable.Range(1, n));
        var permutation = new int[n];
        var factorial = new BigInteger[n];
        factorial[0] = BigInteger.One;
        
        for (var i = 1; i < n; i++)
            factorial[i] = factorial[i - 1] * i;

        for (var position = 0; position < n; position++)
        {
            var baseFactorial = factorial[n - 1 - position];
            var index = (int)(r / baseFactorial);
            r %= baseFactorial;
            
            permutation[position] = pool[index];
            pool.RemoveAt(index);
        }
        
        return permutation;
    }

    private static List<List<int>> DecomposeToCycles(int[] permutation)
    {
        var permutationLength = permutation.Length;
        var visited = new bool[permutationLength + 1];
        var cycles = new List<List<int>>();

        for (var start = 1; start <= permutationLength; start++)
        {
            if (visited[start])
                continue;
            
            var cycle = new List<int>();
            var visit = start;

            while (!visited[visit])
            {
                cycle.Add(visit);
                visited[visit] = true;
                visit = permutation[visit - 1];
            }
                
            cycles.Add(cycle);
        }
        
        return cycles;
    }

    private static void CanonicalizeCycles(List<List<int>> cycles)
    {
        for (var i = 0; i < cycles.Count; i++)
        {
            var cycle = cycles[i];
            int max = cycle[0], position = 0;
            
            for (var j = 1; j < cycle.Count; j++)
                if (cycle[j] > max)
                {
                    max = cycle[j];
                    position = j;
                }
            
            var rotated = new List<int>(cycle.Count);
            rotated.AddRange(cycle.GetRange(position, cycle.Count - position));
            rotated.AddRange(cycle.GetRange(0, position));
            cycles[i] = rotated;
        }
        
        cycles.Sort((a, b) => a[0].CompareTo(b[0]));
    }

    private static void PrintCycles(List<List<int>> cycles)
    {
        var parts = new List<string>(cycles.Count);
        parts.AddRange(cycles.Select(cycle => "( " + string.Join(" ", cycle) + " )"));

        Console.WriteLine(string.Join(" ", parts));
    }

    private static void Main()
    {
        var n = int.Parse(Console.ReadLine()!);
        var t = BigInteger.Parse(Console.ReadLine()!);
        
        var permutation = CalculatePermutation(n, t);
        var cycles = DecomposeToCycles(permutation);
        
        CanonicalizeCycles(cycles);
        PrintCycles(cycles);
    }
}
*/

/*
//3
public static class Permutation
{
    private static int CalculateGcd(int a, int b)
    {
        while (b != 0)
            (a, b) = (b, a % b);
        
        return a;
    }

    private static int GcdOfRuns(string s)
    {
        int n = s.Length, g = 0, length = 1;
        for (var i = 1; i < n; i++)
        {
            if (s[i] == s[i-1])
                length++;

            else
            {
                g = g == 0 ? length : CalculateGcd(g, length);
                length = 1;
            }
        }
        
        g = g == 0 ? length : CalculateGcd(g, length);
        return g;
    }

    private static int[] SortPermutation(string s)
    {
        var n = s.Length;
        var buckets = new List<int>[26];
        
        for (var i = 0; i < 26; i++)
            buckets[i] = [];
        
        for (var i = 0; i < n; i++)
            buckets[s[i] - 'a'].Add(i);
        
        var sortedIndex = new int[n];
        var k = 0;
        for (var i = 0; i < 26; i++)
            foreach (var index in buckets[i])
                sortedIndex[k++] = index;
        
        var permutation = new int[n];
        for (var rank = 0; rank < n; rank++)
            permutation[sortedIndex[rank]] = rank;
        
        return permutation;
    }

    private static int CountCycles(int[] permutation)
    {
        int n = permutation.Length, cycles = 0;
        var visited = new bool[n];

        for (var i = 0; i < n; i++)
        {
            if (visited[i]) 
                continue;
            
            cycles++;
            var visit = i;

            while (!visited[visit])
            {
                visited[visit] = true;
                visit = permutation[visit];
            }
        }
        
        return cycles;
    }

    private static void Main()
    {
        var n = int.Parse(Console.ReadLine()!);
        var b = Console.ReadLine()!.Trim();

        if (n == b.Length)
        {
            var gcdRuns = GcdOfRuns(b);
            var permutation = SortPermutation(b);
            var cycles = CountCycles(permutation);
        
            Console.WriteLine(cycles == gcdRuns ? "Yes" : "No");
        }
    }
}
*/

/*
//4
public static class Permutation
{
    private static List<int> GetPrimesUpTo(int limit)
    {
        var isComposite = new bool[limit + 1];
        var primes = new List<int>();
        
        for (var i = 2; i <= limit; i++)
        {
            if (!isComposite[i])
            {
                primes.Add(i);
                if ((long)i * i <= limit)
                    for (var j = i * i; j <= limit; j += i)
                        isComposite[j] = true;
            }
        }
        
        return primes;
    }

    public static void Main()
    {
        var n = int.Parse(Console.ReadLine()!.Trim());
        var primePowerGroups = new List<List<int>>();
        
        foreach (var prime in GetPrimesUpTo(n))
        {
            var powers = new List<int>();
            var value = prime;
            
            while (value <= n)
            {
                powers.Add(value);
                value *= prime;
            }
            
            primePowerGroups.Add(powers);
        }

        var dp = new BigInteger[n + 1];
        dp[0] = BigInteger.One;

        foreach (var group in primePowerGroups)
        {
            var next = new BigInteger[n + 1];
            for (var cap = 0; cap <= n; cap++)
                next[cap] = dp[cap];

            foreach (var weight in group)
            {
                for (var cap = weight; cap <= n; cap++)
                {
                    if (dp[cap - weight] != BigInteger.Zero)
                    {
                        BigInteger candidate = dp[cap - weight] * weight;
                        if (candidate > next[cap]) 
                            next[cap] = candidate;
                    }
                }
            }

            dp = next;
        }

        var bestLcm = BigInteger.One;
        for (var cap = 1; cap <= n; cap++)
            if (dp[cap] > bestLcm) 
                bestLcm = dp[cap];

        Console.WriteLine(bestLcm);
    }
}
*/
