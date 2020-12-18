using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day18
{
    class Program
    {
        private const string inputFile = @"../../../../input18.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 18 - Operation Order");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            List<long> values = File.ReadAllLines(inputFile).Select(Evaluate).ToList();

            long output1 = values.Sum();

            Console.WriteLine($"The answer is: {output1}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            List<long> values2 = File.ReadAllLines(inputFile).Select(Evaluate2).ToList();

            long output2 = values2.Sum();

            Console.WriteLine($"The answer is: {output2}");

            Console.WriteLine();
            Console.ReadKey();
        }

        public static long Evaluate2(string input)
        {
            int index = 0;
            return EvaluateParens2(input, ref index);
        }

        public static long EvaluateParens2(string input, ref int index)
        {
            StringBuilder scopeBuilder = new StringBuilder();

            while (index < input.Length)
            {
                char currentChar = input[index];

                if (currentChar == '(')
                {
                    index++;
                    long internalValue = EvaluateParens2(input, ref index);

                    scopeBuilder.Append(internalValue.ToString());
                }
                else if (currentChar == ')')
                {
                    index++;
                    return EvaluateOperators2(scopeBuilder.ToString());
                }
                else if (char.IsWhiteSpace(currentChar))
                {
                    index++;
                }
                else
                {
                    scopeBuilder.Append(currentChar);
                    index++;
                }
            }

            return EvaluateOperators2(scopeBuilder.ToString());
        }

        public static long EvaluateOperators2(string input)
        {
            Stack<long> valueStack = new Stack<long>();
            char lastOperator = '\0';
            int index = 0;

            while (index < input.Length)
            {
                char currentChar = input[index];

                if (char.IsDigit(currentChar))
                {
                    valueStack.Push(ParseDigit(input, ref index));

                    if (lastOperator == '+')
                    {
                        valueStack.Push(valueStack.Pop() + valueStack.Pop());
                    }
                }
                else if (currentChar == '*' || currentChar == '+')
                {
                    lastOperator = currentChar;
                    index++;
                }
                else
                {
                    throw new Exception();
                }
            }

            long outputValue = 1;

            while (valueStack.Count > 0)
            {
                outputValue *= valueStack.Pop();
            }

            return outputValue;
        }

        public static long Evaluate(string input)
        {
            int temp = 0;
            return Evaluate(input, ref temp);
        }

        public static long Evaluate(string input, ref int index)
        {
            long currentScopeValue = 0;
            char lastOperator = '+';

            while (index < input.Length)
            {
                switch (input[index])
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        //Parse Digit
                        if (lastOperator == '+')
                        {
                            currentScopeValue += ParseDigit(input, ref index);
                        }
                        else
                        {
                            currentScopeValue *= ParseDigit(input, ref index);
                        }
                        break;

                    case '+':
                    case '*':
                        lastOperator = input[index];
                        index++;
                        break;

                    case '(':
                        index++;
                        if (lastOperator == '+')
                        {
                            currentScopeValue += Evaluate(input, ref index);
                        }
                        else
                        {
                            currentScopeValue *= Evaluate(input, ref index);
                        }
                        break;

                    case ')':
                        index++;
                        return currentScopeValue;

                    case ' ':
                        index++;
                        continue;

                    default:
                        throw new Exception();
                }
            }


            return currentScopeValue;
        }

        public static long ParseDigit(string input, ref int index)
        {
            int endIndex = index + 1;

            while (endIndex < input.Length && char.IsDigit(input[endIndex]))
            {
                endIndex++;
            }

            long value = long.Parse(input[index..endIndex]);

            index = endIndex;

            return value;
        }
    }
}
