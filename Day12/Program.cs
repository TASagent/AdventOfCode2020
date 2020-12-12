using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCTools;

namespace Day12
{
    class Program
    {
        private const string inputFile = @"../../../../input12.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 12 - Rain Risk");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            List<Navigation> instructions = File.ReadAllLines(inputFile)
                .Select(x => new Navigation(x))
                .ToList();

            Ship ship = new Ship();
            ShipPart2 shipPart2 = new ShipPart2();

            foreach (Navigation navigation in instructions)
            {
                navigation.ExecuteInstructions(ship);
                navigation.ExecuteInstructions(shipPart2);
            }

            int output1 = Math.Abs(ship.position.x) + Math.Abs(ship.position.y);

            Console.WriteLine($"The answer is: {output1}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            long output2 = Math.Abs(shipPart2.position.x) + Math.Abs(shipPart2.position.y);

            Console.WriteLine($"The answer is: {output2}");


            Console.WriteLine();
            Console.ReadKey();
        }

        public class Ship
        {
            public Point2D position;
            public Point2D facing = Point2D.XAxis;
        }

        public class ShipPart2
        {
            public LongPoint2D position;
            public LongPoint2D waypointPosition = new LongPoint2D(10, 1);
        }

        public class Navigation
        {
            private readonly Instruction instr;
            private readonly int quantity;

            public Navigation(string input)
            {
                instr = InterpretInstr(input[0]);
                quantity = int.Parse(input[1..]);
            }

            private static Instruction InterpretInstr(char instr)
            {
                switch (instr)
                {
                    case 'N': return Instruction.North;
                    case 'S': return Instruction.South;
                    case 'E': return Instruction.East;
                    case 'W': return Instruction.West;
                    case 'L': return Instruction.Left;
                    case 'R': return Instruction.Right;
                    case 'F': return Instruction.Forward;

                    default: throw new Exception();
                }
            }

            private static Point2D Rotate(Point2D facing, Instruction instr, int quantity)
            {
                quantity %= 360;


                switch (instr)
                {
                    case Instruction.Left:
                        while (quantity > 0)
                        {
                            facing = new Point2D(-facing.y, facing.x);
                            quantity -= 90;
                        }
                        break;

                    case Instruction.Right:
                        while (quantity > 0)
                        {
                            facing = new Point2D(facing.y, -facing.x);
                            quantity -= 90;
                        }
                        break;

                    default: throw new Exception();
                }

                return facing;
            }

            private static LongPoint2D Rotate(LongPoint2D facing, Instruction instr, int quantity)
            {
                quantity %= 360;


                switch (instr)
                {
                    case Instruction.Left:
                        while (quantity > 0)
                        {
                            facing = new LongPoint2D(-facing.y, facing.x);
                            quantity -= 90;
                        }
                        break;

                    case Instruction.Right:
                        while (quantity > 0)
                        {
                            facing = new LongPoint2D(facing.y, -facing.x);
                            quantity -= 90;
                        }
                        break;

                    default: throw new Exception();
                }

                return facing;
            }

            public void ExecuteInstructions(Ship ship)
            {
                switch (instr)
                {
                    case Instruction.North:
                        ship.position += new Point2D(0, quantity);
                        break;

                    case Instruction.South:
                        ship.position += new Point2D(0, -quantity);
                        break;

                    case Instruction.East:
                        ship.position += new Point2D(quantity, 0);
                        break;

                    case Instruction.West:
                        ship.position += new Point2D(-quantity, 0);
                        break;

                    case Instruction.Left:
                    case Instruction.Right:
                        ship.facing = Rotate(ship.facing, instr, quantity);
                        break;

                    case Instruction.Forward:
                        ship.position += quantity * ship.facing;
                        break;

                    default: throw new Exception();
                }
            }

            public void ExecuteInstructions(ShipPart2 ship)
            {
                switch (instr)
                {
                    case Instruction.North:
                        ship.waypointPosition += new LongPoint2D(0, quantity);
                        break;

                    case Instruction.South:
                        ship.waypointPosition += new LongPoint2D(0, -quantity);
                        break;

                    case Instruction.East:
                        ship.waypointPosition += new LongPoint2D(quantity, 0);
                        break;

                    case Instruction.West:
                        ship.waypointPosition += new LongPoint2D(-quantity, 0);
                        break;

                    case Instruction.Left:
                    case Instruction.Right:
                        ship.waypointPosition = Rotate(ship.waypointPosition, instr, quantity);
                        break;

                    case Instruction.Forward:
                        ship.position += quantity * ship.waypointPosition;
                        break;

                    default: throw new Exception();
                }
            }
        }

        public enum Instruction
        {
            North = 0,
            South,
            East,
            West,
            Left,
            Right,
            Forward,
            MAX
        }

    }
}
