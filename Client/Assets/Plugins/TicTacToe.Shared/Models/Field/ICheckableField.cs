namespace TicTacToe.Shared
{
    public interface ICheckableField
    {
        /// <summary>
        /// Return true if the cell at this coordinates is empty
        /// </summary>
        bool IsEmpty(int x, int y);
    }
}