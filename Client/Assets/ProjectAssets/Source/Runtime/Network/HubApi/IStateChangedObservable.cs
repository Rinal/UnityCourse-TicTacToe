using System;
using Newtonsoft.Json.Linq;

namespace TicTacToe.Client
{
    public interface IStateChangedObservable
    {
        IObservable<JObject> OnStateChanged();
    }
}