using System.Text;

namespace lecture_C_;

/*
//1
public class Codeforce5
{
    private static int[] segmentTree;
    private static char[] text;
    private static int textLength;

    private static void Main()
    {
        text = Console.ReadLine()!.Trim().ToCharArray();
        textLength = text.Length;
        var queriesCount = int.Parse(Console.ReadLine()!);
        segmentTree = new int[4 * textLength];
        
        BuildTree(1, 0, textLength - 1);

        var result = new StringBuilder();
        for (var queryIndex = 0; queryIndex < queriesCount; queryIndex++)
        {
            var parts = Console.ReadLine()!.Split();
            var queryType = int.Parse(parts[0]);

            if (queryType == 1)
            {
                var position = int.Parse(parts[1]) - 1;
                var newLetter = parts[2][0];
                
                text[position] = newLetter;
                UpdateLetter(1, 0, textLength - 1, position, newLetter);
            }

            else
            {
                var left = int.Parse(parts[1]) - 1;
                var right = int.Parse(parts[2]) - 1;
                var mask = QueryRange(1, 0, textLength - 1, left, right);
                var distinctLetters = CountBits(mask);
                
                result.AppendLine(distinctLetters.ToString());
            }
        }
        
        Console.WriteLine(result.ToString());
    }

    private static int CreateLetterMask(char letter) => 1 << (letter - 'a');

    private static void BuildTree(int nodeIndex, int segmentLeft, int segmentRight)
    {
        if (segmentLeft == segmentRight)
        {
            segmentTree[nodeIndex] = CreateLetterMask(text[segmentLeft]);
            return;
        }
        
        var middle = (segmentLeft + segmentRight) / 2;
        BuildTree(nodeIndex * 2, segmentLeft, middle);
        BuildTree(nodeIndex * 2 + 1, middle + 1, segmentRight);
        segmentTree[nodeIndex] = segmentTree[nodeIndex * 2] | segmentTree[nodeIndex * 2 + 1];
    }

    private static void UpdateLetter(
        int nodeIndex,
        int segmentLeft,
        int segmentRight,
        int targetPosition,
        char newLetter)
    {
        if (segmentLeft == segmentRight)
        {
            segmentTree[nodeIndex] = CreateLetterMask(newLetter);
            return;
        }
        
        var middle = (segmentLeft + segmentRight) / 2;
        if (targetPosition <= middle)
            UpdateLetter(nodeIndex * 2, segmentLeft, middle, targetPosition, newLetter);
        
        else 
            UpdateLetter(nodeIndex * 2 + 1, middle + 1, segmentRight, targetPosition, newLetter);
        
        segmentTree[nodeIndex] = segmentTree[nodeIndex * 2] | segmentTree[nodeIndex * 2 + 1];
    }

    private static int QueryRange(
        int nodeIndex,
        int segmentLeft,
        int segmentRight,
        int queryLeft,
        int queryRight)
    {
        if (queryLeft > queryRight)
            return 0;
        
        if (queryLeft == segmentLeft && queryRight == segmentRight)
            return segmentTree[nodeIndex];
        
        var middle = (segmentLeft + segmentRight) / 2;
        var leftMask = QueryRange(nodeIndex * 2, segmentLeft, middle, queryLeft, Math.Min(queryRight, middle));
        var rightMask = QueryRange(nodeIndex * 2 + 1, middle + 1, segmentRight, Math.Max(queryLeft, middle + 1), queryRight);
        
        return leftMask | rightMask;
    }

    private static int CountBits(int mask)
    {
        var count = 0;
        while (mask > 0)
        {
            count += mask & 1;
            mask >>= 1;
        }
        
        return count;
    }
}
*/

/*
//2
public class Codeforce5
{
    private static int[] segmentTree;
    private static int[] numbers;

    private static void Main()
    {
        var firstRow = Console.ReadLine()!.Split();
        var height = int.Parse(firstRow[0]);
        var queriesCount = int.Parse(firstRow[1]);
        var arrayLength = 1 << height;
        numbers = Array.ConvertAll(Console.ReadLine()!.Split(), int.Parse);
        segmentTree = new int[arrayLength * 4];
        
        BuildTree(1, 0, arrayLength - 1, height % 2 == 1);
        
        var result = new StringBuilder();
        for (var queryIndex = 0; queryIndex < queriesCount; queryIndex++)
        {
            var queryParts = Console.ReadLine()!.Split();
            var position = int.Parse(queryParts[0]) - 1;
            var newValue = int.Parse(queryParts[1]);
            
            UpdateTree(1, 0, arrayLength - 1, position, newValue, height % 2 == 1);
            result.AppendLine(segmentTree[1].ToString());
        }
        
        Console.WriteLine(result.ToString());
    }

    private static void BuildTree(int nodeIndex, int segmentLeft, int segmentRight, bool orOrNot)
    {
        if (segmentLeft == segmentRight)
        {
            segmentTree[nodeIndex] = numbers[segmentLeft];
            return;
        }
        
        var middle = (segmentLeft + segmentRight) / 2;
        BuildTree(nodeIndex * 2, segmentLeft, middle, !orOrNot);
        BuildTree(nodeIndex * 2 + 1, middle + 1, segmentRight, !orOrNot);
        
        if (orOrNot)
            segmentTree[nodeIndex] = segmentTree[nodeIndex * 2] | segmentTree[nodeIndex * 2 + 1];
        else 
            segmentTree[nodeIndex] = segmentTree[nodeIndex * 2] ^ segmentTree[nodeIndex * 2 + 1];
    }

    private static void UpdateTree(
        int nodeIndex,
        int segmentLeft,
        int segmentRight,
        int targetPosition,
        int newValue,
        bool orOrNot)
    {
        if (segmentLeft == segmentRight)
        {
            segmentTree[nodeIndex] = newValue;
            return;
        }
        
        var middle = (segmentLeft + segmentRight) / 2;
        if (targetPosition <= middle)
            UpdateTree(nodeIndex * 2, segmentLeft, middle, targetPosition, newValue, !orOrNot);
        else 
            UpdateTree(nodeIndex * 2 + 1, middle + 1, segmentRight, targetPosition, newValue, !orOrNot);
        
        if (orOrNot)
            segmentTree[nodeIndex] = segmentTree[nodeIndex * 2] | segmentTree[nodeIndex * 2 + 1];
        else 
            segmentTree[nodeIndex] = segmentTree[nodeIndex * 2] ^ segmentTree[nodeIndex * 2 + 1];
    }
}
*/

/*
//3
public class Codeforce5
{
    private class FenwickTree
    {
        private readonly long[] tree;

        public FenwickTree(int size)
        {
            tree = new long[size + 1];
        }

        public void Add(int index, long delta)
        {
            while (index < tree.Length)
            {
                tree[index] += delta;
                index += index & -index;
            }
        }

        public long GetPrefixSum(int index)
        {
            long sum = 0;
            while (index > 0)
            {
                sum += tree[index];
                index -= index & -index;
            }
            
            return sum;
        }
    }
    
    private static void Main()
    {
        var titansCount = int.Parse(Console.ReadLine()!);
        var heights = Array.ConvertAll(Console.ReadLine()!.Split(), int.Parse);
        var compressedHeights = CompressValues(heights);
        var greaterLeft = new long[titansCount];
        var smallerRight = new long[titansCount];
        
        CountGreaterLeft(compressedHeights, greaterLeft);
        CountSmallRight(compressedHeights, smallerRight);

        long result = 0;
        for (var index = 0; index < titansCount; index++)
            result += greaterLeft[index] * smallerRight[index];
        
        Console.WriteLine(result);
    }

    private static int[] CompressValues(int[] values)
    {
        var sortedValues = (int[])values.Clone();
        Array.Sort(sortedValues);
        
        var valueToRank = new Dictionary<int, int>(values.Length);
        for (var index = 0; index < sortedValues.Length; index++)
            valueToRank[sortedValues[index]] = index + 1;
        
        var compressedValues = new int[values.Length];
        for (var index = 0; index < values.Length; index++)
            compressedValues[index] = valueToRank[values[index]];
        
        return compressedValues;
    }

    private static void CountGreaterLeft(int[] compressedHeights, long[] greaterLeft)
    {
        var length = compressedHeights.Length;
        var fenwickTree = new FenwickTree(length);

        for (var index = 0; index < length; index++)
        {
            var currentRank = compressedHeights[index];
            var processedCount = index;
            var notGreaterCount = fenwickTree.GetPrefixSum(currentRank);
            
            greaterLeft[index] = processedCount - notGreaterCount;
            fenwickTree.Add(currentRank, 1);
        }
    }

    private static void CountSmallRight(int[] compressedHeights, long[] smallRight)
    {
        var length = compressedHeights.Length;
        var fenwickTree = new FenwickTree(length);

        for (var index = length - 1; index >= 0; index--)
        {
            var currentRank = compressedHeights[index];
            smallRight[index] = fenwickTree.GetPrefixSum(currentRank - 1);
            fenwickTree.Add(currentRank, 1);
        }
    }
}
*/

//4
public class Codeforce5
{
    private static int[] segmentTree;
    private static int[] lazyAssignedSongId;
    private static int[] songsByMinutes;
    private static string[] songNamesById;
    private const int MixedSongId = -1;
    private const int NoLazyUpdate = -2;

    private static void Main()
    {
        var firstLine = Console.ReadLine()!.Split();
        var totalMinutes = int.Parse(firstLine[0]);
        var songsCount = int.Parse(firstLine[1]);
        var commandsCount = int.Parse(firstLine[2]);
        var songIdByName = new Dictionary<string, int>();
        var nextSongId = 1;
        var currentMinuteIndex = 0;
        songsByMinutes = new int[totalMinutes];
        songNamesById = new string[songsCount + commandsCount + 5];

        for (var songIndex = 0; songIndex < songsCount; songIndex++)
        {
            var parts = Console.ReadLine()!.Split();
            var songName = parts[0];
            var duration = int.Parse(parts[1]);

            if (!songIdByName.ContainsKey(songName))
            {
                songIdByName[songName] = nextSongId;
                songNamesById[nextSongId] = songName;
                nextSongId++;
            }
            
            var songId = songIdByName[songName];
            for (var minute = 0; minute < duration; minute++)
            {
                songsByMinutes[currentMinuteIndex] = songId;
                currentMinuteIndex++;
            }
        }
        
        segmentTree = new int[totalMinutes * 4];
        lazyAssignedSongId = new int[totalMinutes * 4];
        
        for (var index = 0; index < lazyAssignedSongId.Length; index++)
            lazyAssignedSongId[index] = NoLazyUpdate;
        
        BuildTree(1, 0, totalMinutes - 1);
        
        var result = new StringBuilder();
        for (var index = 0; index < commandsCount; index++)
        {
            var parts = Console.ReadLine()!.Split();
            if (parts[0] == "Update")
            {
                var left = int.Parse(parts[1]) - 1;
                var right = int.Parse(parts[2]) - 1;
                var newSongName = parts[3];

                if (!songIdByName.ContainsKey(newSongName))
                {
                    songIdByName[newSongName] = nextSongId;
                    songNamesById[nextSongId] = newSongName;
                    nextSongId++;
                }
                
                var newSongId = songIdByName[newSongName];
                AssignSongRange(1, 0, totalMinutes - 1, left, right, newSongId);
            }

            else
            {
                var left = int.Parse(parts[1]) - 1;
                var right = int.Parse(parts[2]) - 1;
                var resultSongId = QuerySongRange(1, 0, totalMinutes - 1, left, right);

                result.AppendLine(resultSongId == MixedSongId ? "Mixed" : songNamesById[resultSongId]);
            }
        }
        
        Console.Write(result.ToString());
    }

    private static void BuildTree(int nodeIndex, int segmentLeft, int segmentRight)
    {
        if (segmentLeft == segmentRight)
        {
            segmentTree[nodeIndex] = songsByMinutes[segmentLeft];
            return;
        }

        var middle = (segmentLeft + segmentRight) / 2;
        BuildTree(nodeIndex * 2, segmentLeft, middle);
        BuildTree(nodeIndex * 2 + 1, middle + 1, segmentRight);
        segmentTree[nodeIndex] = MergeValues(segmentTree[nodeIndex * 2], segmentTree[nodeIndex * 2 + 1]);
    }

    private static int MergeValues(int left, int right) => left == right ? left : MixedSongId;

    private static void AssignSongRange(
        int nodeIndex,
        int segmentLeft,
        int segmentRight,
        int queryLeft,
        int queryRight,
        int songId)
    {
        if (queryLeft > queryRight)
            return;

        if (queryLeft == segmentLeft && queryRight == segmentRight)
        {
            segmentTree[nodeIndex] = songId;
            lazyAssignedSongId[nodeIndex] = songId;
            
            return;
        }

        PushLazyUpdate(nodeIndex);
        
        var middle = (segmentLeft + segmentRight) / 2;
        AssignSongRange(nodeIndex * 2, segmentLeft, middle, queryLeft, Math.Min(queryRight, middle), songId);
        AssignSongRange(nodeIndex * 2 + 1, middle + 1, segmentRight, Math.Max(queryLeft, middle + 1), queryRight, songId);
        segmentTree[nodeIndex] = MergeValues(segmentTree[nodeIndex * 2], segmentTree[nodeIndex * 2 + 1]);
    }

    private static void PushLazyUpdate(int nodeIndex)
    {
        if (lazyAssignedSongId[nodeIndex] == NoLazyUpdate)
            return;
        
        var assignedSongId = lazyAssignedSongId[nodeIndex];
        segmentTree[nodeIndex * 2] = assignedSongId;
        segmentTree[nodeIndex * 2 + 1] = assignedSongId;
        lazyAssignedSongId[nodeIndex * 2] = assignedSongId;
        lazyAssignedSongId[nodeIndex * 2 + 1] = assignedSongId;
        lazyAssignedSongId[nodeIndex] = NoLazyUpdate;
    }

    private static int QuerySongRange(
        int nodeIndex,
        int segmentLeft,
        int segmentRight,
        int queryLeft,
        int queryRight)
    {
        if (queryLeft > queryRight)
            return MixedSongId;
        
        if (queryLeft == segmentLeft && queryRight == segmentRight)
            return segmentTree[nodeIndex];
        
        PushLazyUpdate(nodeIndex);
        
        var middle = (segmentLeft + segmentRight) / 2;
        var leftValue = QuerySongRange(nodeIndex * 2, segmentLeft, middle, queryLeft, Math.Min(queryRight, middle));
        var rightValue = QuerySongRange(nodeIndex * 2 + 1, middle + 1, segmentRight, Math.Max(queryLeft, middle + 1), queryRight);
        
        if (queryRight <= middle)
            return leftValue;
        
        return queryLeft > middle ? rightValue : MergeValues(leftValue, rightValue);
    }
}