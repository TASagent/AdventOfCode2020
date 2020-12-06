using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day06
{
    class Program
    {
        private const string inputFile = @"../../../../input06.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 6 - Custom Customs");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            string[] lines = File.ReadAllText(inputFile)
                .Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);

            List<CustomsGroup> customGroups = lines.Select(x => new CustomsGroup(x)).ToList();

            int output1 = customGroups.Sum(x=>x.GetCount());

            Console.WriteLine($"The answer is: {output1}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            int output2 = customGroups.Sum(x => x.GetEveryoneCount());

            Console.WriteLine($"The answer is: {output2}");


            Console.WriteLine();
            Console.ReadKey();
        }

        public class CustomsGroup
        {
            private HashSet<char> yesQuestions;

            private HashSet<char> everyoneYes;

            public CustomsGroup(string input)
            {
                yesQuestions = new HashSet<char>(input.Where(char.IsLetter));

                string[] splitInput = input.Split("\r\n");

                everyoneYes = new HashSet<char>(splitInput[0]);

                for (int i = 1; i < splitInput.Length; i++)
                {
                    everyoneYes.IntersectWith(splitInput[i]);
                }
            }

            public int GetCount() => yesQuestions.Count;
            public int GetEveryoneCount() => everyoneYes.Count;
        }
    }
}
