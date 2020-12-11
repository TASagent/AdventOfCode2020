using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCTools;

namespace Day11
{
    class Program
    {
        private const string inputFile = @"../../../../input11.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 11 - Seating System");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            char[,] seats = File.ReadAllLines(inputFile).ToArrayGrid();
            char[,] nextSeats = (char[,])seats.Clone();

            do
            {
                for (int x = 0; x < seats.GetLength(0); x++)
                {
                    for (int y = 0; y < seats.GetLength(1); y++)
                    {
                        nextSeats[x, y] = Evolve(x, y, seats);
                    }
                }

                (seats, nextSeats) = (nextSeats, seats);

            }
            while (!Compare(seats, nextSeats));


            int output1 = 0;


            for (int x = 0; x < seats.GetLength(0); x++)
            {
                for (int y = 0; y < seats.GetLength(1); y++)
                {
                    if (seats[x, y] == '#')
                    {
                        output1++;
                    }
                }
            }


            Console.WriteLine($"The answer is: {output1}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            //Reset Grids
            seats = File.ReadAllLines(inputFile).ToArrayGrid();
            nextSeats = (char[,])seats.Clone();

            do
            {
                for (int x = 0; x < seats.GetLength(0); x++)
                {
                    for (int y = 0; y < seats.GetLength(1); y++)
                    {
                        nextSeats[x, y] = EvolvePart2(x, y, seats);
                    }
                }

                (seats, nextSeats) = (nextSeats, seats);

            }
            while (!Compare(seats, nextSeats));


            int output2 = 0;


            for (int x = 0; x < seats.GetLength(0); x++)
            {
                for (int y = 0; y < seats.GetLength(1); y++)
                {
                    if (seats[x, y] == '#')
                    {
                        output2++;
                    }
                }
            }

            Console.WriteLine($"The answer is: {output2}");


            Console.WriteLine();
            Console.ReadKey();
        }

        public static bool Compare(char[,] seats, char[,] nextSeats)
        {
            for (int x = 0; x < seats.GetLength(0); x++)
            {
                for (int y = 0; y < seats.GetLength(1); y++)
                {
                    if (seats[x, y] != nextSeats[x, y])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static char Evolve(int x0, int y0, char[,] seats)
        {
            if (seats[x0, y0] == '.')
            {
                return '.';
            }

            int occupiedSeats = 0;

            for (int x = x0 - 1; x <= x0 + 1; x++)
            {
                for (int y = y0 - 1; y <= y0 + 1; y++)
                {
                    if (x >= 0 && x < seats.GetLength(0) &&
                        y >= 0 && y < seats.GetLength(1) &&
                        !(x == x0 && y == y0))
                    {
                        if (seats[x, y] == '#')
                        {
                            occupiedSeats++;
                        }
                    }
                }
            }

            if (seats[x0, y0] == '#')
            {
                return (occupiedSeats >= 4) ? 'L' : '#';
            }
            else
            {
                return (occupiedSeats == 0) ? '#' : 'L';
            }

        }

        public static char EvolvePart2(int x0, int y0, char[,] seats)
        {
            if (seats[x0, y0] == '.')
            {
                return '.';
            }

            int occupiedSeats = 0;

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0)
                    {
                        continue;
                    }

                    int steps = 0;
                    while (true)
                    {
                        steps++;

                        int x = x0 + steps * dx;
                        int y = y0 + steps * dy;

                        if (x < 0 || x >= seats.GetLength(0) ||
                            y < 0 || y >= seats.GetLength(1))
                        {
                            break;
                        }

                        if (seats[x, y] == '.')
                        {
                            continue;
                        }
                        else if (seats[x, y] == 'L')
                        {
                            break;
                        }
                        else if (seats[x, y] == '#')
                        {
                            occupiedSeats++;
                            break;
                        }
                        else throw new Exception("Wat?");
                    }
                }
            }

            if (seats[x0, y0] == '#')
            {
                return (occupiedSeats >= 5) ? 'L' : '#';
            }
            else
            {
                return (occupiedSeats == 0) ? '#' : 'L';
            }

        }

    }
}
