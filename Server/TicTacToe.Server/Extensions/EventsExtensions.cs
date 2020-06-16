using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TicTacToe.Shared;

namespace TicTacToe.Server
{
    public static class EventsExtensions
    {
        public static string ToEvent(this IEnumerable<UserModel> users)
        {
            JObject delta = new JObject();
            delta[nameof(UserModel)] = JsonConvert.SerializeObject(users);
            return delta.ToString();
        }

        public static string ToEvent(this FieldChangedEvent fieldChangedEvent)
        {
            JObject delta = new JObject();
            delta[nameof(FieldChangedEvent)] = JsonConvert.SerializeObject(fieldChangedEvent);
            return delta.ToString();
        }
    }
}