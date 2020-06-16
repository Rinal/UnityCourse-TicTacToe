namespace TicTacToe.Shared
{
    public interface ISettableField
    {
        void Set(Symbols symbol, int x, int y);
    }
}