using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AoCTools;

namespace Day04
{
    class Program
    {
        private const string inputFile = @"../../../../input04.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 4 - Passport Processing");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            string[] passportText = File.ReadAllText(inputFile).Split("\r\n\r\n");

            List<Passport> allPassports = passportText.Select(x => new Passport(x)).ToList();


            int output1 = allPassports.Count(x=>x.IsValid());

            Console.WriteLine($"The answer is: {output1}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            int output2 = allPassports.Count(x => x.IsValid2());

            Console.WriteLine($"The answer is: {output2}");

            Console.WriteLine();
            Console.ReadKey();
        }

        public class Passport
        {
            private Dictionary<string, string> fields = new Dictionary<string, string>();
            private static Regex colorChecker = new Regex("#[0-9a-f]{6}");
            private static Regex pidChecker = new Regex("^[0-9]{9}$");

            private static string[] necessaryFields = new[]
            {
                "byr",
                "iyr",
                "eyr",
                "hgt",
                "hcl",
                "ecl",
                "pid"
            };


            public Passport(string input)
            {
                string[] fieldSegments = input.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries); ;

                foreach (string fieldSegment in fieldSegments)
                {
                    string[] splitSegment = fieldSegment.Split(':');

                    fields.Add(splitSegment[0], splitSegment[1]);
                }
            }

            public bool IsValid()
            {
                foreach (string necessaryField in necessaryFields)
                {
                    if (!fields.ContainsKey(necessaryField))
                    {
                        return false;
                    }
                }

                return true;
            }

            public bool IsValid2()
            {
                if (!IsValid())
                {
                    return false;
                }

                if (!ValidateYear(fields["byr"], 1920, 2002))
                {
                    return false;
                }

                if (!ValidateYear(fields["iyr"], 2010, 2020))
                {
                    return false;
                }

                if (!ValidateYear(fields["eyr"], 2020, 2030))
                {
                    return false;
                }

                if (!ValidateHeight(fields["hgt"]))
                {
                    return false;
                }

                if (!ValidateHairColor(fields["hcl"]))
                {
                    return false;
                }

                if (!ValidateEyeColor(fields["ecl"]))
                {
                    return false;
                }

                if (!ValidatePassportID(fields["pid"]))
                {
                    return false;
                }

                return true;
            }

            public static bool ValidatePassportID(string input) => pidChecker.IsMatch(input);

            public static bool ValidateEyeColor(string input) => input switch
            {
                "amb" or "blu" or "brn" or "gry" or "grn" or "hzl" or "oth" => true,
                _ => false,
            };

            public static bool ValidateHairColor(string input)
            {
                if (input.Length != 7)
                {
                    return false;
                }

                return colorChecker.IsMatch(input);
            }

            public static bool ValidateHeight(string input)
            {
                if (input.Length < 4 || input.Length > 5 || !input[..^2].All(char.IsNumber))
                {
                    return false;
                }

                if (input.EndsWith("cm"))
                {
                    int height = int.Parse(input[..^2]);

                    if (height >= 150 && height <= 193)
                    {
                        return true;
                    }
                }
                else if (input.EndsWith("in"))
                {
                    int height = int.Parse(input[..^2]);

                    if (height >= 59 && height <= 76)
                    {
                        return true;
                    }
                }

                return false;
            }


            public static bool ValidateYear(string input, int lowerBound, int upperBound)
            {
                if (input.Length != 4 || !input.All(char.IsNumber))
                {
                    return false;
                }

                int year = int.Parse(input);

                return year >= lowerBound && year <= upperBound;
            }
        }
    }
}
