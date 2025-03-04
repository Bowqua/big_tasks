/ LD7. Найдите все палиндромы в строке. За O(n log n).
namespace lecture_C_;

public abstract class Palindrome
{
   private static long[] prefixHash;
   private static long[] basePowers;
   private static long[] reversedPrefixHash;
   private static long[] reversedBasePowers; // все они для формулы подсчета хэша

   private static void CalculatePrefixHash(string text) // строим формулу хэша для исходной подстроки
   {
      prefixHash = new long[text.Length + 1];
      basePowers = new long[text.Length + 1];
      prefixHash[0] = 0;
      basePowers[0] = 1;

      for (var i = 0; i < text.Length; i++)
      {
         prefixHash[i + 1] = (prefixHash[i] * 31 + (text[i] - 'a' + 1)) % 1000000007;
         basePowers[i + 1] = basePowers[i] * 31 % 1000000007;
      }
   }

   private static void CalculateReversePrefixHash(string text) // строим формулу хэша для reversed (обратной) подстроки 
   {
      reversedPrefixHash = new long[text.Length + 1];
      reversedBasePowers = new long[text.Length + 1];
      reversedPrefixHash[0] = 0;
      reversedBasePowers[0] = 1;

      for (var i = text.Length - 1; i >= 0; i--)
      {
         var j = text.Length - 1 - i;
         reversedPrefixHash[j + 1] = (reversedPrefixHash[j] * 31 + (text[i] - 'a' + 1)) % 1000000007;
         reversedBasePowers[j + 1] = reversedBasePowers[j] * 31 % 1000000007;
      }
   }

   private static long GetHashForSubstring(int left, int right) // вычисляем хэш для исходной подстроки
   {
      var result = (prefixHash[right + 1] - prefixHash[left] * basePowers[right - left + 1]) % 1000000007;
      if (result < 0) result += 1000000007;
      return result;
   }

   private static long GetHashForReversedSubstring(int left, int right, int n) // вычисляем хэш для обратной подстроки
   {
      var reversedStart = n - 1 - right; // позиция обратной подстроки
      var reversedEnd = n - 1 - left;
      var result = (reversedPrefixHash[reversedEnd + 1] - reversedPrefixHash[reversedStart] 
                        * reversedBasePowers[reversedEnd - reversedStart + 1]) % 1000000007;
      if (result < 0) result += 1000000007;
      return result;
   }

   private static bool IsPalindrome(string text, int left, int right) => 
      GetHashForSubstring(left, right) == GetHashForReversedSubstring(left, right, text.Length);

   private static int FindMaxRadiusOdd(string text, int center) // ищем радиус для нечетного палиндрома с центром center с помощью бинарного поиска
   {
      var minRadius = 1;
      var maxPossibleRadius = Math.Min(center, text.Length - center - 1);
      var maxValidRadius = 0;

      while (minRadius <= maxPossibleRadius)
      {
         var middleRadius = (minRadius + maxPossibleRadius) / 2;
         if (IsPalindrome(text, center - middleRadius, center + middleRadius))
         {
            maxValidRadius = middleRadius;
            minRadius = middleRadius + 1;
         }
         
         else maxPossibleRadius = middleRadius - 1;
      }
      
      return maxValidRadius;
   }

   private static int FindMaxRadiusEven(string text, int left) // ищем радиус для четного палиндрома с центром между left и right с помощью бинарного поиска
   {
      var right = left + 1;
      if (text[left] != text[right]) return -1;
      
      var minRadius = 0;
      var maxPossibleRadius = Math.Min(left, text.Length - right - 1);
      var maxValidRadius = 0;
      
      while (minRadius <= maxPossibleRadius)
      {
         var middleRadius = (minRadius + maxPossibleRadius) / 2;
         if (IsPalindrome(text, left - middleRadius, right + middleRadius))
         {
            maxValidRadius = middleRadius;
            minRadius = middleRadius + 1;
         }
         
         else maxPossibleRadius = middleRadius - 1;
      }
      
      return maxValidRadius;
   }
   
   private static string GetText() => Console.ReadLine()!;

   public static void Main()
   {
      var textFromConsole = GetText();
      var n = textFromConsole.Length;
      if (n == 0) return;
      
      CalculatePrefixHash(textFromConsole);
      CalculateReversePrefixHash(textFromConsole);
      
      var foundPalindromes = new HashSet<string>();

      for (var center = 0; center < n; center++) // обрабатываем случай для нечетных палиндромов, чей центр - один символ
      {
         var maxRadius = FindMaxRadiusOdd(textFromConsole, center);

         for (var radius = 1; radius <= maxRadius; radius++)
         {
            var length = radius * 2 + 1;
            var alternative = textFromConsole.Substring(center - radius, length);
            if (alternative.Length >= 2) foundPalindromes.Add(alternative);
         }
      }

      for (var left = 0; left < n - 1; left++) // обрабатываем случай для четных палиндромов, чей центр между двумя символами
      {
         var maxRadius = FindMaxRadiusEven(textFromConsole, left);
         if (maxRadius < 0) continue;
         
         for (var radius = 0; radius <= maxRadius; radius++)
         {
            var start = left - radius; 
            var length = radius * 2 + 2;
            if (start < 0 || start + length > n) continue;
            
            var alternative = textFromConsole.Substring(start, length);
            if (alternative.Length >= 2) foundPalindromes.Add(alternative);
         }
      }
      
      Console.WriteLine("Found Palindromes:");
      foreach (var foundPalindrome in foundPalindromes) Console.WriteLine(foundPalindrome);
   }
}
