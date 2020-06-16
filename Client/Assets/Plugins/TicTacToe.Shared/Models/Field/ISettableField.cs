namespace TicTacToe.Shared
{
    public interface ISettableField
    {
        void Set(CellModel model, int x, int y);
    }
}