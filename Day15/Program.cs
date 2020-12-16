using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day15
{
    class Program
    {
        const string input = "0,13,1,16,6,17";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 15 - Rambunctious Recitation");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            Dictionary<long, long> numberLastRecitation = new Dictionary<long, long>();


            List<int> problemInput = input.Split(',').Select(int.Parse).ToList();
            long lastNumber = -1;
            long index = 0;

            foreach (int value in problemInput)
            {
                if (lastNumber != -1)
                {
                    numberLastRecitation.Add(lastNumber, index++);
                }
                lastNumber = value;
                Console.WriteLine($"{index}: {lastNumber}");
            }

            while (index < 2019)
            {
                if (numberLastRecitation.ContainsKey(lastNumber))
                {
                    //It's been said
                    long diff = index - numberLastRecitation[lastNumber];
                    numberLastRecitation[lastNumber] = index++;
                    lastNumber = diff;
                }
                else
                {
                    //It's new 
                    //Say Zero
                    numberLastRecitation[lastNumber] = index++;
                    lastNumber = 0;
                }

                if (index < 10 || index > 2010)
                {
                    Console.WriteLine($"{index}: {lastNumber}");
                }
            }

            long output1 = lastNumber;



            Console.WriteLine($"The answer is: {output1}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            long targetNum = 10000;


            while (index < 30_000_000L - 1)
            {
                if (index > targetNum)
                {
                    Console.WriteLine($"We reached {targetNum}");
                    targetNum *= 10;
                }

                if (numberLastRecitation.ContainsKey(lastNumber))
                {
                    //It's been said
                    long diff = index - numberLastRecitation[lastNumber];
                    numberLastRecitation[lastNumber] = index++;
                    lastNumber = diff;
                }
                else
                {
                    //It's new 
                    //Say Zero
                    numberLastRecitation[lastNumber] = index++;
                    lastNumber = 0;
                }
            }

            Console.WriteLine($"The answer is: {lastNumber}");


            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
