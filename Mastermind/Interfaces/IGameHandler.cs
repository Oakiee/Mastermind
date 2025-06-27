
namespace Mastermind.Interfaces
{
    public interface IGameHandler
    {
        void RunGame();
        string GenerateHint(int[] answer, string currentGuess);
        bool InputIsValid(string input);
    }
}
