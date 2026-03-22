namespace lecture_C_;

/*
//1
public class Codeforce6
{
    private class FastBinaryReader(Stream stream)
    {
        private readonly byte[] buffer = new byte[1 << 16];
        private int bufferSize;
        private int bufferPosition;

        public int ReadInt()
        {
            var byte0 = ReadByte();
            var byte1 = ReadByte();
            var byte2 = ReadByte();
            var byte3 = ReadByte();
            
            return byte0 | (byte1 << 8) | (byte2 << 16) | (byte3 << 24);
        }

        private byte ReadByte()
        {
            if (bufferPosition >= bufferSize)
            {
                bufferSize = stream.Read(buffer, 0, buffer.Length);
                bufferPosition = 0;
                
                if (bufferSize == 0)
                    throw new EndOfStreamException();
            }
            
            return buffer[bufferPosition++];
        }
    }
    
    private static void Main()
    {
        using var data = Console.OpenStandardInput();
        var reader = new FastBinaryReader(data);
        var numbersCount = reader.ReadInt();
        var sortedNumbers = new int[numbersCount];
        
        for (var i = 0; i < numbersCount; i++)
            sortedNumbers[i] = reader.ReadInt();
        
        var queriesCount = reader.ReadInt();
        var queries = new int[queriesCount];
        
        for (var i = 0; i < queriesCount; i++)
            queries[i] = reader.ReadInt();
        
        Array.Sort(queries);
        
        long result = 0;
        var numbersPointer = 0;

        for (var i = 0; i < queriesCount; i++)
        {
            var queryValue = queries[i];
            while (numbersPointer < numbersCount && sortedNumbers[numbersPointer] < queryValue)
                numbersPointer++;
            
            result += numbersPointer;
        }
        
        Console.WriteLine(result);
    }
}
*/

/*
//2
public class Codeforce6
{
    private class FastBinaryReader()
    {
        private readonly Stream stream;
        private readonly byte[] buffer;
        private int bufferLength;
        private int bufferPosition;

        public FastBinaryReader(Stream stream) : this()
        {
            this.stream = stream;
            buffer = new byte[1 << 16];
        }

        public int ReadInt()
        {
            var byte0 = ReadByte();
            var byte1 = ReadByte();
            var byte2 = ReadByte();
            var byte3 = ReadByte();
            
            return byte0 | (byte1 << 8) | (byte2 << 16) | (byte3 << 24);
        }

        public byte ReadByte()
        {
            if (bufferPosition >= bufferLength)
            {
                bufferLength = stream.Read(buffer, 0, buffer.Length);
                bufferPosition = 0;
                
                if (bufferLength == 0)
                    throw new EndOfStreamException();
            }
            
            return buffer[bufferPosition++];
        }
    }

    private class BTree
    {
        private class Node(bool isLeaf)
        {
            public readonly int[] Keys = new int[MaxKeys];
            public readonly Node[] Children = new Node[MaxChildren];
            public int KeyCount = 0;
            public bool IsLeaf = isLeaf;
        }
        
        private const int Degree = 32;
        private const int MaxKeys = 2 * Degree - 1;
        private const int MaxChildren = 2 * Degree;
        private Node root = new(true);

        public void Insert(int key)
        {
            if (root.KeyCount == MaxKeys)
            {
                var newRoot = new Node(false)
                {
                    Children =
                    {
                        [0] = root
                    }
                };
                SplitChild(newRoot, 0);
                root = newRoot;
            }

            InsertNonFull(root, key);
        }

        private static void SplitChild(Node parent, int childIndex)
        {
            var fullChild = parent.Children[childIndex];
            var rightNode = new Node(fullChild.IsLeaf)
            {
                KeyCount = Degree - 1
            };

            for (var i = 0; i < Degree - 1; i++)
                rightNode.Keys[i] = fullChild.Keys[i + Degree];
            
            if (!fullChild.IsLeaf) 
                for (var i = 0; i < Degree; i++)
                    rightNode.Children[i] = fullChild.Children[i + Degree];
            
            var middleKey = fullChild.Keys[Degree - 1];
            fullChild.KeyCount = Degree - 1;
            
            for (var i = parent.KeyCount; i >= childIndex + 1; i--)
                parent.Children[i + 1] = parent.Children[i];
            
            parent.Children[childIndex + 1] = rightNode;
            
            for (var i = parent.KeyCount - 1; i >= childIndex; i--)
                parent.Keys[i + 1] = parent.Keys[i];
            
            parent.Keys[childIndex] = middleKey;
            parent.KeyCount++;
        }

        private static void InsertNonFull(Node node, int key)
        {
            while (true)
            {
                var position = node.KeyCount - 1;
                if (node.IsLeaf)
                {
                    while (position >= 0 && key < node.Keys[position])
                    {
                        node.Keys[position + 1] = node.Keys[position];
                        position--;
                    }

                    node.Keys[position + 1] = key;
                    node.KeyCount++;

                    return;
                }

                while (position >= 0 && key < node.Keys[position]) position--;

                var childIndex = position + 1;
                if (node.Children[childIndex].KeyCount == MaxKeys)
                {
                    SplitChild(node, childIndex);
                    if (key > node.Keys[childIndex]) childIndex++;
                }

                node = node.Children[childIndex];
            }
        }

        public int LowerBound(int value)
        {
            var current = root;
            var candidate = 0;
            var hasCandidate = false;

            while (current != null)
            {
                var index = FindFirstGreaterEqualIndex(current.Keys, current.KeyCount, value);
                if (index < current.KeyCount)
                {
                    candidate = current.Keys[index];
                    hasCandidate = true;
                }
                
                if (current.IsLeaf)
                    break;
                
                current = current.Children[index];
            }
            
            return hasCandidate ? candidate : 0;
        }

        private static int FindFirstGreaterEqualIndex(int[] keys, int keyCount, int value)
        {
            var left = 0;
            var right = keyCount;

            while (left < right)
            {
                var middle = left + (right - left) / 2;
                if (keys[middle] < value)
                    left = middle + 1;
                else 
                    right = middle;
            }
            
            return left;
        }
    }

    private static void Main()
    {
        using var data = Console.OpenStandardInput();
        var reader = new FastBinaryReader(data);
        var queriesCount = reader.ReadInt();
        var tree = new BTree();
        long result = 0;

        for (var i = 0; i < queriesCount; i++)
        {
            var queryType = reader.ReadByte();
            var value = reader.ReadInt();
            
            if (queryType == (byte)'+')
                tree.Insert(value);
            
            else 
                result += tree.LowerBound(value);
        }
        
        Console.WriteLine(result);
    }
}
*/

/*
//3
public class Codeforce6
{
    private class FastBinaryReader(Stream stream)
    {
        private readonly byte[] buffer = new byte[1 << 20];
        private int bufferLength;
        private int bufferPosition;

        public int ReadInt()
        {
            int byte0 = ReadByte();
            int byte1 = ReadByte();
            int byte2 = ReadByte();
            int byte3 = ReadByte();

            return byte0 | (byte1 << 8) | (byte2 << 16) | (byte3 << 24);
        }

        public byte ReadByte()
        {
            if (bufferPosition >= bufferLength)
            {
                bufferLength = stream.Read(buffer, 0, buffer.Length);
                bufferPosition = 0;

                if (bufferLength == 0)
                    throw new EndOfStreamException();
            }

            return buffer[bufferPosition++];
        }
    }

    private class FenwickTree(int size)
    {
        private readonly int[] tree = new int[size + 1];

        public void Add(int index, int delta)
        {
            index++;
            while (index < tree.Length)
            {
                tree[index] += delta;
                index += index & -index;
            }
        }

        public int GetPrefixSumExclusive(int right)
        {
            var sum = 0;
            while (right > 0)
            {
                sum += tree[right];
                right -= right & -right;
            }

            return sum;
        }
    }

    private static void Main()
    {
        using var data = Console.OpenStandardInput();
        var reader = new FastBinaryReader(data);
        var queriesCount = reader.ReadInt();
        var queryTypes = new byte[queriesCount];
        var queryValues = new int[queriesCount];
        var insertedValues = new int[queriesCount];
        var insertedCount = 0;

        for (var i = 0; i < queriesCount; i++)
        {
            var queryType = reader.ReadByte();
            var value = reader.ReadInt();

            queryTypes[i] = queryType;
            queryValues[i] = value;

            if (queryType == (byte)'+')
                insertedValues[insertedCount++] = value;
        }

        var coordinates = new int[insertedCount];
        Array.Copy(insertedValues, coordinates, insertedCount);

        var insertPositions = new int[insertedCount];
        for (var i = 0; i < insertedCount; i++)
            insertPositions[i] = i;

        Array.Sort(coordinates, insertPositions);

        var compressedIndexByInsertOrder = new int[insertedCount];
        for (var sortedIndex = 0; sortedIndex < insertedCount; sortedIndex++)
            compressedIndexByInsertOrder[insertPositions[sortedIndex]] = sortedIndex;

        var fenwick = new FenwickTree(insertedCount);
        var insertSeen = 0;
        long result = 0;

        for (var i = 0; i < queriesCount; i++)
        {
            var queryType = queryTypes[i];
            var value = queryValues[i];

            if (queryType == (byte)'+')
            {
                fenwick.Add(compressedIndexByInsertOrder[insertSeen], 1);
                insertSeen++;
            }
            
            else
            {
                var firstGreaterOrEqualIndex = LowerBound(coordinates, value);
                result += fenwick.GetPrefixSumExclusive(firstGreaterOrEqualIndex);
            }
        }

        Console.WriteLine(result);
    }

    private static int LowerBound(int[] array, int value)
    {
        var left = 0;
        var right = array.Length;

        while (left < right)
        {
            var middle = left + ((right - left) >> 1);
            if (array[middle] < value)
                left = middle + 1;
            
            else
                right = middle;
        }

        return left;
    }
}
*/

/*
//4
public class Codeforce6
{
    private class FastBinaryReader(Stream stream)
    {
        private readonly byte[] buffer = new byte[1 << 20];
        private int bufferLength;
        private int bufferPosition;

        public int ReadInt()
        {
            int byte0 = ReadByte();
            int byte1 = ReadByte();
            int byte2 = ReadByte();
            int byte3 = ReadByte();

            return byte0 | (byte1 << 8) | (byte2 << 16) | (byte3 << 24);
        }

        public byte ReadByte()
        {
            if (bufferPosition >= bufferLength)
            {
                bufferLength = stream.Read(buffer, 0, buffer.Length);
                bufferPosition = 0;

                if (bufferLength == 0)
                    throw new EndOfStreamException();
            }

            return buffer[bufferPosition++];
        }
    }

    private class SegmentTree(int size)
    {
        private readonly int[] tree = new int[size * 2];

        public void Set(int index, int value)
        {
            index += size;
            tree[index] = value;
            index >>= 1;

            while (index > 0)
            {
                var leftValue = tree[index << 1];
                var rightValue = tree[(index << 1) | 1];
                
                tree[index] = leftValue > rightValue ? leftValue : rightValue;
                index >>= 1;
            }
        }

        public int GetMax(int left, int right)
        {
            var result = 0;
            left += size;
            right += size;

            while (left < right)
            {
                if ((left & 1) == 1)
                {
                    var value = tree[left];
                    if (value > result)
                        result = value;
                    
                    left++;
                }

                if ((right & 1) == 1)
                {
                    right--;
                    var value = tree[right];
                    
                    if (value > result)
                        result = value;
                }

                left >>= 1;
                right >>= 1;
            }
            
            return result;
        }
    }

    private static int LowerBound(int[] array, int value)
    {
        var left = 0;
        var right = array.Length;

        while (left < right)
        {
            var middle = left + ((right - left) >> 1);
            if (array[middle] < value)
                left = middle + 1;
            
            else 
                right = middle;
        }

        return left;
    }

    private static int UpperBound(int[] array, int value)
    {
        var left = 0;
        var right = array.Length;

        while (left < right)
        {
            var middle = left + ((right - left) >> 1);
            if (array[middle] <= value)
                left = middle + 1;
            
            else 
                right = middle;
        }
        
        return left;
    }

    private static void Main()
    {
        using var data = Console.OpenStandardInput();
        var reader = new FastBinaryReader(data);
        var queriesCount = reader.ReadInt();
        var queryTypes = new byte[queriesCount];
        var firstValues = new int[queriesCount];
        var secondValues = new int[queriesCount];
        var insertedCount = 0;

        for (var i = 0; i < queriesCount; i++)
        {
            var queryType = reader.ReadByte();
            var first = reader.ReadInt();
            var second = reader.ReadInt();
            queryTypes[i] = queryType;
            firstValues[i] = first;
            secondValues[i] = second;

            if (queryType == (byte)'+')
                insertedCount++;
        }

        var coordinates = new int[insertedCount];
        var insertOrder = new int[insertedCount];
        var insertIndex = 0;
        
        for (var i = 0; i < queriesCount; i++)
        {
            if (queryTypes[i] == (byte)'+')
            {
                coordinates[insertIndex] = firstValues[i];
                insertOrder[insertIndex] = insertIndex;
                insertIndex++;
            }
        }

        Array.Sort(coordinates, insertOrder);

        var compressedIndexInsert = new int[insertedCount];
        for (var sortedIndex = 0; sortedIndex < insertedCount; sortedIndex++)
            compressedIndexInsert[insertOrder[sortedIndex]] = sortedIndex;

        var segmentTree = new SegmentTree(insertedCount);
        var insertsSeen = 0;
        long result = 0;

        for (var i = 0; i < queriesCount; i++)
        {
            var queryType = queryTypes[i];
            var first = firstValues[i];
            var second = secondValues[i];

            if (queryType == (byte)'+')
            {
                var compressedIndex = compressedIndexInsert[insertsSeen];
                segmentTree.Set(compressedIndex, second);
                
                insertsSeen++;
            }
            
            else
            {
                var leftIndex = LowerBound(coordinates, first);
                var rightExclusive = UpperBound(coordinates, second);
                
                result += segmentTree.GetMax(leftIndex, rightExclusive);
            }
        }

        Console.WriteLine(result);
    }
}
*/

//5
public class Codeforce6
{
    private class FastBinaryReader(Stream stream)
    {
        private readonly byte[] buffer = new byte[1 << 20];
        private int bufferLength;
        private int bufferPosition;

        public int ReadInt()
        {
            int byte0 = ReadByte();
            int byte1 = ReadByte();
            int byte2 = ReadByte();
            int byte3 = ReadByte();

            return byte0 | (byte1 << 8) | (byte2 << 16) | (byte3 << 24);
        }

        public byte ReadByte()
        {
            if (bufferPosition >= bufferLength)
            {
                bufferLength = stream.Read(buffer, 0, buffer.Length);
                bufferPosition = 0;

                if (bufferLength == 0)
                    throw new EndOfStreamException();
            }

            return buffer[bufferPosition++];
        }
    }

    private class FenwickTree(int size)
    {
        private readonly int[] tree = new int[size + 1];

        public void Add(int index, int delta)
        {
            index++;
            while (index < tree.Length)
            {
                tree[index] += delta;
                index += index & -index;
            }
        }

        public int GetPrefixSum(int right)
        {
            var sum = 0;
            while (right > 0)
            {
                sum += tree[right];
                right -= right & -right;
            }
            
            return sum;
        }

        public int FindByOrder(int order)
        {
            var index = 0;
            var bit = HighestPower(tree.Length);

            while (bit > 0)
            {
                var nextIndex = index + bit;
                if (nextIndex < tree.Length && tree[nextIndex] < order)
                {
                    order -= tree[nextIndex];
                    index = nextIndex;
                }

                bit >>= 1;
            }
            
            return index;
        }

        private static int HighestPower(int value)
        {
            var power = 1;
            while (power << 1 < value)
                power <<= 1;
            
            return power;
        }
    }
    
    private static int LowerBound(int[] array, int value)
    {
        var left = 0;
        var right = array.Length;

        while (left < right)
        {
            var middle = left + ((right - left) >> 1);
            if (array[middle] < value)
                left = middle + 1;
            
            else 
                right = middle;
        }

        return left;
    }

    private static void Main()
    {
        using var data = Console.OpenStandardInput();
        var reader = new FastBinaryReader(data);
        var queriesCount = reader.ReadInt();
        var queryTypes = new byte[queriesCount];
        var queryValues = new int[queriesCount];
        var insertedCount = 0;

        for (var i = 0; i < queriesCount; i++)
        {
            var type = reader.ReadByte();
            var value = reader.ReadInt();
            queryTypes[i] = type;
            queryValues[i] = value;
            
            if (type == (byte)'+')
                insertedCount++;
        }
        
        var coordinates = new int[insertedCount];
        var fillIndex = 0;

        for (var i = 0; i < queriesCount; i++)
        {
            if (queryTypes[i] == (byte)'+')
                coordinates[fillIndex++] = queryValues[i];
        }
        
        Array.Sort(coordinates);
        
        var fenwickTree = new FenwickTree(insertedCount);
        var isActive = new bool[insertedCount];
        long result = 0;

        for (var i = 0; i < queriesCount; i++)
        {
            var type = queryTypes[i];
            var value = queryValues[i];

            if (type == (byte)'+')
            {
                var index = LowerBound(coordinates, value);
                if (!isActive[index])
                {
                    isActive[index] = true;
                    fenwickTree.Add(index, 1);
                }
            }
            
            else if (type == (byte)'-')
            {
                var index = LowerBound(coordinates, value);
                if (index < insertedCount && coordinates[index] == value && isActive[index])
                {
                    isActive[index] = false;
                    fenwickTree.Add(index, -1);
                }
            }

            else
            {
                var startIndex = LowerBound(coordinates, value);
                var activeBefore = fenwickTree.GetPrefixSum(startIndex);
                var targetOrder = activeBefore + 1;
                var answerIndex = fenwickTree.FindByOrder(targetOrder);
                
                result += coordinates[answerIndex];
            }
        }
        
        Console.WriteLine(result);
    }
}