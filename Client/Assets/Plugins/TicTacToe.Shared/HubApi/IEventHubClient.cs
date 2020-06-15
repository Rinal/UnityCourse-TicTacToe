using System.Threading.Tasks;

namespace TicTacToe.Shared
{
    public interface IEventHubClient
    {
        Task OnEvent(int code, string json);
    }
}