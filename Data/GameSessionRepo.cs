using Data.Models;

namespace Data
{
    /// <summary>
    /// Stores the stats for the current game session
    /// </summary>
    public class GameSessionRepo
    {
        //Note: Probably overkill for this small scale one-off console game application, but wanted to illustrate how the data would typically be handled in it's own project
        //Stretch goal would be to write to a file to store multiple session data and convert to async functions
        private List<SingleGameData> sessionData = [];

        /// <summary>
        /// Get the current console session's game data
        /// </summary>
        /// <returns>A list of the game data</returns>
        public List<SingleGameData> GetSessionData()
        {
            return sessionData;
        }

        /// <summary>
        /// Add a singular game's data to the session cache
        /// </summary>
        /// <param name="gameData">data about a singular game</param>
        public void AddGameDataToSession(SingleGameData gameData)
        {
            sessionData.Add(gameData);
        }
    }
}
