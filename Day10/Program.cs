using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day10
{
    class Program
    {
        private const string inputFile = @"../../../../input10.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 10 - Adapter Array");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            List<int> adapters = File.ReadAllLines(inputFile).Select(int.Parse).ToList();

            adapters.Add(0);
            adapters.Add(adapters.Max() + 3);
            adapters.Sort();

            //Add one at lines.Max() + 3

            int oneDiffCount = 0;
            int threeDiffCount = 0;


            for (int i = 0; i < adapters.Count - 1; i++)
            {
                int diff = adapters[i + 1] - adapters[i];

                switch (diff)
                {
                    case 1:
                        oneDiffCount++;
                        break;

                    case 2:
                        //Do nothing
                        break;

                    case 3:
                        threeDiffCount++;
                        break;

                    default:
                        throw new Exception();
                }
            }

            int output1 = oneDiffCount * threeDiffCount;

            Console.WriteLine($"The answer is: {output1}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            Dictionary<int, long> cache = new Dictionary<int, long>();

            long output2 = GetArrangements(adapters, 0, cache);

            Console.WriteLine($"The answer is: {output2}");

            Console.WriteLine();
            Console.ReadKey();
        }

        static long GetArrangements(
            List<int> adapters,
            int currentAdapterIndex,
            Dictionary<int, long> cache)
        {
            if (cache.ContainsKey(currentAdapterIndex))
            {
                return cache[currentAdapterIndex];
            }

            if (currentAdapterIndex == adapters.Count - 1)
            {
                return 1;
            }

            long arrangements = 0;
            int currentJoltage = adapters[currentAdapterIndex];

            for (int i = 1; i <= 3; i++)
            {
                if (currentAdapterIndex + i < adapters.Count && adapters[currentAdapterIndex + i] - currentJoltage < 4)
                {
                    arrangements += GetArrangements(adapters, currentAdapterIndex + i, cache);
                }
                else
                {
                    break;
                }
            }

            cache.Add(currentAdapterIndex, arrangements);
            return arrangements;
        }
    }
}
