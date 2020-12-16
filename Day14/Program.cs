using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day14
{
    class Program
    {
        private const string inputFile = @"../../../../input14.txt";
        private static Regex memExtractor = new Regex(@"mem\[(\d+)\] \= (\d+)");

        static void Main(string[] args)
        {
            Console.WriteLine("Day 14 - Docking Data");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            Dictionary<int, long> memory = new Dictionary<int, long>();

            string[] lines = File.ReadAllLines(inputFile);

            long settingMask = 0;
            long unsettingMask = 0;

            foreach (string line in lines)
            {
                if (line.StartsWith("mask"))
                {
                    settingMask = 0;
                    unsettingMask = 0;

                    foreach (char c in line.Split(" = ")[1])
                    {
                        settingMask <<= 1;
                        unsettingMask <<= 1;
                        switch (c)
                        {
                            case '1':
                                settingMask |= 1;
                                unsettingMask |= 1;
                                break;

                            case '0':
                                //settingMask gets a zero
                                //unsettingMask gets a zero
                                break;

                            case 'X':
                                //settingMask gets a zero
                                unsettingMask |= 1;
                                break;

                            default:
                                throw new Exception();
                        }
                    }

                }
                else if (line.StartsWith("mem"))
                {
                    Match match = memExtractor.Match(line);

                    int address = int.Parse(match.Groups[1].Value);
                    long value = long.Parse(match.Groups[2].Value);

                    value &= unsettingMask;
                    value |= settingMask;

                    memory[address] = value;
                }
                else
                {
                    throw new Exception();
                }
            }

            long output1 = memory.Values.Sum();


            Console.WriteLine($"Max X line: {lines.Select(x => x.Count(c => c == 'X')).Max()}");


            Console.WriteLine($"The answer is: {output1}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();


            Dictionary<long, long> memory2 = new Dictionary<long, long>();
            List<int> wildIndices = new List<int>();

            foreach (string line in lines)
            {
                if (line.StartsWith("mask"))
                {
                    settingMask = 0;
                    wildIndices.Clear();

                    char[] mask = line.Split(" = ")[1].Reverse().ToArray();

                    for (int i = 0; i < mask.Length; i++)
                    {
                        switch (mask[i])
                        {
                            case '1':
                                settingMask |= (1L << i);
                                break;

                            case '0':
                                //settingMask gets a zero
                                break;

                            case 'X':
                                //settingMask gets a zero
                                wildIndices.Add(i);
                                break;

                            default:
                                throw new Exception();
                        }
                    }
                }
                else if (line.StartsWith("mem"))
                {
                    Match match = memExtractor.Match(line);

                    long address = long.Parse(match.Groups[1].Value);
                    long value = long.Parse(match.Groups[2].Value);

                    address |= settingMask;
                    int count = 0;

                    foreach (long newAddress in GetAllIndices(address, wildIndices, 0))
                    {
                        count++;
                        memory2[newAddress] = value;
                    }

                    Console.WriteLine($"Count: {count}");
                }
                else
                {
                    throw new Exception();
                }
            }

            long output2 = memory2.Values.Sum();

            Console.WriteLine($"The answer is: {output2}");

            Console.WriteLine();
            Console.ReadKey();
        }

        public static IEnumerable<long> GetAllIndices(long address, List<int> indices, int position)
        {
            if (position >= indices.Count)
            {
                yield return address;
                yield break;
            }

            int bitPosition = indices[position];

            foreach (long value in GetAllIndices(address, indices, position + 1))
            {
                yield return value & ~(1L << bitPosition);
                yield return value | (1L << bitPosition);
            }

        }
    }
}
