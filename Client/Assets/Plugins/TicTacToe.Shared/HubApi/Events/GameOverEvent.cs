using Newtonsoft.Json;

namespace TicTacToe.Shared
{
    public sealed class GameOverEvent
    {
        [JsonProperty]
        public Symbols? Symbol { get; private set; }

        public GameOverEvent(Symbols? symbol)
        {
            Symbol = symbol;
        }
    }
}