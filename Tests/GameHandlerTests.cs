using Mastermind;
using Xunit;

namespace Tests
{
    public class GameHandlerTests
    {
        public GameHandlerTests()
        {

        }

        [Theory]
        [InlineData("4815")]
        [InlineData("0987")]
        public void InputIsValid_PassValidInputs(string input)
        {
            var gameHandler = new GameHandler();
            Assert.True(gameHandler.InputIsValid(input));
        }

        [Theory]
        [InlineData("mastermind")]
        [InlineData("123a")]
        [InlineData("")]
        [InlineData("123")]
        [InlineData("12345")]
        public void InputIsValid_FailInvalidInputs(string input)
        {
            var gameHandler = new GameHandler();
            Assert.False(gameHandler.InputIsValid(input));
        }

        [Theory]
        [InlineData(new int[]{1, 2, 3, 4}, "0000")]
        [InlineData(new int[] { 7, 7, 7, 7 }, "1234")]
        [InlineData(new int[] { 9,5,3,6 }, "2211")]
        public void GenerateHint_NoHits(int[] answer, string input)
        {
            var gameHandler = new GameHandler();
            Assert.Equal("", gameHandler.GenerateHint(answer, input));
        }

        [Theory]
        [InlineData(new int[] { 1, 2, 3, 4 }, "9874")]
        [InlineData(new int[] { 0, 0, 0, 0 }, "8407")]
        [InlineData(new int[] { 1, 1, 5, 5 }, "1223")]
        public void GenerateHint_OnePlus(int[] answer, string input)
        {
            var gameHandler = new GameHandler();
            Assert.Equal("+", gameHandler.GenerateHint(answer, input));
        }

        [Theory]
        [InlineData(new int[] { 1, 2, 3, 4 }, "1267")]
        [InlineData(new int[] { 0, 0, 0, 0 }, "4007")]
        [InlineData(new int[] { 7, 2, 7, 2 }, "3472")]
        public void GenerateHint_TwoPluses(int[] answer, string input)
        {
            var gameHandler = new GameHandler();
            Assert.Equal("++", gameHandler.GenerateHint(answer, input));
        }

        [Theory]
        [InlineData(new int[] { 1, 2, 3, 4 }, "7771")]
        [InlineData(new int[] { 0, 0, 1, 0 }, "4507")]
        [InlineData(new int[] { 2, 3, 4, 2 }, "0278")]
        [InlineData(new int[] { 7, 2, 3, 2 }, "2111")]
        public void GenerateHint_OneMinus(int[] answer, string input)
        {
            var gameHandler = new GameHandler();
            Assert.Equal("-", gameHandler.GenerateHint(answer, input));
        }

        [Theory]
        [InlineData(new int[] { 1, 2, 3, 4 }, "4321")]
        [InlineData(new int[] { 0, 1, 1, 0 }, "1001")]
        [InlineData(new int[] { 5, 8, 3, 1 }, "3518")]
        public void GenerateHint_FourMinuses(int[] answer, string input)
        {
            var gameHandler = new GameHandler();
            Assert.Equal("----", gameHandler.GenerateHint(answer, input));
        }

        [Theory]
        [InlineData(new int[] { 1, 2, 3, 4 }, "1243")]
        [InlineData(new int[] { 4, 8, 1, 5 }, "5814")]
        [InlineData(new int[] { 4, 8, 1, 5 }, "4185")]
        public void GenerateHint_TwoPlusesTwoMinuses(int[] answer, string input)
        {
            var gameHandler = new GameHandler();
            Assert.Equal("++--", gameHandler.GenerateHint(answer, input));
        }

        [Theory]
        [InlineData(new int[] { 1, 2, 3, 4 }, "2994")]
        [InlineData(new int[] { 1, 6, 2, 3 }, "0021")]
        [InlineData(new int[] { 4, 8, 1, 5 }, "5117")]
        public void GenerateHint_OnePlusOneMinus(int[] answer, string input)
        {
            var gameHandler = new GameHandler();
            Assert.Equal("+-", gameHandler.GenerateHint(answer, input));
        }

        [Theory]
        [InlineData(new int[] { 1, 2, 3, 4 }, "4233")]
        [InlineData(new int[] { 1, 5, 1, 6 }, "0116")]
        public void GenerateHint_TwoPlusesOneMinus(int[] answer, string input)
        {
            var gameHandler = new GameHandler();
            Assert.Equal("++-", gameHandler.GenerateHint(answer, input));
        }
    }
}
