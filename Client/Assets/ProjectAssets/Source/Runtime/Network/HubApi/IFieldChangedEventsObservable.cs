using System;
using TicTacToe.Shared;

namespace TicTacToe.Client
{
    public interface IFieldChangedEventsObservable
    {
        IObservable<FieldChangedData> OnFieldChanged();
    }
}