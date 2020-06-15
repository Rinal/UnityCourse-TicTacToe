using Newtonsoft.Json;

namespace TicTacToe.Shared
{
    public sealed class FieldChangedEventData
    {
        [JsonProperty]
        public SymbolModel Symbol { get; set; }
        [JsonProperty]
        public int X { get; set; }
        [JsonProperty]
        public int Y { get; set; }

        [JsonConstructor]
        private FieldChangedEventData()
        {
        }

        public FieldChangedEventData(SymbolModel symbol, int x, int y)
        {
            Symbol = symbol;
            X = x;
            Y = y;
        }
    }
}