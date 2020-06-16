using System.Text;
using Newtonsoft.Json;
using TicTacToe.Shared;

namespace TicTacToe.Server
{
    public sealed class UserModel
    {
        [JsonProperty]
        public string Id { get; private set; }

        [JsonProperty]
        public string Name { get; private set; }

        [JsonProperty]
        public Symbols Symbol { get; private set; }

        public UserModel(string id, string name, Symbols symbol)
        {
            Id = id;
            Name = name;
            Symbol = symbol;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"{nameof(UserModel)}: {nameof(Id)}={Id}");
            builder.AppendLine($"{nameof(Name)}={Name}");
            builder.AppendLine($"{nameof(Symbol)}={nameof(Symbol)}");
            return builder.ToString();
        }
    }
}