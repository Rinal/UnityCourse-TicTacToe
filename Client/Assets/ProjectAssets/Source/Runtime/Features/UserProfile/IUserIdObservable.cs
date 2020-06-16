using UniRx;

namespace TicTacToe.Client
{
    public interface IUserIdObservable
    {
        IReadOnlyReactiveProperty<string> Id { get; }
    }
}