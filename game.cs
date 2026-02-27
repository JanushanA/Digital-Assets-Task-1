using System;
using System.Collections.Generic;

namespace Games
{
    internal class Program
    {
        private static readonly Random Rng = new Random();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("                GAMES              ");
                Console.WriteLine("==================================");
                Console.WriteLine("1) Naughts and Crosses (PvP)");
                Console.WriteLine("2) Rock Paper Scissors (PvComputer)");
                Console.WriteLine("3) Blackjack (PvDealer)");
                Console.WriteLine("4) Exit");
                Console.WriteLine();

                int choice = GetIntInRange("Choose an option (1-4): ", 1, 4);

                if (choice == 4)
                {
                    Console.WriteLine("\nGame closing!");
                    break;
                }

                switch (choice)
                {
                    case 1:
                        PlayNaughtsAndCrosses();
                        break;
                    case 2:
                        PlayRockPaperScissors();
                        break;
                    case 3:
                        PlayBlackjack();
                        break;
                }
            }
        }

        // Shared Input Functions

        private static int GetIntInRange(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (!int.TryParse(input, out int value))
                {
                    Console.WriteLine("Invalid input. Enter a number.");
                    continue;
                }

                if (value < min || value > max)
                {
                    Console.WriteLine($"Enter a number between {min} and {max}.");
                    continue;
                }

                return value;
            }
        }

        private static bool GetYesNo(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Enter Y or N.");
                    continue;
                }

                input = input.Trim().ToUpper();

                if (input == "Y") return true;
                if (input == "N") return false;

                Console.WriteLine("Invalid input. Enter Y or N.");
            }
        }

        private static char GetChoiceChar(string prompt, params char[] allowed)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Invalid input.");
                    continue;
                }

                char c = char.ToUpper(input.Trim()[0]);

                foreach (char a in allowed)
                {
                    if (c == char.ToUpper(a))
                        return c;
                }

                Console.WriteLine("Invalid choice.");
            }
        }

        private static void Pause()
        {
            Console.WriteLine("\nPress Enter to return to the menu...");
            Console.ReadLine();
        }

        // Game 1: Naughts and Crosses

        private static void PlayNaughtsAndCrosses()
        {
            int xWins = 0, oWins = 0, draws = 0;

            while (true)
            {
                // Board 9 cells
                char[] board = new char[9];
                for (int i = 0; i < 9; i++) board[i] = ' ';

                char current = 'X';

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("NAUGHTS AND CROSSES\n");
                    Console.WriteLine($"Score: X={xWins}  O={oWins}  Draws={draws}\n");

                    DisplayBoard(board);

                    Console.WriteLine();
                    Console.WriteLine($"Player {current}'s turn.");
                    Console.WriteLine("Choose a square by typing a number 1-9:");

                    int move = GetIntInRange("Move (1-9): ", 1, 9);

                    // Convert input into an array(0-8)
                    int index = move - 1;

                    // validity check
                    if (board[index] != ' ')
                    {
                        Console.WriteLine("That square is already taken. Try again.");
                        Console.ReadLine();
                        continue;
                    }

                    board[index] = current;

                    if (CheckWin(board, current))
                    {
                        Console.Clear();
                        Console.WriteLine("NAUGHTS AND CROSSES\n");
                        DisplayBoard(board);
                        Console.WriteLine($"\nPlayer {current} wins!");
                        if (current == 'X') xWins++; else oWins++;
                        break;
                    }

                    if (CheckDraw(board))
                    {
                        Console.Clear();
                        Console.WriteLine("NAUGHTS AND CROSSES\n");
                        DisplayBoard(board);
                        Console.WriteLine("\nDraw!");
                        draws++;
                        break;
                    }

                    current = current == 'X' ? 'O' : 'X';
                }

                if (!GetYesNo("Play again? (Y/N): "))
                    break;
            }

            Pause();
        }

        private static void DisplayBoard(char[] board)
        {
            // Visual aid board
            string Cell(int i)
            {
                return board[i] == ' ' ? (i + 1).ToString() : board[i].ToString();
            }

            Console.WriteLine("┌───┬───┬───┐");
            Console.WriteLine($"│ {Cell(0)} │ {Cell(1)} │ {Cell(2)} │");
            Console.WriteLine("├───┼───┼───┤");
            Console.WriteLine($"│ {Cell(3)} │ {Cell(4)} │ {Cell(5)} │");
            Console.WriteLine("├───┼───┼───┤");
            Console.WriteLine($"│ {Cell(6)} │ {Cell(7)} │ {Cell(8)} │");
            Console.WriteLine("└───┴───┴───┘");
        }

        private static bool CheckWin(char[] b, char p)
        {
            // winning possibilites
            int[,] lines = new int[,]
            {
                {0,1,2}, {3,4,5}, {6,7,8}, // rows
                {0,3,6}, {1,4,7}, {2,5,8}, // columns
                {0,4,8}, {2,4,6}           // diagonals
            };

            // Loop through each line and check if all 3 positions match the current player
            for (int i = 0; i < lines.GetLength(0); i++)
            {
                int a = lines[i, 0];
                int c = lines[i, 1];
                int d = lines[i, 2];

                if (b[a] == p && b[c] == p && b[d] == p)
                    return true;
            }

            return false;
        }

        private static bool CheckDraw(char[] b)
        {
            // If there is any empty square left, it is not a draw.
            for (int i = 0; i < 9; i++)
            {
                if (b[i] == ' ') return false;
            }
            return true;
        }

        // Game 2: Rock Paper Scissors

        private static void PlayRockPaperScissors()
        {
            int wins = 0, losses = 0, draws = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("ROCK PAPER SCISSORS\n");
                Console.WriteLine($"Score: Wins={wins} Looses={losses} Draws={draws}\n");

                Console.WriteLine("1) Rock");
                Console.WriteLine("2) Paper");
                Console.WriteLine("3) Scissors");

                int player = GetIntInRange("Choose (1-3): ", 1, 3);
                int computer = Rng.Next(1, 4);

                Console.WriteLine($"\nComputer chose {computer}");

                if (player == computer)
                {
                    Console.WriteLine("Draw!");
                    draws++;
                }
                else if ((player == 1 && computer == 3) ||
                         (player == 2 && computer == 1) ||
                         (player == 3 && computer == 2))
                {
                    Console.WriteLine("You win!");
                    wins++;
                }
                else
                {
                    Console.WriteLine("You lose!");
                    losses++;
                }

                if (!GetYesNo("Play again? (Y/N): "))
                    break;
            }

            Pause();
        }

        // Game 3: Blackjack

        private static void PlayBlackjack()
        {
            int wins = 0, losses = 0, pushes = 0;

            while (true)
            {
                List<string> playerNames = new List<string>();
                List<int> playerValues = new List<int>();

                List<string> dealerNames = new List<string>();
                List<int> dealerValues = new List<int>();

                DealCard(playerNames, playerValues);
                DealCard(dealerNames, dealerValues);
                DealCard(playerNames, playerValues);
                DealCard(dealerNames, dealerValues);

                bool playerBusted = false;

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("BLACKJACK\n");
                    Console.WriteLine($"Score: Wins={wins} Losses={losses} Pushes={pushes}\n");

                    Console.WriteLine($"Dealer shows: {dealerNames[0]}  [Hidden]\n");

                    int playerTotal = CalculateTotal(playerValues);

                    Console.WriteLine($"Your hand: {Join(playerNames)}");
                    Console.WriteLine($"Total: {playerTotal}\n");

                    if (playerTotal > 21)
                    {
                        playerBusted = true;
                        break;
                    }

                    char choice = GetChoiceChar("Hit or Stand? (H/S): ", 'H', 'S');
                    if (choice == 'S') break;

                    DealCard(playerNames, playerValues);
                }

                bool dealerBusted = false;

                if (!playerBusted)
                {
                    while (CalculateTotal(dealerValues) < 17)
                        DealCard(dealerNames, dealerValues);

                    if (CalculateTotal(dealerValues) > 21)
                        dealerBusted = true;
                }

                Console.Clear();
                Console.WriteLine("BLACKJACK RESULT\n");

                int pTotal = CalculateTotal(playerValues);
                int dTotal = CalculateTotal(dealerValues);

                Console.WriteLine($"Dealer: {Join(dealerNames)}  ({dTotal})");
                Console.WriteLine($"You:    {Join(playerNames)}  ({pTotal})\n");

                if (playerBusted)
                {
                    Console.WriteLine("You busted. You lose!");
                    losses++;
                }
                else if (dealerBusted)
                {
                    Console.WriteLine("Dealer busted. You win!");
                    wins++;
                }
                else if (pTotal > dTotal)
                {
                    Console.WriteLine("You win!");
                    wins++;
                }
                else if (pTotal < dTotal)
                {
                    Console.WriteLine("You lose!");
                    losses++;
                }
                else
                {
                    Console.WriteLine("Push.");
                    pushes++;
                }

                if (!GetYesNo("Play again? (Y/N): "))
                    break;
            }

            Pause();
        }

        private static void DealCard(List<string> names, List<int> values)
        {
            int rank = Rng.Next(1, 14);

            if (rank == 1)
            {
                names.Add("A");
                values.Add(11);
            }
            else if (rank <= 10)
            {
                names.Add(rank.ToString());
                values.Add(rank);
            }
            else
            {
                names.Add(rank == 11 ? "J" : rank == 12 ? "Q" : "K");
                values.Add(10);
            }
        }

        private static int CalculateTotal(List<int> values)
        {
            int total = 0;
            int aces = 0;

            foreach (int v in values)
            {
                total += v;
                if (v == 11) aces++;
            }

            while (total > 21 && aces > 0)
            {
                total -= 10;
                aces--;
            }

            return total;
        }

        private static string Join(List<string> cards)
        {
            string result = "";
            for (int i = 0; i < cards.Count; i++)
            {
                result += cards[i];
                if (i < cards.Count - 1) result += ", ";
            }
            return result;
        }
    }
}
