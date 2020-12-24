using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day23
{
    class Program
    {
        private const string input = "398254716";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 23 - Crab Cups");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            List<int> layout = input.Select(x => int.Parse(x.ToString())).ToList();
            int[] withdrawnCups = new int[3];

            for (int i = 0; i < 100; i++)
            {
                //Current Index is always 0
                PerformMove(layout, withdrawnCups);
            }

            int indexOfOne = layout.IndexOf(1);

            Console.WriteLine($"The original answer is: {new string(layout.Select(x => x.ToString()[0]).ToArray())}");

            var returnList = layout.Skip(indexOfOne + 1).Concat(layout.Take(indexOfOne));

            Console.WriteLine($"The answer is: {new string(returnList.Select(x => x.ToString()[0]).ToArray())}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            LinkedList<int> layout2 = new LinkedList<int>(input.Select(x => int.Parse(x.ToString())));

            int maxInitial = layout2.Max() + 1;

            for (int i = maxInitial; i <= 1_000_000; i++)
            {
                layout2.AddLast(i);
            }

            int reportNumber = 10;
            LinkedListNode<int>[] withdrawnCups2 = new LinkedListNode<int>[3];

            Dictionary<int, LinkedListNode<int>> nodeCache = new Dictionary<int, LinkedListNode<int>>();

            LinkedListNode<int> currentNode = layout2.First;

            while (currentNode != null)
            {
                nodeCache[currentNode.Value] = currentNode;
                currentNode = currentNode.Next;
            }

            for (int i = 0; i < 10_000_000; i++)
            {
                if (i == reportNumber)
                {
                    Console.WriteLine($"We have reached {i}");
                    reportNumber *= 10;
                }

                //Current Cup is always Head
                PerformMove(layout2, withdrawnCups2, nodeCache);
            }

            LinkedListNode<int> cupOne = layout2.Find(1);

            long output2 = (long)cupOne.Next.Value * (long)cupOne.Next.Next.Value;

            Console.WriteLine($"The answer is: {output2}");


            Console.WriteLine();
            Console.ReadKey();
        }

        public static void PerformMove(List<int> input, int[] withdrawnCups)
        {
            withdrawnCups[0] = input[1];
            withdrawnCups[1] = input[2];
            withdrawnCups[2] = input[3];

            input.RemoveAt(1);
            input.RemoveAt(1);
            input.RemoveAt(1);

            int currentMin = input.Min();
            int currentMax = input.Max();

            int targetNumber = input[0] - 1;

            while (!input.Contains(targetNumber))
            {
                if (targetNumber < currentMin)
                {
                    targetNumber = currentMax;
                    break;
                }
                else
                {
                    targetNumber--;
                }
            }

            int insertionIndex = input.IndexOf(targetNumber) + 1;

            for (int i = 0; i < 3; i++)
            {
                input.Insert(insertionIndex + i, withdrawnCups[i]);
            }

            int oldCurrentCup = input[0];
            input.RemoveAt(0);
            input.Add(oldCurrentCup);
        }

        public static void PerformMove(
            LinkedList<int> input,
            LinkedListNode<int>[] withdrawnCups,
            Dictionary<int, LinkedListNode<int>> nodeCache)
        {
            LinkedListNode<int> head = input.First;

            withdrawnCups[0] = head.Next;
            withdrawnCups[1] = withdrawnCups[0].Next;
            withdrawnCups[2] = withdrawnCups[1].Next;
            input.Remove(head.Next);
            input.Remove(head.Next);
            input.Remove(head.Next);

            int targetNumber = head.Value - 1;

            if (targetNumber == 0)
            {
                targetNumber = 1_000_000;
            }

            while (withdrawnCups.Select(x => x.Value).Contains(targetNumber))
            {
                targetNumber--;

                if (targetNumber == 0)
                {
                    targetNumber = 1_000_000;
                }
            }

            LinkedListNode<int> insertionNode = nodeCache[targetNumber];
            input.AddAfter(insertionNode, withdrawnCups[2]);
            input.AddAfter(insertionNode, withdrawnCups[1]);
            input.AddAfter(insertionNode, withdrawnCups[0]);

            input.RemoveFirst();
            input.AddLast(head);
        }
    }
}
