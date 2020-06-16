using Newtonsoft.Json;

namespace TicTacToe.Shared
{
    public abstract class BaseResponse
    {
        [JsonProperty]
        public string Error { get; private set; }

        protected BaseResponse(string error)
        {
            Error = error;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}