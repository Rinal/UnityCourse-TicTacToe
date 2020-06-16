using Newtonsoft.Json;

namespace TicTacToe.Shared
{
    public sealed class ActiveUserChangedEvent
    {
        [JsonProperty]
        public string Id { get; private set; }

        public ActiveUserChangedEvent(string id)
        {
            Id = id;
        }
    }
}