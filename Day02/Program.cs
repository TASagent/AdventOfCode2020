using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day02
{
    class Program
    {
        private const string inputFile = @"../../../../input02.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 2 - Password Philosophy");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            IEnumerable<Password> passwords = File.ReadLines(inputFile).Select(x => new Password(x));

            int output1 = passwords.Count(x => x.IsValid());

            Console.WriteLine($"The answer is: {output1}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            int output2 = passwords.Count(x => x.IsValidDumber());

            Console.WriteLine($"The answer is: {output2}");


            Console.WriteLine();
            Console.ReadKey();
        }

        class Password
        {
            private int lowerBound;
            private int upperBound;
            private char character;
            private string password;

            private static char[] splitChars = new[] { ' ', ':', '-' };

            public Password(string input)
            {
                string[] splitString = input.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);

                lowerBound = int.Parse(splitString[0]);
                upperBound = int.Parse(splitString[1]);

                character = splitString[2][0];

                password = splitString[3];
            }

            public bool IsValid()
            {
                int charCount = password.Count(x => x == character);

                return charCount >= lowerBound && charCount <= upperBound;
            }

            public bool IsValidDumber()
            {
                bool firstMatch = password[lowerBound - 1] == character;
                bool secondMatch = password[upperBound - 1] == character;

                return firstMatch != secondMatch;
            }
        }
    }
}
