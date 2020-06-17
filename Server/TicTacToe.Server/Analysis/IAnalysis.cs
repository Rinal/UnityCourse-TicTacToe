using TicTacToe.Shared;

namespace TicTacToe.Server
{
    public interface IAnalysis
    {
        bool WinnerDefiner(CellModel[,] field, out Symbols? symbol);
    }
}