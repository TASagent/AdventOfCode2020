using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCTools;

namespace Day17
{
    class Program
    {
        private const string inputFile = @"../../../../input17.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 17 - Conway Cubes");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            char[,] grid = File.ReadAllLines(inputFile).ToArrayGrid();

            {
                HashSet<Point3D> activeStates = new HashSet<Point3D>();
                HashSet<Point3D> tempActiveStates = new HashSet<Point3D>();

                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    for (int y = 0; y < grid.GetLength(1); y++)
                    {
                        if (grid[x, y] == '#')
                        {
                            activeStates.Add((x, y, 0));
                        }
                    }
                }

                int cycleCount = 0;


                while (cycleCount < 6)
                {
                    //Perform a cycle
                    tempActiveStates.Clear();

                    Point3D min = new Point3D(int.MaxValue, int.MaxValue, int.MaxValue);
                    Point3D max = new Point3D(int.MinValue, int.MinValue, int.MinValue);

                    foreach (Point3D point in activeStates)
                    {
                        min = AoCMath.Min(min, point);
                        max = AoCMath.Max(max, point);
                    }

                    for (int x = min.x - 1; x <= max.x + 1; x++)
                    {
                        for (int y = min.y - 1; y <= max.y + 1; y++)
                        {
                            for (int z = min.z - 1; z <= max.z + 1; z++)
                            {
                                if (CheckState(activeStates, x, y, z))
                                {
                                    tempActiveStates.Add(new Point3D(x, y, z));
                                }
                            }
                        }
                    }

                    cycleCount++;
                    (activeStates, tempActiveStates) = (tempActiveStates, activeStates);
                }


                int output1 = activeStates.Count();

                Console.WriteLine($"The answer is: {output1}");
            }

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            {
                HashSet<Point4D> activeStates = new HashSet<Point4D>();
                HashSet<Point4D> tempActiveStates = new HashSet<Point4D>();

                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    for (int y = 0; y < grid.GetLength(1); y++)
                    {
                        if (grid[x, y] == '#')
                        {
                            activeStates.Add((x, y, 0, 0));
                        }
                    }
                }

                int cycleCount = 0;


                while (cycleCount < 6)
                {
                    //Perform a cycle
                    tempActiveStates.Clear();

                    Point4D min = new Point4D(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);
                    Point4D max = new Point4D(int.MinValue, int.MinValue, int.MinValue, int.MinValue); ;

                    foreach (Point4D point in activeStates)
                    {
                        min = AoCMath.Min(min, point);
                        max = AoCMath.Max(max, point);
                    }

                    for (int x = min.x - 1; x <= max.x + 1; x++)
                    {
                        for (int y = min.y - 1; y <= max.y + 1; y++)
                        {
                            for (int z = min.z - 1; z <= max.z + 1; z++)
                            {
                                for (int w = min.w - 1; w <= max.w + 1; w++)
                                {
                                    if (CheckState(activeStates, x, y, z, w))
                                    {
                                        tempActiveStates.Add(new Point4D(x, y, z, w));
                                    }
                                }
                            }
                        }
                    }

                    cycleCount++;
                    (activeStates, tempActiveStates) = (tempActiveStates, activeStates);
                }


                int output2 = activeStates.Count();

                Console.WriteLine($"The answer is: {output2}");
            }

            Console.WriteLine();
            Console.ReadKey();
        }


        public static bool CheckState(HashSet<Point3D> activeStates, int x0, int y0, int z0)
        {
            if (activeStates.Contains((x0, y0, z0)))
            {
                //State is active
                int neighborCount = 0;

                for (int x = x0 - 1; x <= x0 + 1; x++)
                {
                    for (int y = y0 - 1; y <= y0 + 1; y++)
                    {
                        for (int z = z0 - 1; z <= z0 + 1; z++)
                        {
                            if (activeStates.Contains((x, y, z)))
                            {
                                neighborCount++;

                                if (neighborCount > 4)
                                {
                                    return false;
                                }
                            }

                        }
                    }
                }

                return neighborCount == 3 || neighborCount == 4;
            }
            else
            {
                //State is active
                int neighborCount = 0;

                for (int x = x0 - 1; x <= x0 + 1; x++)
                {
                    for (int y = y0 - 1; y <= y0 + 1; y++)
                    {
                        for (int z = z0 - 1; z <= z0 + 1; z++)
                        {
                            if (activeStates.Contains((x, y, z)))
                            {
                                neighborCount++;

                                if (neighborCount > 3)
                                {
                                    return false;
                                }
                            }

                        }
                    }
                }

                return neighborCount == 3;
            }
        }

        public static bool CheckState(HashSet<Point4D> activeStates, int x0, int y0, int z0, int w0)
        {
            if (activeStates.Contains((x0, y0, z0, w0)))
            {
                //State is active
                int neighborCount = 0;

                for (int x = x0 - 1; x <= x0 + 1; x++)
                {
                    for (int y = y0 - 1; y <= y0 + 1; y++)
                    {
                        for (int z = z0 - 1; z <= z0 + 1; z++)
                        {
                            for (int w = w0 - 1; w <= w0 + 1; w++)
                            {
                                if (activeStates.Contains((x, y, z, w)))
                                {
                                    neighborCount++;

                                    if (neighborCount > 4)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }

                return neighborCount == 3 || neighborCount == 4;
            }
            else
            {
                //State is active
                int neighborCount = 0;

                for (int x = x0 - 1; x <= x0 + 1; x++)
                {
                    for (int y = y0 - 1; y <= y0 + 1; y++)
                    {
                        for (int z = z0 - 1; z <= z0 + 1; z++)
                        {
                            for (int w = w0 - 1; w <= w0 + 1; w++)
                            {
                                if (activeStates.Contains((x, y, z, w)))
                                {
                                    neighborCount++;

                                    if (neighborCount > 3)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }

                return neighborCount == 3;
            }
        }
    }
}
