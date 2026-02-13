using System.Text;

namespace lecture_C_;

/*
//1
public class Algorithms1
{
    private static void Main()
    {
        var number = int.Parse(Console.ReadLine()!);
        Console.WriteLine(number - 5 * 5 + 25);
    }
}
*/

/*
//2
public class Algorithms1
{
    private static int[] GetNumbers()
    {
        var budget = int.Parse(Console.ReadLine()!);
        var moneyForIron = int.Parse(Console.ReadLine()!);
        var moneyForWood = int.Parse(Console.ReadLine()!);

        return [budget, moneyForIron, moneyForWood];
    }

    private static void Main()
    {
        var numbers = GetNumbers();
        var total = numbers[1] + numbers[2];
        var result = 0;

        if (numbers[0] >= total)
        {
            while (numbers[0] >= total)
            {
                result += 1;
                numbers[0] -= total;
            }
        }
        
        Console.WriteLine(result);
    }
}
*/

/*
//3
public class Algorithms1
{
    private static double[] GetTotal()
    {
        var first = double.Parse(Console.ReadLine()!);
        var second = double.Parse(Console.ReadLine()!);
        var third = double.Parse(Console.ReadLine()!);
        var fourth = double.Parse(Console.ReadLine()!);
        
        return [first, second, third, fourth];
    }

    private static void Main()
    {
        var total = GetTotal();
        var sum = total.Sum();
        
        Console.WriteLine((total[2] + total[3]) / sum * 100);
    }
}
*/

/*
//4
public class Algorithms1
{
    private static string[] GetAction()
    {
        var firstAction = Console.ReadLine()!;
        var secondAction = Console.ReadLine()!;
        
        return [firstAction, secondAction];
    }

    private static void Main()
    {
        var actions = GetAction();
        switch (actions[0])
        {
            case "Rock" when actions[1] == "Rock":
                Console.WriteLine("Tie");
                break;
            case "Rock" when actions[1] == "Scissors":
            case "Rock" when actions[1] == "Lizard":
                Console.WriteLine("First");
                break;
            case "Lizard" when actions[1] == "Lizard":
                Console.WriteLine("Tie");
                break;
            case "Lizard" when actions[1] == "Spock":
            case "Lizard" when actions[1] == "Paper":
                Console.WriteLine("First");
                break;
            case "Spock" when actions[1] == "Spock":
                Console.WriteLine("Tie");
                break;
            case "Spock" when actions[1] == "Rock":
            case "Spock" when actions[1] == "Scissors":
                Console.WriteLine("First");
                break;
            case "Scissors" when actions[1] == "Scissors":
                Console.WriteLine("Tie");
                break;
            case "Scissors" when actions[1] == "Lizard":
            case "Scissors" when actions[1] == "Paper":
                Console.WriteLine("First");
                break;
            case "Paper" when actions[1] == "Paper":
                Console.WriteLine("Tie");
                break;
            case "Paper" when actions[1] == "Spock":
            case "Paper" when actions[1] == "Rock":
                Console.WriteLine("First");
                break;
            default:
                Console.WriteLine("Second");
                break;
        }
    }
}
*/

/*
//5
public class Algorithms1
{
    private static int[] CalculateAesthetic(int standard, int departure, int amount)
    {
        var result = new List<int>();
        while (amount > 0)
        {
            var number = int.Parse(Console.ReadLine()!);
            if (Math.Abs(number - standard) > departure)
                result.Add(number);

            amount--;
        }

        return result.ToArray();
    }

    private static void Main()
    {
        var standard = int.Parse(Console.ReadLine()!);
        var departure = int.Parse(Console.ReadLine()!);
        var amount = int.Parse(Console.ReadLine()!);
        var result = CalculateAesthetic(standard, departure, amount);

        foreach (var number in result)
            Console.WriteLine(number);
    }
}
*/

/*
//6
public class Algorithms1
{
    private static int CalculatePosition(int amount, int start)
    {
        var current = start;
        while (amount > 0)
        {
            var positions = Console.ReadLine()!.Split(" ").Select(int.Parse).ToList();
            if (current == positions[0])
                current = positions[1];
            
            else if (current == positions[1])
                current = positions[0];
            
            amount--;
        }
        
        return current;
    }

    private static void Main()
    {
        var start = Console.ReadLine()!.Split(" ").Select(int.Parse).ToList();
        var result = CalculatePosition(start[0], start[1]);
        
        Console.WriteLine(result);
    }
}
*/

/*
//7
public class Algorithms1
{
    private static int[] CalculateLazyWork(int days, int subjects, int[] homework)
    {
        var last = new int[subjects + 1];
        for (var i = 0; i <= subjects; i++)
            last[i] = -1;

        for (var i = 0; i < days; i++)
        {
            var subject = homework[i];
            last[subject] = i;
        }

        var result = new int[days];
        for (var i = 0; i < days; i++)
        {
            var subject = homework[i];
            result[i] = i == last[subject] ? subject : 0;
        }
        
        return result;
    }

    private static void Main()
    {
        var start = Console.ReadLine()!.Split(' ').Select(int.Parse).ToArray();
        var days = start[0];
        var subjects = start[1];
        var homework = Console.ReadLine()!.Split(' ').Select(int.Parse).ToArray();
        
        var result = CalculateLazyWork(days, subjects, homework);
        Console.WriteLine(string.Join(" ", result));
    }
}
*/

/*
//8
public class Algorithms1
{
    private static void Main()
    {
        var password = long.Parse(Console.ReadLine()!);
        for (var mask = 0; mask < 65536; mask++)
        {
            long hash = 0;
            for (var i = 15; i >= 0; i--)
            {
                var bit = (mask >> i) & 1;
                var a = bit == 0 ? 48 : 49;
                
                hash = (hash * 257 + a) % 1000000000000159;
            }

            if (password == hash)
            {
                var result = new StringBuilder(16);
                for (var j = 15; j >= 0; j--)
                    result.Append((mask >> j) & 1);
                    
                Console.WriteLine(result.ToString());
                return;
            }
        }
        
        Console.WriteLine("No");
    }
}
*/

/*
//9
public class Algorithms1
{
    private static void Main()
    {
        var numbers = Console.ReadLine()!.Split(" ").Select(int.Parse).ToArray();
        var vertexes = numbers[0];
        var edges = numbers[1];
        var adjacencyList = new List<int>[vertexes];
        
        for (var i = 0; i < vertexes; i++)
            adjacencyList[i] = [];

        for (var i = 0; i < edges; i++)
        {
            var variants = Console.ReadLine()!.Split(" ").Select(int.Parse).ToArray();
            adjacencyList[variants[0] - 1].Add(variants[1] - 1);
            adjacencyList[variants[1] - 1].Add(variants[0] - 1);
        }
        
        var visited = new bool[vertexes];

        Dfs(0, -1);
        Console.WriteLine(-1);
        return;

        void Dfs(int edge, int parent)
        {
            visited[edge] = true;
            foreach (var vertex in adjacencyList[edge])
            {
                if (!visited[vertex])
                    Dfs(vertex, edge);
                
                else if (vertex != parent)
                {
                    Console.WriteLine($"{edge + 1} {vertex + 1}");
                    Environment.Exit(0);
                }
            }
        }
    }
}
*/

//10
public class Algorithms1
{
    private static void Main()
    {
        var amount = int.Parse(Console.ReadLine()!);
        List<(int start, int finish, int power)> listOfLessons = [];

        for (var i = 0; i < amount; i++)
        {
            var numbers = Console.ReadLine()!.Split(' ').Select(int.Parse).ToList();
            listOfLessons.Add((numbers[0], numbers[1], numbers[2]));
        }
        
        listOfLessons.Sort((a, b) => a.finish.CompareTo(b.finish));
        var finishes = new int[listOfLessons.Count];
        
        for (var i = 0; i < listOfLessons.Count; i++)
            finishes[i] = listOfLessons[i].finish;
        
        var result = new int[listOfLessons.Count];
        for (var i = 0; i < listOfLessons.Count; i++)
        {
            var index = Array.BinarySearch(finishes, listOfLessons[i].start);

            if (index < 0)
                index = ~index - 1;
            
            else if (index >= i)
                index = i - 1;
            
            var take = listOfLessons[i].power + (index >= 0 ? result[index] : 0);
            var skip = i > 0 ? result[i - 1] : 0;
            
            result[i] = Math.Max(take, skip);
        }
        
        Console.WriteLine(result[listOfLessons.Count - 1]);    
    }
}