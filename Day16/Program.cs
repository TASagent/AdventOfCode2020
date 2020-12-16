using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day16
{
    class Program
    {
        private const string inputFile = @"../../../../input16.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 16 - Ticket Translation");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            string[] lines = File.ReadAllLines(inputFile);

            int phase = 0;

            List<Constraint> constraints = new List<Constraint>();

            int[] myTicket = null;

            List<int[]> passengerTickets = new List<int[]>();


            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    phase++;
                    i++;
                    continue;
                }

                if (phase == 0)
                {
                    constraints.Add(new Constraint(lines[i]));
                }
                else if (phase == 1)
                {
                    myTicket = lines[i].Split(',').Select(int.Parse).ToArray();
                }
                else if (phase == 2)
                {
                    passengerTickets.Add(lines[i].Split(',').Select(int.Parse).ToArray());
                }
                else
                {
                    throw new Exception();
                }
            }


            int output1 = 0;


            foreach (int[] passengerTicket in passengerTickets)
            {
                foreach (int value in passengerTicket)
                {
                    if (!constraints.Any(x => x.IsValid(value)))
                    {
                        output1 += value;
                    }
                }
            }

            Console.WriteLine($"The answer is: {output1}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            passengerTickets.RemoveAll(
                (int[] x) => x.Any(
                    (int y) => !constraints.Any(z => z.IsValid(y))));


            foreach (Constraint constraint in constraints)
            {
                foreach (int[] ticket in passengerTickets)
                {
                    constraint.CompareSlots(ticket);
                }
            }

            do
            {
                foreach (Constraint constraint in constraints)
                {
                    if (constraint.IsSolved())
                    {
                        int index = constraint.possibleSlots[0];
                        foreach (Constraint otherConstraint in constraints)
                        {
                            otherConstraint.RemoveSlot(index);
                        }
                    }
                }

            }
            while (constraints.Any(x => !x.IsSolved()));


            long output2 = 1;

            foreach(int index in constraints.Where(x=>x.name.Contains("departure")).Select(x=>x.possibleSlots[0]))
            {
                output2 *= myTicket[index];
            }

            Console.WriteLine($"The answer is: {output2}");

            Console.WriteLine();
            Console.ReadKey();
        }
    }

    public class Constraint
    {
        public readonly string name;
        public readonly (int lb, int ub) rangeA;
        public readonly (int lb, int ub) rangeB;

        private readonly static Regex parser = new Regex(@"(.*)\: (\d+)-(\d+) or (\d+)-(\d+)");

        public List<int> possibleSlots = null;

        public Constraint(string line)
        {
            Match match = parser.Match(line);

            name = match.Groups[1].Value;

            rangeA = (int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value));
            rangeB = (int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value));
        }

        public bool IsValid(int value) =>
            (value >= rangeA.lb && value <= rangeA.ub) ||
            (value >= rangeB.lb && value <= rangeB.ub);

        public void CompareSlots(int[] values)
        {
            if (possibleSlots == null)
            {
                possibleSlots = new List<int>();

                for (int i = 0; i < values.Length; i++)
                {
                    possibleSlots.Add(i);
                }
            }

            if (possibleSlots.Count == 1)
            {
                return;
            }

            for (int i = possibleSlots.Count - 1; i >= 0; i--)
            {
                int slot = possibleSlots[i];
                int value = values[slot];

                if (!IsValid(value))
                {
                    possibleSlots.RemoveAt(i);
                }
            }
        }

        public void RemoveSlot(int index)
        {
            if (possibleSlots.Count == 1)
            {
                return;
            }

            if (possibleSlots.Contains(index))
            {
                possibleSlots.Remove(index);
            }
        }

        public bool IsSolved() => possibleSlots.Count == 1;
    }
}
