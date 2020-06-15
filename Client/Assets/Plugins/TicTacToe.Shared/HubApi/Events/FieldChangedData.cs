using Newtonsoft.Json;

namespace TicTacToe.Shared
{
    public sealed class FieldChangedData
    {
        [JsonProperty]
        public SymbolModel Symbol { get; set; }
        [JsonProperty]
        public int X { get; set; }
        [JsonProperty]
        public int Y { get; set; }

        [JsonConstructor]
        private FieldChangedData()
        {
        }

        public FieldChangedData(SymbolModel symbol, int x, int y)
        {
            Symbol = symbol;
            X = x;
            Y = y;
        }
    }
}