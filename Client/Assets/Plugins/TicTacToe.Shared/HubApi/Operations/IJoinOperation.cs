using System.Threading.Tasks;

namespace TicTacToe.Shared
{
    public interface IJoinOperation
    {
        Task Join(string json);
    }
}