using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TicTacToe.Shared
{
    public sealed class GameOverEvent
    {
        [JsonProperty]
        public Symbols? Symbol { get; private set; }

        [JsonProperty]
        public IEnumerable<ValueTuple<int, int>> WinCellPositions { get; private set; }


        public GameOverEvent()
        {
            //Draw
        }
        public GameOverEvent(Symbols symbol, IEnumerable<ValueTuple<int, int>> winCellPositions)
        {
            Symbol = symbol;
        }
    }
}