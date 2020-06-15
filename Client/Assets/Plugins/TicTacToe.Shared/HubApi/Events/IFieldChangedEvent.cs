using System.Threading.Tasks;

namespace TicTacToe.Shared
{
    public interface IFieldChangedEvent
    {
        Task OnFieldChanged(string json);
    }
}