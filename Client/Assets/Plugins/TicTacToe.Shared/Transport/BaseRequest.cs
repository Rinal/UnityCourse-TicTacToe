using Newtonsoft.Json;

namespace TicTacToe.Shared
{
    public abstract class BaseRequest
    {
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}