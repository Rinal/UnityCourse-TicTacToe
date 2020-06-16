using System.Threading.Tasks;

namespace TicTacToe.Shared
{
    public interface IStateChangedListener
    {
        Task OnStateChanged(string json);
    }
}