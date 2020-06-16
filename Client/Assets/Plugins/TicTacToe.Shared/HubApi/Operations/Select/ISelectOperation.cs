using System.Threading.Tasks;

namespace TicTacToe.Shared
{
    public interface ISelectOperation
    {
        Task<string> Select(string json);
    }
}