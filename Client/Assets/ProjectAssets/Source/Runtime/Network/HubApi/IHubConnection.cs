using System;
using UniRx;

namespace TicTacToe.Client
{
    public interface IHubConnection
    {
        IObservable<Unit> Start();
        IObservable<Unit> Stop();
    }
}