using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
    }
}