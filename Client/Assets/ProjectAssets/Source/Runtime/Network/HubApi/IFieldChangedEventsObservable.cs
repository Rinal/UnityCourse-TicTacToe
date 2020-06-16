using System;
using Newtonsoft.Json.Linq;

namespace TicTacToe.Client
{
    public interface IFieldChangedEventsObservable
    {
        IObservable<JObject> OnStateChanged();
    }
}