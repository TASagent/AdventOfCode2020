using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day25
{
    class Program
    {
        private const long InputNumber1 = 3418282;
        private const long InputNumber2 = 8719412;

        static void Main(string[] args)
        {
            Console.WriteLine("Day 25 - Combo Breaker");
            Console.WriteLine();

            long doorSecretNumber = 0;
            long cardSecretNumber = 0;

            long subjectNumber = 7;
            long currentNumber = 1;

            while (currentNumber != InputNumber1)
            {
                currentNumber *= subjectNumber;
                currentNumber %= 20201227;
                doorSecretNumber++;
            }

            currentNumber = 1;

            while (currentNumber != InputNumber2)
            {
                currentNumber *= subjectNumber;
                currentNumber %= 20201227;
                cardSecretNumber++;
            }

            subjectNumber = InputNumber1;
            currentNumber = 1;

            for (int loop = 0; loop < cardSecretNumber; loop++)
            {
                currentNumber *= subjectNumber;
                currentNumber %= 20201227;
            }

            long output = currentNumber;

            Console.WriteLine($"The answer is: {output}");
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
