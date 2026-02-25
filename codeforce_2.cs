namespace lecture_C_;

/*
//1
public class Codeforce2
{
    private static void Main()
    {
        var dictionary = new Dictionary<string, List<string>>
        {
            { "Slytherin", [] },
            { "Hufflepuff", [] },
            { "Gryffindor", [] },
            { "Ravenclaw", [] }
        };
        var number = int.Parse(Console.ReadLine()!);
        var list = new List<string>();

        for (var i = 0; i < number * 2; i++)
        {
            var name = Console.ReadLine()!;
            if (!dictionary.ContainsKey(name))
            {
                list.Add(name);
                continue;
            }

            dictionary[name].Add(list[0]);
            list.RemoveAt(0);
        }

        var index = 0;
        var total = dictionary.Count;
        
        foreach (var (faculty, names) in dictionary)
        {
            Console.WriteLine($"{faculty}:");

            foreach (var name in names)
                Console.WriteLine($"{name}");
            
            if (index < total - 1)
                Console.WriteLine();
            
            index++;
        }
    }
}
*/

/*
//2
public class Codeforce2
{
    private static void Main()
    {
        var amount = int.Parse(Console.ReadLine()!);
        var listNumbers = Console.ReadLine()!.Split(" ").Select(int.Parse).ToList();
        var dictionary = new Dictionary<int, int>();

        foreach (var number in listNumbers)
        {
            if (!dictionary.TryAdd(number, 1))
                dictionary[number]++;
        }
        
        Console.WriteLine(dictionary.Values.Max());
    }
}
*/

/*
//3
public class Codeforce2
{
    private static void Main()
    {
        using var reader = new StreamReader("homo.in");
        using var writer = new StreamWriter("homo.out");
 
        var number = int.Parse(reader.ReadLine()!);
        var count = new Dictionary<string, int>();
        var distinctCount = 0;
        var duplicateCount = 0;
 
        for (var i = 0; i < number; i++)
        {
            var action = reader.ReadLine()!.Split(" ");
            if (action[0] == "insert")
            {
                if (!count.TryGetValue(action[1], out var countValue))
                {
                    count[action[1]] = 1;
                    distinctCount++;
                }
 
                else
                {
                    if (countValue == 1)
                        duplicateCount++;
                    count[action[1]] = countValue + 1;
                }
            }
 
            else
            {
                if (count.TryGetValue(action[1], out var countValue) && countValue > 0)
                {
                    if (countValue == 2)
                        duplicateCount--;
 
                    if (countValue == 1)
                    {
                        count.Remove(action[1]);
                        distinctCount--;
                    }
 
                    else
                        count[action[1]] = countValue - 1;
                }
            }
 
            if (distinctCount >= 2 && duplicateCount >= 1)
                writer.WriteLine("both");
 
            else if (duplicateCount >= 1)
                writer.WriteLine("homo");
 
            else if (distinctCount >= 2)
                writer.WriteLine("hetero");
 
            else
                writer.WriteLine("neither");
        }
    }
}
*/

/*
//4
public class Codeforce2
{
    private class MultiSet
    {
        private readonly SortedDictionary<long, int> values = [];

        public bool IsEmpty => values.Count == 0;

        public void Add(long number)
        {
            if (values.TryGetValue(number, out var count))
                values[number] = count + 1;

            else
                values[number] = 1;
        }

        public void Remove(long number)
        {
            if (!values.TryGetValue(number, out var count))
                return;

            if (count == 1)
                values.Remove(number);

            else
                values[number] = count - 1;
        }

        public long GetMin()
        {
            var iterator = values.GetEnumerator();
            iterator.MoveNext();

            return iterator.Current.Key;
        }
    }

    private static void ReadPrices(string line, long[] array)
    { 
        var index = 0; 
        var number = 0; 
        var reading = false;
        
        for (var i = 0; i <= line.Length; i++)
        {
            var letter = i == line.Length ? ' ' : line[i];
            if (char.IsDigit(letter))
            {
                number = number * 10 + (letter - '0');
                reading = true;
            }

            else if (reading)
            {
                array[index++] = number;
                number = 0;
                reading = false;
            }
        }
    }

    private static List<int>[] ReadBannedPairs(StreamReader reader, int leftSize) 
    { 
        var pairCount = int.Parse(reader.ReadLine()!); 
        var banned = new List<int>[leftSize];
        
        for (var i = 0; i < leftSize; i++)
                banned[i] = [];

        for (var j = 0; j < pairCount; j++)
        {
            var parts = reader.ReadLine()!.Split(' ');
            var left = int.Parse(parts[0]) - 1;
            var right = int.Parse(parts[1]) - 1;

            banned[left].Add(right);
        }

        return banned;
    }

    private static long[] BuildLayers(long[] currentPrices, long[] nextLayerCosts, List<int>[] bannedPairs) 
    {
        var n = currentPrices.Length; 
        long m = nextLayerCosts.Length; 
        var nextLayerCostsAvailability = new MultiSet();

        for (var i = 0; i < m; i++)
            if (nextLayerCosts[i] < (long)4e18) 
                nextLayerCostsAvailability.Add(nextLayerCosts[i]);

        var resultCosts = new long[n];
        for (var i = 0; i < n; i++)
        {
            foreach (var j in bannedPairs[i].Where(j => nextLayerCosts[j] < (long)4e18))
                nextLayerCostsAvailability.Remove(nextLayerCosts[j]);

            if (nextLayerCostsAvailability.IsEmpty)
                resultCosts[i] = (long)4e18;

            else
                resultCosts[i] = currentPrices[i] + nextLayerCostsAvailability.GetMin();

            foreach (var j in bannedPairs[i].Where(j => nextLayerCosts[j] < (long)4e18))
                nextLayerCostsAvailability.Add(nextLayerCosts[j]);
        }

        return resultCosts;
    }

    private static void Main()
    { 
        using var reader = new StreamReader(Console.OpenStandardInput()); 
        using var writer = new StreamWriter(Console.OpenStandardOutput());
        
        var counts = reader.ReadLine()!.Split(' '); 
        var motherboardCount = int.Parse(counts[0]); 
        var cpuCount = int.Parse(counts[1]); 
        var ramCount = int.Parse(counts[2]); 
        var gpuCount = int.Parse(counts[3]); 
        var motherboardPrice = new long[motherboardCount]; 
        var cpuPrice = new long[cpuCount]; 
        var ramPrice = new long[ramCount]; 
        var gpuPrice = new long[gpuCount];

        ReadPrices(reader.ReadLine()!, motherboardPrice); 
        ReadPrices(reader.ReadLine()!, cpuPrice); 
        ReadPrices(reader.ReadLine()!, ramPrice); 
        ReadPrices(reader.ReadLine()!, gpuPrice);

        var bannedMotherboardCpu = ReadBannedPairs(reader, motherboardCount); 
        var bannedCpuRam = ReadBannedPairs(reader, cpuCount); 
        var bannedRamGpu = ReadBannedPairs(reader, ramCount);
        var ramTotalCost = BuildLayers(ramPrice, gpuPrice, bannedRamGpu); 
        var cpuTotalCost = BuildLayers(cpuPrice, ramTotalCost, bannedCpuRam); 
        var motherboardTotalCost = BuildLayers(motherboardPrice, cpuTotalCost, bannedMotherboardCpu); 
        var answer = motherboardTotalCost.Prepend((long)4e18).Min();

        writer.WriteLine(answer >= (long)4e18 ? -1 : answer);
    }
}
*/