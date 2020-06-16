using UniRx;

namespace TicTacToe.Client
{
    public interface IActiveUserObservable
    {
        /// <summary>
        /// Id of the user that is currently active (can select field)
        /// </summary>
        IReadOnlyReactiveProperty<string> Id { get; }
    }
}