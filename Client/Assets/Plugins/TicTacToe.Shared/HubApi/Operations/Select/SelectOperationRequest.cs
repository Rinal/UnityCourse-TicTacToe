using Newtonsoft.Json;

namespace TicTacToe.Shared
{
    public sealed class SelectOperationRequest : BaseRequest
    {
        [JsonProperty]
        public int X { get; private set; }
        [JsonProperty]
        public int Y { get; private set; }

        public SelectOperationRequest(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"{nameof(SelectOperationRequest)}: X={X} Y={Y}";
        }
    }
}