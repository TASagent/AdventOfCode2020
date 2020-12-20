using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day19
{
    class Program
    {
        private const string inputFile = @"../../../../input19.txt";

        private static Rule ruleZero;

        static void Main(string[] args)
        {
            Console.WriteLine("Day 19 - Monster Messages");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            List<string> lines = File.ReadAllLines(inputFile).ToList();

            int splitPosition = lines.IndexOf("");

            List<Rule> rules = lines.Take(splitPosition).Select(Rule.ConstructRule).ToList();
            List<string> messages = lines.Skip(splitPosition + 1).ToList();

            ruleZero = Rule.ruleLookup[0];

            int output1 = messages.Count(CheckMessage);

            Console.WriteLine($"The answer is: {output1}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            var timer = new System.Diagnostics.Stopwatch();
            timer.Start();

            Rule.ruleLookup[8] = new AlternateRule(8, "42 | 42 8");
            Rule.ruleLookup[11] = new AlternateRule(11, "42 31 | 42 11 31");

            int output2 = messages.Count(CheckMessage);

            timer.Stop();

            Console.WriteLine($"The answer is: {output2}");
            Console.WriteLine($"The solution took: {timer.ElapsedMilliseconds} ms");


            Console.WriteLine();
            Console.ReadKey();
        }

        public static bool CheckMessage(string message)
        {
            return ruleZero.CheckRule(message, 0).Contains(message.Length);
        }
    }

    public abstract class Rule
    {
        protected readonly int index;

        public static readonly Dictionary<int, Rule> ruleLookup = new Dictionary<int, Rule>();

        public Rule(int index)
        {
            this.index = index;
        }

        public static Rule ConstructRule(string input)
        {
            int colonIndex = input.IndexOf(':');
            int index = int.Parse(input[0..colonIndex]);

            Rule newRule = null;

            if (input.Contains('|'))
            {
                newRule = new AlternateRule(index, input[(colonIndex + 1)..]);
            }
            else if (input.Contains('"'))
            {
                newRule = new ConcreteRule(index, input[(colonIndex + 1)..]);
            }
            else
            {
                newRule = new SimpleRule(index, input[(colonIndex + 1)..]);
            }

            ruleLookup.Add(newRule.index, newRule);

            return newRule;
        }

        public abstract IEnumerable<int> CheckRule(string input, int index);
    }

    public class AlternateRule : Rule
    {
        private readonly List<int> optionA = new List<int>();
        private readonly List<int> optionB = new List<int>();

        private static char[] splitChars = new[] { '|' };

        public AlternateRule(int index, string input)
            : base(index)
        {
            string[] splitInput = input.Split(splitChars);

            optionA.AddRange(splitInput[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));
            optionB.AddRange(splitInput[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));
        }

        public override IEnumerable<int> CheckRule(string input, int index)
        {
            HashSet<int> returnIndices = new HashSet<int>();

            HashSet<int> nextIndices = new HashSet<int>() { index };
            HashSet<int> newIndices = new HashSet<int>();

            for (int i = 0; i < optionA.Count; i++)
            {
                foreach (int nextIndex in nextIndices)
                {
                    newIndices.UnionWith(ruleLookup[optionA[i]].CheckRule(input, nextIndex));
                }

                if (newIndices.Count == 0)
                {
                    nextIndices.Clear();
                    break;
                }

                (nextIndices, newIndices) = (newIndices, nextIndices);

                newIndices.Clear();
            }

            returnIndices.UnionWith(nextIndices);

            nextIndices.Clear();
            nextIndices.Add(index);
            newIndices.Clear();

            for (int i = 0; i < optionB.Count; i++)
            {
                foreach (int nextIndex in nextIndices)
                {
                    newIndices.UnionWith(ruleLookup[optionB[i]].CheckRule(input, nextIndex));
                }

                if (newIndices.Count == 0)
                {
                    nextIndices.Clear();
                    break;
                }

                (nextIndices, newIndices) = (newIndices, nextIndices);

                newIndices.Clear();
            }

            returnIndices.UnionWith(nextIndices);

            foreach (int value in returnIndices)
            {
                yield return value;
            }
        }
    }

    public class ConcreteRule : Rule
    {
        private readonly char character;

        public ConcreteRule(int index, string input)
            : base(index)
        {
            character = input[2];
        }

        public override IEnumerable<int> CheckRule(string input, int index)
        {
            if (index < input.Length && input[index] == character)
            {
                yield return index + 1;
            }
        }
    }

    public class SimpleRule : Rule
    {
        private readonly List<int> rules = new List<int>();
        public SimpleRule(int index, string input)
            : base(index)
        {
            rules.AddRange(input.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));
        }

        public override IEnumerable<int> CheckRule(string input, int index)
        {
            HashSet<int> nextIndices = new HashSet<int>() { index };
            HashSet<int> newIndices = new HashSet<int>();

            for (int i = 0; i < rules.Count; i++)
            {
                foreach (int nextIndex in nextIndices)
                {
                    newIndices.UnionWith(ruleLookup[rules[i]].CheckRule(input, nextIndex));
                }

                if (newIndices.Count == 0)
                {
                    yield break;
                }

                (nextIndices, newIndices) = (newIndices, nextIndices);

                newIndices.Clear();
            }

            foreach (int nextIndex in nextIndices)
            {
                yield return nextIndex;
            }
        }
    }
}
