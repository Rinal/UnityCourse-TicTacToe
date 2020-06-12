using Newtonsoft.Json;

namespace TicTacToe.Shared
{
    public sealed class SymbolModel
    {
        [JsonProperty]
        public Symbols Symbol { get; private set; }

        public SymbolModel(Symbols symbol)
        {
            Symbol = symbol;
        }
    }
}