using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class RomanNumber
    {
        public int Value { get; set; }
        private const char ZERO_DIGIT = 'N';
        private const char MINUS_SIGN = '-';
        private const String INVALID_DIGIT_MESSAGE = "Invalid Roman digit(s):";
        private const String EMPTY_MESSAGE = "NULL or Empty:";
        private const String ADD_NULL_MESSAGE = "Cannot add null object:";
        private const String NULL_MESSAGE_PATTERN = "{0}: '{1}'";
        private const String SUM_NULL_MESSAGE = "Invalid Sum() invocation with NULL argument";

        public RomanNumber(int value  = 0) 
        {
            Value = value;        
        }

        public override string ToString()
        {
            Dictionary<int, String> parts = new()
            {
                {1000, "M" },
                {900, "CM" },
                {500, "D" },
                {400, "CD" },
                {100, "C" },
                {90, "XC" },
                {50, "L" },
                {40, "XL" },
                {10, "X" },
                {9, "IX" },
                {5, "V" },
                {4, "IV" },
                {1, "I" }
            };

            if (Value == 0)
            {
                return ZERO_DIGIT.ToString();
            }

            var number = Value;
            bool isNegative = false;
            if( number < 0)
            {
                isNegative = true;
                number = Math.Abs(number);
            }
            StringBuilder sb = new();
            if (isNegative) sb.Append(MINUS_SIGN);

            foreach (var part in parts)
            {
                while (number >= part.Key)
                {
                    sb.Append(part.Value);
                    number -= part.Key;
                }
            }
            return sb.ToString();
        }

        public static RomanNumber Parse(String input)
        {

            input = input?.Trim()!;

            CheackValidityOrThrow(input);
            CheckLegalityOrThrow(input);

            if (input == ZERO_DIGIT.ToString()) return new();

            int prev = 0;
            int result = 0;
            int lastDigitIndex = input[0] == MINUS_SIGN ? 1 : 0;
            int current = 0;
    
            for (int i = input.Length - 1; i >= lastDigitIndex; i--)
            {
                current = DigitValue(input[i]);
                result += prev > current ? -current : current;
                prev = current;
            }
           
            return new()
            {
                Value = result * (1 - 2 * lastDigitIndex)
            };
        }
        
        private static int DigitValue(char digit)
        {
            return digit switch
            {
                ZERO_DIGIT => 0,
                'I' => 1,
                'V' => 5,
                'X' => 10,
                'L' => 50,
                'C' => 100,
                'D' => 500,
                'M' => 1000,
                _ => throw new ArgumentException($"{INVALID_DIGIT_MESSAGE} '{digit}'")
            };
        }
    
        private static void CheackValidityOrThrow(String input)
        {
            if (String.IsNullOrEmpty(input))
            {
                throw new ArgumentException(EMPTY_MESSAGE);
            }
            if(input.StartsWith(MINUS_SIGN))
            {
                input = input[1..];
            }

            

            List<char> invalidChars = new();
            foreach (char c in input)
            {
                try { DigitValue(c); }
                catch { invalidChars.Add(c); }
            }

            if (invalidChars.Count > 0)
            {
                String chars = String.Join(", ",
                    invalidChars
                    .Select(c => $"'{c}'"));
                throw new ArgumentException($"{INVALID_DIGIT_MESSAGE} {chars}");
            }
        }

        private static void CheckLegalityOrThrow(String input)
        {
            int maxDigit = 0;
            int lessDigitsCount = 0;
            int lastDigitIndex = input.StartsWith('-') ? 1 : 0;
            for (int i = input.Length - 1; i >= lastDigitIndex; i--)
            {

                int digitValue = DigitValue(input[i]);

                if (digitValue < maxDigit)
                {
                    lessDigitsCount += 1;
                    if (lessDigitsCount > 1)
                    {
                        throw new ArgumentException(input);
                    }
                }
                else
                {
                    maxDigit = digitValue;
                    lessDigitsCount = 0;
                }

            }
        }

        public static RomanNumber Sum(params RomanNumber[] arr_r)
        {
            if (arr_r is null)
            {
                throw new ArgumentNullException(
                    String.Format(
                        NULL_MESSAGE_PATTERN,
                        SUM_NULL_MESSAGE,
                        nameof(arr_r)));
            }
            int res = 0;
            foreach (var r in arr_r)
            {
                res += r.Value;
            }
            return new(res);
        }

        public RomanNumber Add(RomanNumber other)
        {
            if (other == null)
                throw new ArgumentException($"Cannot Add null object: {nameof(other)}");
            return new() { Value = Value + other.Value };
        }
    }
}
