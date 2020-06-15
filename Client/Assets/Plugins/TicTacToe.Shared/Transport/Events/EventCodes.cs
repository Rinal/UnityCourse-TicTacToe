using System.Collections.Generic;

namespace TicTacToe.Shared
{
    public static class EventCodes
    {
        private static readonly List<string> EventNames = new List<string>
        {
            nameof(FieldChangedEvent)
        };

        public static int ToCode(string name)
        {
            //TODO Check for exist!
            return EventNames.IndexOf(name);
        }
    }
}