using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day13
{
    class Program
    {
        private const string inputFile = @"../../../../input13.txt";

        static int startTime;

        static void Main(string[] args)
        {
            Console.WriteLine("Day 13 - Shuttle Search");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            string[] lines = File.ReadAllLines(inputFile);

            startTime = int.Parse(lines[0]);

            List<int> busIDs = lines[1]
                .Split(',')
                .Where(value => value != "x")
                .Select(int.Parse)
                .ToList();

            var earliest = busIDs.Select(GetEarliestTime).OrderBy(x => x.Item2).First();

            int output1 = busIDs[earliest.Item1] * (earliest.Item2 - startTime);

            Console.WriteLine($"The answer is: {output1}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            List<(int, int)> smarterBusIDs = new List<(int, int)>();

            string[] splitInput = lines[1].Split(',');

            for (int i = 0; i < splitInput.Length; i++)
            {
                if (splitInput[i] != "x")
                {
                    smarterBusIDs.Add((int.Parse(splitInput[i]), i));
                }
            }

            long currentTime = 0;
            long currentFactor = 1;

            foreach ((int id, int offset) in smarterBusIDs)
            {
                while ((currentTime + offset) % id != 0)
                {
                    currentTime += currentFactor;
                }

                currentFactor *= id;
            }

            Console.WriteLine($"The answer is: {currentTime}");


            Console.WriteLine();
            Console.ReadKey();
        }

        static (int, int) GetEarliestTime(int value, int index)
        {
            int factor = (startTime + value - 1) / value;
            return (index, factor * value);
        }
    }
}
