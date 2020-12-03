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
            Console.WriteLine("Day 1 - Report Repair");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            int[] lines = File.ReadLines(inputFile).Select(int.Parse).ToArray();

            int output1 = 0;
            int output2 = 0;

            if (args.Length > 0 && args[0] == "-old")
            {
                (output1, output2) = SimpleSolution(lines);
            }
            else
            {
                (output1, output2) = LinqSolution(lines);
            }

            Console.WriteLine($"The answer is: {output1}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            Console.WriteLine($"The answer is: {output2}");


            Console.WriteLine();
            Console.ReadKey();
        }

        /// <summary>
        /// The simple, initial solution I submitted onstream
        /// </summary>
        static (int,int) SimpleSolution(int[] lines)
        {
            //
            // First Star
            //
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

            //
            // Second star
            //
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

            return (output1, output2);
        }


        /// <summary>
        /// A more sophisticated, albeit less clear, Linq-based solution
        /// </summary>
        static (int, int) LinqSolution(int[] lines)
        {
            //
            // First Star
            //
            int output1 = lines
                .GetUnorderedSubsets(2) //Get every unique, 2-item subset 
                .First(x => x.Sum() == 2020) //Get just the first subset which sums to 2020
                .Aggregate(1, (x, y) => x * y); //Find the product of all elements

            //
            // Second star
            //
            int output2 = lines
                .GetUnorderedSubsets(3) //Get every unique, 3-item subset 
                .First(x => x.Sum() == 2020) //Get just the first subset which sums to 2020
                .Aggregate(1, (x, y) => x * y); //Find the product of all elements

            return (output1, output2);
        }
    }
}
