using Newtonsoft.Json;

namespace TicTacToe.Shared
{
    public sealed class CellModel
    {
        [JsonProperty]
        public Symbols Symbol { get; private set; }

        public CellModel(Symbols symbol)
        {
            Symbol = symbol;
        }
    }
}