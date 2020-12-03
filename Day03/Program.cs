using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day03
{
    class Program
    {
        private const string inputFile = @"../../../../input03.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 3 - Toboggan Trajectory");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            string[] lines = File.ReadAllLines(inputFile);

            {
                int x = 0;
                int y = 0;

                int treeCount = 0;

                while (y < lines.Length)
                {
                    if (CheckForTree(lines, x, y))
                    {
                        treeCount++;
                    }

                    y++;
                    x += 3;
                }

                Console.WriteLine($"The answer is: {treeCount}");
            }

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            long cumulativeTreeCount = 1;

            (int, int)[] offsets = new[]
            {
                (1, 1),
                (3, 1),
                (5, 1),
                (7, 1),
                (1, 2)
            };

            foreach ((int xOffset, int yOffset) in offsets)
            {
                int x = 0;
                int y = 0;

                long treeCount = 0;

                while (y < lines.Length)
                {
                    if (CheckForTree(lines, x, y))
                    {
                        treeCount++;
                    }

                    x += xOffset;
                    y += yOffset;
                }

                cumulativeTreeCount *= treeCount;
            }

            Console.WriteLine($"The answer is: {cumulativeTreeCount}");




            Console.WriteLine();
            Console.ReadKey();
        }


        private static bool CheckForTree(string[] lines, int x, int y)
        {
            x %= lines[0].Length;

            return lines[y][x] == '#';
        }
    }
}
