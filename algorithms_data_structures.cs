// namespace lecture_C_;
//
// public static class AlgorithmsDataStructures
// {
//     //1
//     static void Main()
//     {
//         var firstLine = Console.ReadLine()!.Split();
//         var n = int.Parse(firstLine[0]);
//         var a = int.Parse(firstLine[1]);
//         
//         var secondLine = Console.ReadLine()!.Split();
//         var t = new int[n];
//         
//         for (var i = 0; i < n; i++)
//             t[i] = int.Parse(secondLine[i]);
//         
//         var endTime = new int[n];
//         var lastFinish = 0;
//         
//         for (var i = 0; i < n; i++)
//         {
//             var start = Math.Max(t[i], lastFinish);
//             var finish = start + a;
//             
//             endTime[i] = finish;
//             lastFinish = finish;
//         }
//         
//         for (var i = 0; i < n; i++)
//             Console.WriteLine(endTime[i]);
//     }
// }

namespace lecture_C_;

public static class AlgorithmsDataStructures
{
    //2
    static void Main()
    {
        var s = Console.ReadLine();
        var n = s!.Length;

        var frequent = new long[26];

        for (var i = 0; i < n; i++)
        {
            var c = s[i];
            var index = c - 'a';

            frequent[index] += (long)(i + 1) * (n - i);
        }

        for (var i = 0; i < 26; i++)
        {
            if (frequent[i] > 0)
            {
                var letter = (char)('a' + i);
                
                Console.WriteLine($"{letter}: {frequent[i]}");
            }
        }
    }
}


