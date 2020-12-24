using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day21
{
    class Program
    {
        private const string inputFile = @"../../../../input21.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 21 - Allergen Assessment");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            List<Food> foods = File.ReadAllLines(inputFile)
                .Select(x => new Food(x))
                .ToList();

            List<string> allergenTexts = foods
                .SelectMany(x => x.allergens)
                .Distinct()
                .ToList();

            HashSet<string> uniqueIngredients = new HashSet<string>(foods.SelectMany(x => x.ingredients));

            List<Allergen> allergens = new List<Allergen>();

            foreach (string allergenText in allergenTexts)
            {
                Allergen allergen = new Allergen(allergenText);

                foreach (Food food in foods.Where(x => x.allergens.Contains(allergenText)))
                {
                    allergen.AddIngredientCollection(food.ingredients);
                }

                allergens.Add(allergen);
            }


            bool finished = false;
            HashSet<Allergen> figuredOutAllergens = new HashSet<Allergen>();

            while (!finished)
            {
                finished = true;

                foreach (Allergen allergen in allergens)
                {
                    if (allergen.IsFiguredOut &&
                        !figuredOutAllergens.Contains(allergen))
                    {
                        foreach (Allergen removeTarget in allergens.Where(x => x != allergen))
                        {
                            removeTarget.RemoveIngredient(allergen.MyIngredient);
                        }

                        figuredOutAllergens.Add(allergen);

                        finished = false;
                    }
                }
            }

            HashSet<string> nonAllergenIngredients =
                new HashSet<string>(uniqueIngredients);

            foreach (Allergen allergen in allergens)
            {
                nonAllergenIngredients.ExceptWith(allergen.ingredientCollection);
            }

            int output1 = foods
                .SelectMany(x => x.ingredients)
                .Where(x => nonAllergenIngredients.Contains(x))
                .Count();

            Console.WriteLine($"The answer is: {output1}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            Console.WriteLine($"The answer is: {string.Join(',', allergens.OrderBy(x => x.name).Select(x => x.MyIngredient))}");


            Console.WriteLine();
            Console.ReadKey();
        }
    }

    public class Food
    {
        public readonly List<string> ingredients = new List<string>();
        public readonly List<string> allergens = new List<string>();

        private static readonly Regex ingredientGetter = new Regex(@"\b(\w+)\b");

        public Food(string input)
        {
            const string ContainsTarget = "contains ";
            int indexOfParen = input.IndexOf('(');
            int indexOfContains = input.IndexOf(ContainsTarget) + ContainsTarget.Length;

            foreach (Match match in ingredientGetter.Matches(input[0..indexOfParen]))
            {
                ingredients.Add(match.Groups[1].Value);
            }

            foreach (Match match in ingredientGetter.Matches(input[indexOfContains..]))
            {
                allergens.Add(match.Groups[1].Value);
            }
        }
    }

    public class Allergen
    {
        public readonly string name;

        public HashSet<string> ingredientCollection = null;

        public bool IsFiguredOut => ingredientCollection.Count == 1;
        public string MyIngredient => ingredientCollection.Count == 1 ?
            ingredientCollection.First() : throw new Exception();

        public Allergen(string name)
        {
            this.name = name;
        }

        public void AddIngredientCollection(IEnumerable<string> ingredients)
        {
            if (ingredientCollection is null)
            {
                ingredientCollection = new HashSet<string>(ingredients);
            }
            else
            {
                ingredientCollection.IntersectWith(ingredients);
            }
        }

        public void RemoveIngredient(string ingredient)
        {
            if (ingredientCollection.Contains(ingredient))
            {
                ingredientCollection.Remove(ingredient);
            }
        }
    }
}
