using Newtonsoft.Json;

namespace TicTacToe.Shared
{
    public sealed class FieldChangedEvent
    {
        [JsonProperty]
        public SymbolModel Symbol { get; set; }
        [JsonProperty]
        public int X { get; set; }
        [JsonProperty]
        public int Y { get; set; }

        [JsonConstructor]
        private FieldChangedEvent()
        {
        }

        public FieldChangedEvent(SymbolModel symbol, int x, int y)
        {
            Symbol = symbol;
            X = x;
            Y = y;
        }
    }
}