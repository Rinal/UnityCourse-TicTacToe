namespace TicTacToe.Shared
{
    public interface ISettableField
    {
        void Set(SymbolModel model, int x, int y);
    }
}