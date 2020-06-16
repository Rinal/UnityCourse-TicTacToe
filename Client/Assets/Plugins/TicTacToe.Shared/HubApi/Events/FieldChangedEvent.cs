using Newtonsoft.Json;

namespace TicTacToe.Shared
{
    public sealed class FieldChangedEvent
    {
        [JsonProperty]
        public Symbols Symbol { get; set; }
        [JsonProperty]
        public int X { get; set; }
        [JsonProperty]
        public int Y { get; set; }

        [JsonConstructor]
        private FieldChangedEvent()
        {
        }

        public FieldChangedEvent(Symbols symbol, int x, int y)
        {
            Symbol = symbol;
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"{nameof(FieldChangedEvent)}: {Symbol} ({X},{Y})";
        }
    }
}