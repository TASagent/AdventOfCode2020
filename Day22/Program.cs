using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day22
{
    class Program
    {
        private const string inputFile = @"../../../../input22.txt";

        private static Dictionary<string, bool> battleResult =
            new Dictionary<string, bool>();

        private static Queue<int> endingPlayerDeck;
        private static Queue<int> endingCrabDeck;

        static void Main(string[] args)
        {
            Console.WriteLine("Day 22 - Crab Combat");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            string[] lines = File.ReadAllLines(inputFile);

            Queue<int> playerDeck = new Queue<int>();
            Queue<int> crabDeck = new Queue<int>();

            bool playerDeckMode = true;

            for (int i = 0; i < lines.Length; i++)
            {
                string currentLine = lines[i];

                if (string.IsNullOrEmpty(currentLine) || currentLine == "Player 1:")
                {
                    continue;
                }

                if (currentLine == "Player 2:")
                {
                    playerDeckMode = false;
                    continue;
                }

                if (playerDeckMode)
                {
                    playerDeck.Enqueue(int.Parse(currentLine));
                }
                else
                {
                    crabDeck.Enqueue(int.Parse(currentLine));
                }
            }

            while (playerDeck.Count > 0 && crabDeck.Count > 0)
            {
                int playerCard = playerDeck.Dequeue();
                int crabCard = crabDeck.Dequeue();

                Queue<int> winningDeck = playerCard > crabCard ? playerDeck : crabDeck;

                winningDeck.Enqueue(playerCard > crabCard ? playerCard : crabCard);
                winningDeck.Enqueue(playerCard < crabCard ? playerCard : crabCard);
            }

            long output1 = 0;

            Queue<int> finalWinningDeck = playerDeck.Count > crabDeck.Count ? playerDeck : crabDeck;

            while (finalWinningDeck.Count > 0)
            {
                long currentCount = finalWinningDeck.Count;
                output1 += currentCount * finalWinningDeck.Dequeue();
            }

            Console.WriteLine($"The answer is: {output1}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();


            {
                playerDeck.Clear();
                crabDeck.Clear();

                playerDeckMode = true;

                for (int i = 0; i < lines.Length; i++)
                {
                    string currentLine = lines[i];

                    if (string.IsNullOrEmpty(currentLine) || currentLine == "Player 1:")
                    {
                        continue;
                    }

                    if (currentLine == "Player 2:")
                    {
                        playerDeckMode = false;
                        continue;
                    }

                    if (playerDeckMode)
                    {
                        playerDeck.Enqueue(int.Parse(currentLine));
                    }
                    else
                    {
                        crabDeck.Enqueue(int.Parse(currentLine));
                    }
                }
            }

            Fight(playerDeck, crabDeck);

            finalWinningDeck = endingPlayerDeck.Count > endingCrabDeck.Count ? endingPlayerDeck : endingCrabDeck;

            long output2 = 0;

            while (finalWinningDeck.Count > 0)
            {
                long currentCount = finalWinningDeck.Count;
                output2 += currentCount * finalWinningDeck.Dequeue();
            }

            //Not  29918

            Console.WriteLine($"The answer is: {output2}");


            Console.WriteLine();
            Console.ReadKey();
        }

        public static bool Fight(
            IEnumerable<int> playerDeckInput,
            IEnumerable<int> crabDeckInput)
        {
            string battleKey = $"{string.Join(',', playerDeckInput)}vs{string.Join(',', crabDeckInput)}";
            if (battleResult.ContainsKey(battleKey))
            {
                return battleResult[battleKey];
            }

            bool playerWins = true;
            bool shortCircuitWin = false;

            HashSet<string> priorGames = new HashSet<string>();
            Queue<int> playerDeck = new Queue<int>(playerDeckInput);
            Queue<int> crabDeck = new Queue<int>(crabDeckInput);

            while (playerDeck.Count > 0 && crabDeck.Count > 0)
            {
                string deckLayout = $"{string.Join(',', playerDeck)}vs{string.Join(',', crabDeck)}";

                int playerCard = playerDeck.Dequeue();
                int crabCard = crabDeck.Dequeue();

                if (!priorGames.Add(deckLayout))
                {
                    shortCircuitWin = true;
                    break;
                }

                bool playerHasEnough = playerDeck.Count >= playerCard;
                bool crabHasEnough = crabDeck.Count >= crabCard;

                if (playerHasEnough && crabHasEnough)
                {
                    playerWins = Fight(playerDeck.Take(playerCard), crabDeck.Take(crabCard));
                }
                else
                {
                    playerWins = playerCard > crabCard;
                }

                if (playerWins)
                {
                    playerDeck.Enqueue(playerCard);
                    playerDeck.Enqueue(crabCard);
                }
                else
                {
                    crabDeck.Enqueue(crabCard);
                    crabDeck.Enqueue(playerCard);
                }
            }

            if (shortCircuitWin)
            {
                playerWins = true;
            }
            else
            {
                playerWins = playerDeck.Count > crabDeck.Count;
            }

            battleResult.Add(battleKey, playerWins);

            endingPlayerDeck = playerDeck;
            endingCrabDeck = crabDeck;

            return playerWins;
        }
    }
}
