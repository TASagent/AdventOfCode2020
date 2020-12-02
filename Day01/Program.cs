using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AoCTools;

namespace Day01
{
    class Program
    {
        private const string inputFile = @"../../../../input01.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 1");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            int[] lines = File.ReadAllLines(inputFile).Select(int.Parse).ToArray();

            int output1 = 0;

            for (int i = 0; i < lines.Length - 1; i++)
            {
                for (int j = i + 1; j < lines.Length; j++)
                {
                    if (lines[i] + lines[j] == 2020)
                    {
                        output1 = lines[i] * lines[j];
                        break;
                    }
                }

                if (output1 != 0)
                {
                    break;
                }
            }

            Console.WriteLine($"The answer is: {output1}");

            int linqOutput1 = lines.GetUnorderedSubsets(2).First(x => x.Sum() == 2020).Aggregate(1, (x, y) => x * y);

            Console.WriteLine($"The Linq answer is: {linqOutput1}");



            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();


            int output2 = 0;

            for (int i = 0; i < lines.Length - 2; i++)
            {
                for (int j = i + 1; j < lines.Length - 1; j++)
                {

                    for (int k = j + 1; k < lines.Length; k++)
                    {
                        if (lines[i] + lines[j] + lines[k] == 2020)
                        {
                            output2 = lines[i] * lines[j] * lines[k];
                            break;
                        }
                    }

                    if (output2 != 0)
                    {
                        break;
                    }
                }

                if (output2 != 0)
                {
                    break;
                }
            }



            Console.WriteLine($"The answer is: {output2}");

            int linqOutput2 = lines.GetUnorderedSubsets(3).First(x => x.Sum() == 2020).Aggregate(1, (x, y) => x * y);

            Console.WriteLine($"The Linq answer is: {linqOutput2}");


            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
