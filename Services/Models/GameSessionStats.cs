using Data.Models;

namespace Services.Models
{
    public class GameSessionStats
    {
        public List<SingleGameData> GamesData { get; set; } = new List<SingleGameData>();

        /// <summary>
        /// Average number of guesses on winning games
        /// </summary>
        public double AverageNumberOfGuesses { get; set; }
        public int GamesWon { get; set; }
    }
}
