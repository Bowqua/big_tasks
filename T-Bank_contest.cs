using System.Drawing;
using System.Text;

namespace lecture_C_;

/*
//1
public class Contest
{
    private static string GetText() => Console.ReadLine()!;
    
    private static void Main()
    {
       var textWithNumbers = GetText().ToCharArray();
       Array.Sort(textWithNumbers);
       
       if (textWithNumbers[0] == '0')
           for (var i = 1; i < textWithNumbers.Length; i++)
               if (textWithNumbers[i] != '0')
               {
                   (textWithNumbers[0], textWithNumbers[i]) = (textWithNumbers[i], textWithNumbers[0]);
                   break;
               }
       
       Console.WriteLine(textWithNumbers);
    }
}
*/

/*
//3
public class Contest
{
    private static int GetNumber() => int.Parse(Console.ReadLine()!);

    private static void Main()
    {
        var number = GetNumber();
        var result = new List<long>();
        
        while (number-- > 0)
        {
            var letters = Console.ReadLine();
            var n = letters.Length;
            int maximumRun;
            
            if (letters.IndexOf("0") == -1)
                maximumRun = n;
            
            else if (letters.IndexOf("1") == -1)
                maximumRun = 0;

            else
            {
                var joinedStrings = letters + letters;
                var current = 0;
                maximumRun = 0;

                for (var i = 0; i < joinedStrings.Length; i++)
                {
                    if (joinedStrings[i] == '1')
                    {
                        current++;
                        if (current > n)
                            current = n;
                        
                        if (current > maximumRun)
                            maximumRun = current;
                    }
                    
                    else 
                        current = 0;
                }
            }

            if (maximumRun == 0)
            {
                result.Add(0);
                continue;
            }
            
            var maxSum = maximumRun + 1L;
            var a = maxSum / 2;
            var b = maxSum - a;
            
            result.Add(a * b);
        }
        
        foreach (var item in result)
            Console.WriteLine(item);
    }
}
*/

/*
//4
public class Contest
{
    private static void Main()
    {
        var firstRow = Console.ReadLine()!.Split();
        var vertex = int.Parse(firstRow[0]);
        var edge = int.Parse(firstRow[1]);
        var graph = new List<int>[vertex + 1];
        
        for (var i = 1; i <= vertex; i++)
            graph[i] = new List<int>();

        for (var i = 0; i < edge; i++)
        {
            var parts = Console.ReadLine()!.Split();
            var a = int.Parse(parts[0]);
            var b = int.Parse(parts[1]);
            
            graph[a].Add(b);
            graph[b].Add(a);
        }

        var result = int.MaxValue;
        var distance = new int[vertex + 1];
        var parent = new int[vertex + 1];
        var queue = new Queue<int>(vertex);

        for (var i = 1; i <= vertex; i++)
        {
            Array.Fill(distance, -1);
            Array.Fill(parent, -1);
            queue.Clear();

            distance[i] = 0;
            queue.Enqueue(i);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach (var to in graph[current])
                {
                    if (distance[to] == -1)
                    {
                        distance[to] = distance[current] + 1;
                        parent[to] = current;
                        queue.Enqueue(to);
                    }
                    
                    else if (to != parent[current])
                    {
                        var lenCycle = distance[to] + distance[current] + 1;
                        if (lenCycle < result)
                            result = lenCycle;
                    }
                }
            }
        }
        
        Console.WriteLine(result == int.MaxValue ? -1 : result);
    }
}
*/

/*
//6
public class Contest
{
    private static void Main()
    {
        var numbers = Console.ReadLine()!.Split(" ");
        var amountOperations = int.Parse(numbers[1]);
        var text = Console.ReadLine()!;
        var result = new List<char>();

        while (amountOperations-- > 0)
        {
            var query = Console.ReadLine()!.Split(" ");
            if (query[0] == "1")
            {
                var from = int.Parse(query[1]);
                var to = int.Parse(query[2]);
                var middle = text.Substring(from - 1, to - from + 1);
                text = text.Remove(from - 1, to - from + 1);
                var stringBuilder = new StringBuilder();

                foreach (var letter in middle)
                {
                    stringBuilder.Append(letter);
                    stringBuilder.Append(letter);
                }
                
                text = text.Insert(from - 1, stringBuilder.ToString());
            } 
            
            else if (query[0] == "2")
            {
                var position = int.Parse(query[1]);
                result.Add(text[position - 1]);
            }
        }
        
        foreach (var letter in result)
            Console.WriteLine(letter);
    }
}
*/

/*
//7
public class Contest
{
    const long mod = 1_000_000_007;
    private static long[] CountWaysForOneColor(int boardSize, int amountBishops, int cellColor)
    {
        var cellsOnDiagonal = new List<int>[2 * boardSize - 1];
        for (var i = 0; i < cellsOnDiagonal.Length; i++)
            cellsOnDiagonal[i] = new List<int>();
        
        for (var row = 0; row < boardSize; row++)
            for (var column = 0; column < boardSize; column++)
            {
                if ((row + column) % 2 != cellColor)
                    continue;
                
                var mainDiagonal = row + column;
                var antiDiagonal = row - column + (boardSize - 1);
                
                cellsOnDiagonal[mainDiagonal].Add(antiDiagonal);
            }

        var dp = new Dictionary<long, long[]>();
        var startWays = new long[amountBishops + 1];
        startWays[0] = 1;
        dp[0L] = startWays;

        for (var mainDiagonal = 0; mainDiagonal < cellsOnDiagonal.Length; mainDiagonal++)
        {
            var nextDp = new Dictionary<long, long[]>();
            foreach (var state in dp)
            {
                var usedAntiDiagonal = state.Key;
                var waysByCount = state.Value;

                if (!nextDp.TryGetValue(usedAntiDiagonal, out var stayWays))
                {
                    stayWays = new long[amountBishops + 1];
                    nextDp[usedAntiDiagonal] = stayWays;
                }
                
                for (var bishopPlaced = 0; bishopPlaced <= amountBishops; bishopPlaced++)
                    stayWays[bishopPlaced] = (stayWays[bishopPlaced] += waysByCount[bishopPlaced]) % mod;

                foreach (var antiDiagonal in cellsOnDiagonal[mainDiagonal])
                {
                    var bit = 1L << antiDiagonal;
                    if ((usedAntiDiagonal & bit) != 0)
                        continue;
                    
                    var newUsed = usedAntiDiagonal | bit;
                    if (!nextDp.TryGetValue(newUsed, out var putWays))
                    {
                        putWays = new long[amountBishops + 1];
                        nextDp[newUsed] = putWays;
                    }
                    
                    for (var bishopPlaced = 0; bishopPlaced < amountBishops; bishopPlaced++)
                        putWays[bishopPlaced + 1] = (putWays[bishopPlaced + 1] += waysByCount[bishopPlaced]) % mod;
                }
            }
            
            dp = nextDp;
        }
        
        var result = new long[amountBishops + 1];
        foreach (var state in dp.Values)
            for (var i = 0; i <= amountBishops; i++)
                result[i] = (result[i] + state[i]) % mod;
        
        return result;
    }
    
    private static void Main()
    {
        var numbers = Console.ReadLine()!.Split(" ");
        var boardSize = int.Parse(numbers[0]);
        var bishops = int.Parse(numbers[1]);
        var blackWays = CountWaysForOneColor(boardSize, bishops, 0);
        var whiteWays = CountWaysForOneColor(boardSize, bishops, 1);
        long result = 0;
        
        for (var i = 0; i <= bishops; i++)
            result = (result + blackWays[i] * whiteWays[bishops - i]) % mod;
        
        Console.WriteLine(result);
    }
}
*/

//2
public class Contest
{
    private static int Replace(string first, string second) => 
        first.Where((t, i) => t != second[i]).Count();

    private static void Main()
    {
        var text = Console.ReadLine();
        var result = Replace(text, @"\");
    }
}