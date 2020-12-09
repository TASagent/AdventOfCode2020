using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day09
{
    class Program
    {
        private const string inputFile = @"../../../../input09.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 9 - Encoding Error");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            long[] numbers = File.ReadAllLines(inputFile).Select(long.Parse).ToArray();
            HashSet<long> searchingDigit = new HashSet<long>(25);

            long firstBadNumber = -1;

            for (int i = 25; i < numbers.Length; i++)
            {
                long targetNumber = numbers[i];
                bool found = false;

                for (int j = 1; j < 26; j++)
                {
                    if (searchingDigit.Contains(numbers[i - j]))
                    {
                        found = true;
                        break;
                    }

                    searchingDigit.Add(targetNumber - numbers[i - j]);
                }

                if (!found)
                {
                    firstBadNumber = targetNumber;
                    break;
                }

                searchingDigit.Clear();
            }

            Console.WriteLine($"The answer is: {firstBadNumber}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            int lowerIndex = 0;
            int upperIndex = 0;
            long cumulativeSum = 0;

            while (cumulativeSum != firstBadNumber)
            {
                while (cumulativeSum < firstBadNumber)
                {
                    cumulativeSum += numbers[upperIndex];
                    upperIndex++;
                }

                while (cumulativeSum > firstBadNumber)
                {
                    cumulativeSum -= numbers[lowerIndex];
                    lowerIndex++;
                }
            }

            long min = numbers[lowerIndex..upperIndex].Min();
            long max = numbers[lowerIndex..upperIndex].Max();


            Console.WriteLine($"The answer is: {min + max}");


            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
