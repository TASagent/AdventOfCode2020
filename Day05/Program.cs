using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day05
{
    class Program
    {
        private const string inputFile = @"../../../../input05.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 5 - Binary Boarding");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            List<int> seatIDs = File.ReadAllLines(inputFile).Select(TranslateSeat).ToList();


            int output1 = seatIDs.Max();



            Console.WriteLine($"The answer is: {output1}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            seatIDs.Sort();

            int output2 = 0;
            for (int i = 1; i < seatIDs.Count - 2; i++)
            {
                if (seatIDs[i + 1] - seatIDs[i] == 2)
                {
                    output2 = seatIDs[i] + 1;
                }
            }

            Console.WriteLine($"The answer is: {output2}");


            Console.WriteLine();
            Console.ReadKey();
        }

        public static int TranslateSeat(string input)
        {
            int seatID = 0;

            foreach (char c in input)
            {
                seatID <<= 1;

                switch (c)
                {
                    case 'B':
                    case 'R':
                        seatID |= 1;
                        break;

                    default:
                        break;
                }
            }

            return seatID;
        }
    }
}
