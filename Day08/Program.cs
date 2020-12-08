using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day08
{
    class Program
    {
        private const string inputFile = @"../../../../input08.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 8 - Handheld Halting");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            List<Instruction> instructions = File.ReadAllLines(inputFile).Select(x => new Instruction(x)).ToList();

            int instrIndex = 0;
            int acc = 0;

            HashSet<int> visitedInstructions = new HashSet<int>();

            while (visitedInstructions.Add(instrIndex))
            {
                instructions[instrIndex].Execute(ref instrIndex, ref acc);
            }

            Console.WriteLine($"The answer is: {acc}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();



            foreach (int index in visitedInstructions)
            {
                HashSet<int> visitedInstructions2 = new HashSet<int>();

                instrIndex = 0;
                acc = 0;
                bool success = false;

                while (visitedInstructions2.Add(instrIndex))
                {
                    if (instrIndex == instructions.Count)
                    {
                        success = true;
                        break;
                    }

                    if (index == instrIndex)
                    {
                        instructions[instrIndex].ExecuteFlipped(ref instrIndex, ref acc);
                    }
                    else
                    {
                        instructions[instrIndex].Execute(ref instrIndex, ref acc);
                    }
                }

                if (success)
                {
                    break;
                }    
            }


            Console.WriteLine($"The answer is: {acc}");


            Console.WriteLine();
            Console.ReadKey();
        }

        public class Instruction
        {
            private readonly InstrType instrType;
            private readonly int diff;

            public Instruction(string input)
            {
                string[] splitInput = input.Split(' ');

                instrType = Enum.Parse<InstrType>(splitInput[0]);
                diff = int.Parse(splitInput[1]);
            }

            public void Execute(ref int instrIndex, ref int acc)
            {
                switch (instrType)
                {
                    case InstrType.nop:
                        instrIndex++;
                        break;

                    case InstrType.jmp:
                        instrIndex += diff;
                        break;

                    case InstrType.acc:
                        acc += diff;
                        instrIndex++;
                        break;

                    default:
                        throw new Exception("Not Implemented");
                }
            }

            public void ExecuteFlipped(ref int instrIndex, ref int acc)
            {
                switch (instrType)
                {
                    case InstrType.jmp:
                        instrIndex++;
                        break;

                    case InstrType.nop:
                        instrIndex += diff;
                        break;

                    case InstrType.acc:
                        acc += diff;
                        instrIndex++;
                        break;

                    default:
                        throw new Exception("Not Implemented");
                }
            }

        }

        public enum InstrType
        {
            nop,
            acc,
            jmp
        }


    }
}
