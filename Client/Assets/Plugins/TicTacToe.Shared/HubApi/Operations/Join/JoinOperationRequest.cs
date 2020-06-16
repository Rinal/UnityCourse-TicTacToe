using Newtonsoft.Json;

namespace TicTacToe.Shared
{
    public sealed class JoinOperationRequest : BaseRequest
    {
        [JsonProperty]
        public string Name { get; private set; }

        public JoinOperationRequest(string name)
        {
            Name = name;
        }
    }
}