namespace TicTacToe.Server
{
    public interface IActiveUserState
    {
        /// <summary>
        /// Currently active user id 
        /// </summary>
        string Current { get; }

        void Set(string id);
    }
}