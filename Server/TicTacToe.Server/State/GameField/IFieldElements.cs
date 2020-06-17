using TicTacToe.Shared;

namespace TicTacToe.Server
{
    public interface IFieldElements
    {
        CellModel[,] Field { get; }
    }
}