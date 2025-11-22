namespace lecture_C_;

/*
//1 
public class Contest
{
    private static string GetText() => Console.ReadLine()!.Trim();

    private static void Main()
    {
        var text = GetText();
        var currentSum = 0;
        var result = 0;
        var count = text.Count(symbol => symbol == 'M');

        for (var i = 1; i <= count; i++)
        {
            if (currentSum + i > count)
                break;
            
            currentSum += i;
            result++;
        }
        
        Console.WriteLine(result);
    }
}
*/

/*
//2
public class Contest
{
    private static List<string> GetNumbers()
    {
        var countNumbers = int.Parse(Console.ReadLine()!);
        var preparingList = new List<string>();
        
        for (var i = 0; i < countNumbers; i++)
            preparingList.Add(Console.ReadLine()!.Trim());
        
        return preparingList;
    }

    private static void Main()
    {
        var listOfNumbers = GetNumbers();
        listOfNumbers.Sort((a, b) => string.Compare(b + a, a + b, StringComparison.Ordinal));
        
        Console.WriteLine(string.Join("", listOfNumbers));
    }
}
*/

//3
public class Contest
{
    private const string symbols = "@!$&%#^*()";
    private static List<string> listSymbols = [];
    private static List<(int left, int right)> constraints = [];
    private static bool[] cannotBeZero = new bool[10];
    private static bool[] usedDigit = new bool[10];
    private static int[] mapping = new int[10];   
    private static int[] permutated = new int[10];
    private static bool found;

    private static bool CheckPermutation()
    {
        string? previous = null;
        foreach (var encrypted in listSymbols)
        {
            var letters = new char[encrypted.Length];
            for (var i = 0; i < encrypted.Length; i++)
            {
                var symbol = encrypted[i];
                var index = symbols.IndexOf(symbol);
                var digit = mapping[index];

                if (i == 0 && encrypted.Length > 1 && digit == 0)
                    return false;

                letters[i] = (char)('0' + digit);
            }

            var current = new string(letters);
            if (previous != null && !IsLess(previous, current))
                return false;

            previous = current;
        }

        return true;
    }

    private static bool IsLess(string a, string b)
    {
        if (a.Length != b.Length)
            return a.Length < b.Length;

        return string.CompareOrdinal(a, b) < 0;
    }

    private static bool CheckConstraintsPartial()
    {
        foreach (var (left, right) in constraints)
        {
            var dleft = mapping[left];
            var dright = mapping[right];

            if (dleft == -1 || dright == -1)
                continue;

            if (dleft >= dright)
                return false;
        }

        return true;
    }

    private static void Dfs(int position)
    {
        if (found)
            return;

        if (position == 10)
        {
            if (CheckPermutation())
            {
                for (var i = 0; i < 10; i++)
                    permutated[i] = mapping[i];
                found = true;
            }
            
            return;
        }

        for (var digit = 0; digit <= 9; digit++)
        {
            if (usedDigit[digit])
                continue;

            if (cannotBeZero[position] && digit == 0)
                continue;

            mapping[position] = digit;
            usedDigit[digit] = true;

            if (CheckConstraintsPartial())
                Dfs(position + 1);

            usedDigit[digit] = false;
            mapping[position] = -1;

            if (found)
                return;
        }
    }

    private static void Main()
    {
        var numberCount = int.Parse(Console.ReadLine()!);
        for (var i = 0; i < numberCount; i++)
        {
            var text = Console.ReadLine()!;
            listSymbols.Add(text);

            if (text.Length > 1)
            {
                var firstSymbol = text[0];
                var index = symbols.IndexOf(firstSymbol);
                if (index >= 0)
                    cannotBeZero[index] = true;
            }
        }

        for (var i = 0; i + 1 < listSymbols.Count; i++)
        {
            var a = listSymbols[i];
            var b = listSymbols[i + 1];

            if (a.Length > b.Length)
            {
                Console.WriteLine("NO");
                return;
            }

            if (a.Length == b.Length)
            {
                var j = 0;
                while (j < a.Length && a[j] == b[j])
                    j++;

                if (j == a.Length)
                {
                    Console.WriteLine("NO");
                    return;
                }

                var left = symbols.IndexOf(a[j]);
                var right = symbols.IndexOf(b[j]);
                constraints.Add((left, right));
            }
        }

        for (var i = 0; i < 10; i++)
            mapping[i] = -1;

        Dfs(0);

        if (!found)
            Console.WriteLine("NO");
        
        else
        {
            Console.WriteLine("YES");
            Console.WriteLine(string.Join(" ", permutated));
        }
    }
}
