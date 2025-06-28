using Mastermind.Interfaces;
using Services;

namespace Mastermind
{
    /// <summary>
    /// This is the service that handles the running of the game
    /// </summary>
    public class GameHandler : IGameHandler
    {
        private readonly GameSessionService gameSessionService;
        private int[]? answer;
        private int guessesMade;
        private bool gameWon;
        private string consoleMargin = "  ";
        private List<string> previousGuesses;

        public GameHandler()
        {
            gameSessionService = new GameSessionService();
            previousGuesses = new List<string>();
        }

        /// <summary>
        /// Handles the high level execution of the game
        /// </summary>
        public void RunGame()
        {
            ShowBootupScreen();

            answer = GenerateSecretAnswer();

            //Left commented in here for ease of QA testing
            //WriteThemeCaution($"Answer: {string.Join("", answer)}");

            do
            {
              RunGameRound();
            } while (!gameWon && guessesMade < 10);

            if (!gameWon)
            {
                WriteThemeWarning($"\n\nSorry you lost. The Answer is {string.Join("", answer)}");
            }

            AddGameDataToSessionData();
            DisplayGameSessionStats();

            var userNeedsToMakeSelection = true;

            do
            {
                WriteLineWithMargin("Would you Like to Play Again? (Y/N)");
                var input = Console.ReadLine()?.Trim();
                if (input?.ToUpperInvariant() == "Y")
                {
                    userNeedsToMakeSelection = false;
                }
                else if (input?.ToUpperInvariant() == "N")
                {
                    WriteThemeAttention("Thanks for Playing!");
                    Thread.Sleep(2000);
                    Environment.Exit(0);
                }
                else
                {
                    WriteThemeWarning("Please enter a valid input (Y/N) i.e. Y by itself.");
                }
            } while (userNeedsToMakeSelection);

            ResetGame();
            RunGame();
        }

        /// <summary>
        /// Validates the player's input
        /// </summary>
        /// <param name="input">the console input from the player</param>
        /// <returns>true or false if the input is valid</returns>
        public bool InputIsValid(string input)
        {
            int numberGuess;
            var answerIsInt = int.TryParse(input, out numberGuess);

            return answerIsInt && input?.Length == 4;
        }

        /// <summary>
        /// Handles the generation of the + - hint for each guess
        /// </summary>
        /// <param name="answer">the answer the player is guessing for</param>
        /// <param name="currentGuess">the most recent guess that the hint is being generated for</param>
        /// <returns>the hint with +'s first and -'s second</returns>
        public string GenerateHint(int[] answer, string currentGuess)
        {
            var hint = "";
            var currentGuessDigits = currentGuess.Select(n => Convert.ToInt32(n) - 48).ToArray();
            var guessDigitAssessed = new bool[] { false, false, false, false };
            var answerDigitAssessed = new bool[] { false, false, false, false };

            for (int i = 0; i < 4; i++)
            {
                if (answer[i] == currentGuessDigits[i])
                {
                    hint += "+";
                    guessDigitAssessed[i] = true;
                    answerDigitAssessed[i] = true;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    if (!guessDigitAssessed[i] && !answerDigitAssessed[j] && answer[j] == currentGuessDigits[i])
                    {
                        hint += "-";
                        guessDigitAssessed[i] = true;
                        break;
                    }
                }
            }

            return hint;
        }

        /// <summary>
        /// Displays the initial text to the player upon bootup
        /// </summary>
        private void ShowBootupScreen()
        {
            WriteLineWithMargin("Mastermind\n");


            var instructions = $@"{consoleMargin}Welcome to the game Mastermind!
                                
{consoleMargin}4 digits (0-9) will be randomly generated and you will be given 10 chances to guess the correct 4 digits in the correct order. 
  
{consoleMargin}After each guess you will get a hint as to how accurate your guess is. You will see the following:
{consoleMargin}{consoleMargin}(-) minus sign for every digit that is correct but in the wrong position
{consoleMargin}{consoleMargin}(+) plus sign for every digit that is both correct and in the correct position
  
{consoleMargin}Note that the plus signs will be printed first, and minus signs second (i.e. the first (+) plus sign doesn't necessarily reflect the first digit).

{consoleMargin}Example: 
{consoleMargin}{consoleMargin}If the secret answer is: 1234
{consoleMargin}{consoleMargin}And the guess is: 4233
{consoleMargin}{consoleMargin}The hint returned will be: ++-

{consoleMargin}If the answer has a repeating digit (ex. 2 in 7232) and your guess contains one instance of that repeating digit in the wrong position, you will receive only (-) minus
{consoleMargin}Example: 
{consoleMargin}{consoleMargin}If the secret answer is: 7232
{consoleMargin}{consoleMargin}And the guess is: 2111
{consoleMargin}{consoleMargin}The hint returned will be: -

{consoleMargin}Please enter your 4 digit guess.";

            Console.WriteLine(instructions);
        }

        /// <summary>
        /// Generate the answer that the player is guessing for
        /// </summary>
        /// <returns>the secret answer</returns>
        private int[] GenerateSecretAnswer()
        {
            var random = new Random();
            return [random.Next(0, 9), random.Next(0, 9), random.Next(0, 9), random.Next(0, 9)];
        }

        /// <summary>
        /// Handles the execution of one game round
        /// </summary>
        private void RunGameRound()
        {
            var guessesRemaining = 10 - guessesMade;
            var guessIsValid = false;

            Console.WriteLine("\n");

            if(guessesRemaining < 4)
            {
                WriteThemeWarning($"Guesses Remaining: {guessesRemaining}");
            } else if (guessesRemaining < 6)
            {
                WriteThemeCaution($"Guesses Remaining: {guessesRemaining}");
            } else
            {
                WriteLineWithMargin($"Guesses Remaining: {guessesRemaining}");
            }

            do
            {
                var currentGuess = Console.ReadLine()?.Trim();

                if (currentGuess != null && InputIsValid(currentGuess))
                {
                    if (!GuessAlreadyMadeInCurrentGame(currentGuess))
                    {
                        guessIsValid = true;
                        previousGuesses.Add(currentGuess);
                        var guessMatchesAnswer = currentGuess == string.Join("", answer);
                        if (guessMatchesAnswer)
                        {
                            WriteThemeAttention("You Win!!! Congratulations!!");
                            gameWon = true;
                        }
                        else
                        {
                            var hint = GenerateHint(answer, currentGuess);
                            WriteLineWithMargin($"Hint: {hint}");
                        }
                    } else
                    {
                        WriteThemeWarning("You've already made that guess! Try another.");
                    }
                }
                else
                {
                    WriteThemeWarning("Please enter a valid guess in the form of 4 digits (i.e. 4815)");
                }

            } while (!guessIsValid);

            guessesMade++;
        }

        /// <summary>
        /// Determines if the player has already made this guess in game to ensure they don't accidentally waste a guess
        /// </summary>
        /// <param name="currentGuess">the current guess to check against previous guesses</param>
        /// <returns>true if the player has made the current guess previously this game</returns>
        private bool GuessAlreadyMadeInCurrentGame(string currentGuess)
        {
            return previousGuesses.Contains(currentGuess);
        }

        /// <summary>
        /// On a replay, set the game data up again
        /// </summary>
        private void ResetGame()
        {
            guessesMade = 0;
            gameWon = false;
            previousGuesses = new List<string>();
        }

        /// <summary>
        /// sends game data to the cache
        /// </summary>
        private void AddGameDataToSessionData()
        {
            gameSessionService.AddGameDataToSession(gameWon, guessesMade);
        }

        /// <summary>
        /// Displays all the game session stats to the console
        /// </summary>
        private void DisplayGameSessionStats()
        {
            var stats = gameSessionService.GetGameSessionStats();
            Console.WriteLine("\n");
            WriteThemeAttentionSecondary($"*******Game Session Stats*******");
            WriteLineWithMargin($"Games Played: {stats.GamesData.Count}");
            WriteLineWithMargin($"Games Won: {stats.GamesWon}");
            WriteLineWithMargin($"Average Guesses Made on Winning Games: {stats.AverageNumberOfGuesses}");
            WriteThemeAttentionSecondary($"********************************\n\n");
        }

        #region ConsoleStyles

        /// <summary>
        /// Meant to draw player's attention to a warning
        /// </summary>
        /// <param name="text"></param>
        private void WriteThemeWarning(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            //Note: For colorblind players you want to ensure you aren't just conveying meaning through color so I'm adding symbols that maybe aren't as typical in console apps as icons are on the web
            WriteLineWithMargin($"(!!!){text}");
            Console.ResetColor();
        }

        /// <summary>
        /// Meant to draw player's attention. Typically for something that could become a warning.
        /// </summary>
        /// <param name="text"></param>
        private void WriteThemeCaution(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            WriteLineWithMargin($"(!){text}");
            Console.ResetColor();
        }

        /// <summary>
        /// Meant to draw player's attention, generally a positive connotation.
        /// </summary>
        /// <param name="text"></param>
        private void WriteThemeAttention(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            WriteLineWithMargin($"(*){text}");
            Console.ResetColor();
        }

        /// <summary>
        /// Meant to draw player's attention, generally a positive connotation.
        /// </summary>
        /// <param name="text"></param>
        private void WriteThemeAttentionSecondary(string text)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            WriteLineWithMargin($"(*){text}");
            Console.ResetColor();
        }

        /// <summary>
        /// Helps standardizing margins and allows quick changes to them.
        /// </summary>
        /// <param name="text"></param>
        private void WriteLineWithMargin(string text)
        {
            Console.WriteLine($"{consoleMargin}{text}");
        }

        #endregion ConsoleStyles
    }
}
