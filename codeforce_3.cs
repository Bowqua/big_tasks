using System.Text;

namespace lecture_C_;

/*
//1
public class Codeforce3
{
    private static void Main()
    {
        var ordinarySchedule = Console.ReadLine()!;
        var idealSchedule = Console.ReadLine()!;
        const char zero = '0';
        const char one = '1';
        var countZero = ordinarySchedule.Count(digit => digit == zero);
        var countOne = ordinarySchedule.Count(digit => digit == one);
        var countZeroNeeded = idealSchedule.Count(digit => digit == zero);
        var countOneNeeded = idealSchedule.Count(digit => digit == one);
        
        if (countZeroNeeded > countZero || countOneNeeded > countOne)
            Console.WriteLine(ordinarySchedule);

        else
        {
            var result = new StringBuilder();
            result.Append(idealSchedule);
            countZero -= countZeroNeeded;
            countOne -= countOneNeeded;
            
            var length = idealSchedule.Length;
            int maxCovering;
            var maxPrefix = new int[length];

            for (var i = 1; i < length; i++)
            {
                var j = maxPrefix[i - 1];
                while (j > 0 && idealSchedule[i] != idealSchedule[j])
                    j = maxPrefix[j - 1];

                if (idealSchedule[i] == idealSchedule[j])
                    j++;
                
                maxPrefix[i] = j;
            }
            
            maxCovering = maxPrefix[length - 1];

            var adding = idealSchedule[maxCovering..];
            var addingZero = adding.Count(digit => digit == zero);
            var addingOne = adding.Count(digit => digit == one);

            while (countZero >= addingZero && countOne >= addingOne)
            {
                result.Append(adding);
                countZero -= addingZero;
                countOne -= addingOne;
            }
            
            result.Append(new string(zero, countZero));
            result.Append(new string(one, countOne));
            
            Console.WriteLine(result.ToString());
        }
    }
}
*/

/*
//2
public class Codeforce3
{
    private static int[] PrefixFunction(string text)
    {
        var maxCovering = new int[text.Length];
        for (var i = 1; i < text.Length; i++)
        {
            var count = maxCovering[i - 1];
            while (count > 0 && text[i] != text[count])
                count = maxCovering[count - 1];
            
            if (text[i] == text[count])
                count++;
            
            maxCovering[i] = count;
        }
        
        return maxCovering;
    }
    
    private static void Main()
    {
        var text = Console.ReadLine()!;
        var maxCovering = PrefixFunction(text);
        var length = text.Length;
        var seenOrNot = new bool[length + 1];
        
        for (var i = 0; i < length - 1; i++)
            seenOrNot[maxCovering[i]] = true;
        
        var maxLength = maxCovering[length - 1];
        while (maxLength > 0 && !seenOrNot[maxLength])
            maxLength = maxCovering[maxLength - 1];
        
        Console.WriteLine(maxLength == 0 ? "Just a legend" : text[..maxLength]);
    }
}
*/

/*
//3
public class Codeforce3
{
    private static void Main()
    {
        _ = Console.ReadLine();
        var brokenText = Console.ReadLine()!;
        var amountWords = int.Parse(Console.ReadLine()!);
        const int maxNodes = 1_000_005;
        var next = new int[maxNodes * 26];
        var wordId = new int[maxNodes];
        Array.Fill(wordId, -1);
        var words = new List<string>(amountWords);
        var nodesCount = 1;

        for (var i = 0; i < amountWords; i++)
        {
            var word = Console.ReadLine()!;
           words.Add(word);

           var node = 1;
           for (var j = word.Length - 1; j >= 0; j--)
           {
               var letter = word[j];
               if (letter >= 'A' && letter <= 'Z')
                   letter = (char)(letter - 'A' + 'a');

               var index = letter - 'a';
               var edgeIndex = node * 26 + index;
               var to  = next[edgeIndex];

               if (to == 0)
               {
                   to = ++nodesCount;
                   next[edgeIndex] = to;
               }

               node = to;
           }
           
           if (wordId[node] == -1)
               wordId[node] = i;
        }
        
        var length = brokenText.Length;
        var canBuild = new bool[length + 1];
        canBuild[0] = true;
        var previous = new int[length + 1];
        var chosenWordIndex = new int[length + 1];
        Array.Fill(chosenWordIndex, -1);

        for (var i = 0; i < length; i++)
        {
            if (!canBuild[i])
                continue;
            
            var node = 1;
            for (var j = i; j < length; j++)
            {
               var index = brokenText[j] - 'a';
               var to = next[node * 26 + index];
               
               if (to == 0)
                   break;

               node = to;
               
               var worldId = wordId[node];
               if (worldId != -1 && !canBuild[j + 1])
               {
                   canBuild[j + 1] = true;
                   previous[j + 1] = i;
                   chosenWordIndex[j + 1] = worldId;
               }
            }
        }
        
        var result = new List<string>();
        var position = length;

        while (position > 0)
        {
            var wordIndex = chosenWordIndex[position];
            result.Add(words[wordIndex]);
            position = previous[position];
        }
        
        result.Reverse();
        Console.WriteLine(string.Join(" ", result));
    }
}
*/

/*
//4
public class Codeforce3
{
    private static void Main()
    {
        _ = Console.ReadLine();
        var firstText = Console.ReadLine()!;
        var secondText = Console.ReadLine()!;
        var automation = new SuffixAutomation(firstText.Length);
       
        automation.Build(firstText);
        
        var (maxLength, endPosition) = automation.FindLongestCommonSubstring(secondText);
        if (maxLength == 0)
        {
            Console.WriteLine("");
            return;
        }
        
        Console.WriteLine(secondText.Substring(endPosition - maxLength + 1, maxLength));
    }

    private class SuffixAutomation
    {
        private readonly int[] transitions;
        private readonly int[] suffixLink;
        private readonly int[] stateLength;
        private int statesCount;
        private int lastState;

        public SuffixAutomation(int textLength)
        {
            var maxStates = 2 * textLength + 5;
            transitions = new int[maxStates * 26];
            suffixLink = new int[maxStates];
            stateLength = new int[maxStates];
            statesCount = 1;
            lastState = 0;
            suffixLink[0] = -1;
            stateLength[0] = 0;
        }

        public void Build(string text)
        {
            foreach (var letter in text)
                AddLetter(letter - 'A');
        }

        private void AddLetter(int letterIndex)
        {
            var currentState = statesCount++;
            var previousState = lastState;
            stateLength[currentState] = stateLength[lastState] + 1;

            while (previousState != -1 && GetTransition(previousState, letterIndex) == 0)
            {
                SetTransition(previousState, letterIndex, currentState);
                previousState = suffixLink[previousState];
            }
            
            if (previousState == -1)
                suffixLink[currentState] = 0;

            else
            {
                var nextState = GetTransition(previousState, letterIndex);
                if (stateLength[previousState] + 1 == stateLength[nextState])
                    suffixLink[currentState] = nextState;

                else
                {
                    var cloneState = statesCount++;
                    stateLength[cloneState] = stateLength[previousState] + 1;
                    
                    Array.Copy(transitions, nextState * 26, transitions, cloneState * 26, 26);
                    
                    suffixLink[cloneState] = suffixLink[nextState];
                    while (previousState != -1 && GetTransition(previousState, letterIndex) == nextState)
                    {
                        SetTransition(previousState, letterIndex, cloneState);
                        previousState = suffixLink[previousState];
                    }
                    
                    suffixLink[nextState] = cloneState;
                    suffixLink[currentState] = cloneState;
                }
            }
            
            lastState = currentState;
        }

        public (int maxLength, int endPosition) FindLongestCommonSubstring(string text)
        {
            var currentState = 0;
            var currentLength = 0;
            var maxLength = 0;
            var endPosition = -1;

            for (var i = 0; i < text.Length; i++)
            {
                var letterIndex = text[i] - 'A';
                if (GetTransition(currentState, letterIndex) != 0)
                {
                    currentState = GetTransition(currentState, letterIndex);
                    currentLength++;
                }

                else
                {
                    while (currentState != -1 && GetTransition(currentState, letterIndex) == 0)
                        currentState = suffixLink[currentState];

                    if (currentState == -1)
                    {
                        currentState = 0;
                        currentLength = 0;
                    }

                    else
                    {
                        currentLength = stateLength[currentState] + 1;
                        currentState = GetTransition(currentState, letterIndex);
                    }
                }

                if (currentLength > maxLength)
                {
                    maxLength = currentLength;
                    endPosition = i;
                }
            }
            
            return (maxLength, endPosition);
        }

        private int GetTransition(int state, int letterIndex) => transitions[state * 26 + letterIndex];
        private void SetTransition(int state, int letterIndex, int toState) => transitions[state * 26 + letterIndex] = toState;
    }
}
*/