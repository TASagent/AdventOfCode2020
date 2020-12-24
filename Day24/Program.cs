using AoCTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day24
{
    class Program
    {
        private const string inputFile = @"../../../../input24.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 24 - Lobby Layout");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            string[] lines = File.ReadAllLines(inputFile);


            List<HexDir[]> directions = lines.Select(x => ParseLine(x).ToArray()).ToList();

            HashSet<Point3D> flippedHexes = new HashSet<Point3D>();


            foreach (HexDir[] directionSet in directions)
            {
                Point3D position = new Point3D();

                foreach (HexDir dir in directionSet)
                {
                    position = Move(position, dir);
                }

                if (flippedHexes.Contains(position))
                {
                    flippedHexes.Remove(position);
                }
                else
                {
                    flippedHexes.Add(position);
                }
            }

            int output1 = flippedHexes.Count();

            Console.WriteLine($"The answer is: {output1}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            HashSet<Point3D> nextFlippedHexes = new HashSet<Point3D>();

            for (int i = 0; i < 100; i++)
            {
                int radius = flippedHexes.Select(MaybeDistance).Max() + 1;

                for (int x = -radius; x <= radius; x++)
                {
                    for (int y = -radius; y <= radius; y++)
                    {
                        int z = -x - y;

                        Point3D point = (x, y, z);

                        int neighbors = CountNeighbors(point, flippedHexes);

                        if (flippedHexes.Contains((x, y, z)))
                        {
                            //Currently Black
                            if (neighbors == 1 || neighbors == 2)
                            {
                                nextFlippedHexes.Add(point);
                            }
                        }
                        else
                        {
                            //Currently White
                            if (neighbors == 2)
                            {
                                nextFlippedHexes.Add(point);
                            }
                        }
                    }
                }

                (nextFlippedHexes, flippedHexes) = (flippedHexes, nextFlippedHexes);
                nextFlippedHexes.Clear();
            }

            int output2 = flippedHexes.Count();

            Console.WriteLine($"The answer is: {output2}");


            Console.WriteLine();
            Console.ReadKey();
        }

        public static Point3D Move(in Point3D initial, HexDir direction)
        {
            switch (direction)
            {
                case HexDir.East: return new Point3D(initial.x + 1, initial.y - 1, initial.z);
                case HexDir.NorthEast: return new Point3D(initial.x, initial.y - 1, initial.z + 1);
                case HexDir.NorthWest: return new Point3D(initial.x - 1, initial.y, initial.z + 1);
                case HexDir.West: return new Point3D(initial.x - 1, initial.y + 1, initial.z);
                case HexDir.SouthWest: return new Point3D(initial.x, initial.y + 1, initial.z - 1);
                case HexDir.SouthEast: return new Point3D(initial.x + 1, initial.y, initial.z - 1);

                default:
                    throw new Exception();
            }
        }

        public static int MaybeDistance(Point3D point) =>
            Math.Max(Math.Abs(point.x), Math.Max(Math.Abs(point.y), Math.Abs(point.z)));

        public static int CountNeighbors(in Point3D point, HashSet<Point3D> grid)
        {
            int count = 0;

            for (HexDir dir = 0; dir < HexDir.MAX; dir++)
            {
                if (grid.Contains(Move(point, dir)))
                {
                    count++;
                }
            }

            return count;
        }


        public static IEnumerable<HexDir> ParseLine(string line)
        {
            char pendingDir = '\0';


            foreach (char c in line)
            {
                switch (c)
                {
                    case 'n':
                    case 's':
                        pendingDir = c;
                        break;

                    case 'e':
                        if (pendingDir == 'n')
                        {
                            pendingDir = '\0';
                            yield return HexDir.NorthEast;
                        }
                        else if (pendingDir == 's')
                        {
                            pendingDir = '\0';
                            yield return HexDir.SouthEast;
                        }
                        else
                        {
                            yield return HexDir.East;
                        }
                        break;

                    case 'w':
                        if (pendingDir == 'n')
                        {
                            pendingDir = '\0';
                            yield return HexDir.NorthWest;
                        }
                        else if (pendingDir == 's')
                        {
                            pendingDir = '\0';
                            yield return HexDir.SouthWest;
                        }
                        else
                        {
                            yield return HexDir.West;
                        }
                        break;

                    default:
                        throw new Exception();
                }
            }
        }

        public enum HexDir
        {
            East,
            NorthEast,
            NorthWest,
            West,
            SouthWest,
            SouthEast,
            MAX
        }


    }
}
