using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day07
{
    class Program
    {
        private const string inputFile = @"../../../../input07.txt";

        private const string targetBag = "shiny gold";

        private static Bag shinyGoldBag;
        private static Dictionary<string, Bag> allBags;

        static void Main(string[] args)
        {
            Console.WriteLine("Day 7 - Handy Haversacks");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            allBags = new Dictionary<string, Bag>();

            List<Bag> inputBags = File.ReadAllLines(inputFile).Select(x => new Bag(x)).ToList();

            foreach (Bag bag in inputBags)
            {
                allBags.Add(bag.bagType, bag);
            }


            shinyGoldBag = allBags[targetBag];

            int output1 = 0;


            foreach (Bag parentBag in inputBags)
            {
                foreach (Bag internalBag in GetInternalBags(parentBag))
                {
                    if (internalBag == shinyGoldBag)
                    {
                        output1++;
                        break;
                    }
                }
            }

            output1--;

            Console.WriteLine($"The answer is: {output1}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            int output2 = shinyGoldBag.GetBagCounts() - 1;

            Console.WriteLine($"The answer is: {output2}");


            Console.WriteLine();
            Console.ReadKey();
        }

        public static IEnumerable<Bag> GetInternalBags(Bag parent)
        {
            yield return parent;

            foreach (Bag internalBag in parent.GetInternalBagTypes())
            {
                foreach (Bag returnBag in GetInternalBags(internalBag))
                {
                    yield return returnBag;
                }
            }
        }

        public class Bag
        {
            public readonly string bagType;

            private readonly List<(int, string)> internalBags = new List<(int, string)>();

            private static readonly Regex bagCapture = new Regex(@"^(\w+ \w+) bags");
            private static readonly Regex internalBagCapture = new Regex(@"(\d+) (\w+ \w+) bag");

            public Bag(string input)
            {
                Match match = bagCapture.Match(input);

                bagType = match.Groups[1].Value;

                foreach (Match internalMatch in internalBagCapture.Matches(input))
                {
                    int count = int.Parse(internalMatch.Groups[1].Value);
                    string bag = internalMatch.Groups[2].Value;

                    internalBags.Add((count, bag));
                }
            }

            public IEnumerable<Bag> GetInternalBagTypes()
            {
                foreach ((int count, string bagType) in internalBags)
                {
                    yield return allBags[bagType];
                }
            }

            public int GetBagCounts()
            {
                int totalBags = 1;

                foreach ((int count, string bagType) in internalBags)
                {
                    totalBags += count * allBags[bagType].GetBagCounts();
                }

                return totalBags;
            }

        }
    }
}
