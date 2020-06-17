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

        public static string ToEvent(this ActiveUserChangedEvent activeUserChangedEvent)
        {
            JObject delta = new JObject();
            delta[nameof(ActiveUserChangedEvent)] = JsonConvert.SerializeObject(activeUserChangedEvent);
            return delta.ToString();
        }

        public static string ToEvent(this GameOverEvent gameOverEvent)
        {
            JObject delta = new JObject();
            delta[nameof(GameOverEvent)] = JsonConvert.SerializeObject(gameOverEvent);
            return delta.ToString();
        }
    }
}