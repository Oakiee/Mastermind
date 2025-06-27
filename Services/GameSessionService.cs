using Data;
using Data.Models;
using Services.Models;

namespace Services
{
    /// <summary>
    /// The service connected to the Game Session Repo that will perform any relevant business logic
    /// </summary>
    public class GameSessionService
    {
        private readonly GameSessionRepo repo;

        public GameSessionService()
        {
            repo = new GameSessionRepo();
        }

        /// <summary>
        /// Gets the game session data as well as calculates any additional stats
        /// </summary>
        /// <returns>game session stats</returns>
        public GameSessionStats GetGameSessionStats()
        {
            var sessionData = repo.GetSessionData();
            var wonGames = sessionData.Where(g => g.GameWon);

            return new GameSessionStats()
            {
                GamesData = sessionData,
                AverageNumberOfGuesses = (wonGames != null && wonGames.Count() > 0) ? wonGames.Average(g => g.GuessesMade) : 0,
                GamesWon = wonGames != null ? wonGames.Count(d => d.GameWon == true) : 0
            };
        }

        /// <summary>
        /// Sends singular game data to repo to be written to the session cache
        /// </summary>
        /// <param name="data">data about a single game</param>
        public void AddGameDataToSession(bool gamesWon, int guessesMade)
        {
            repo.AddGameDataToSession(new SingleGameData(){GameWon = gamesWon, GuessesMade = guessesMade});
        }
    }
}
