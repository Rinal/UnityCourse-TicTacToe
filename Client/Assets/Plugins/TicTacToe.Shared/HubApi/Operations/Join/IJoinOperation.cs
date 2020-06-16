using System.Threading.Tasks;

namespace TicTacToe.Shared
{
    public interface IJoinOperation
    {
        Task<string> Join(string json);
    }
}