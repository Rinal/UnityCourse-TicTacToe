using Newtonsoft.Json;

namespace TicTacToe.Shared
{
    public sealed class SelectOperationRequest : BaseRequest
    {
        [JsonProperty]
        public Symbols Symbol { get; private set; }
        [JsonProperty]
        public int X { get; private set; }
        [JsonProperty]
        public int Y { get; private set; }

        public SelectOperationRequest(Symbols symbol, int x, int y)
        {
            Symbol = symbol;
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"{nameof(SelectOperationRequest)}: X={X} Y={Y}";
        }
    }
}