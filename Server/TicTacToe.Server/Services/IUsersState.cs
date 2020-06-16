namespace TicTacToe.Server
{
    public interface IUsersState
    {
        UserModel this[string id] { get; }
        void Add(UserModel user);
        void Remove(string id);
    }
}