using System;
using System.Collections.Generic;
using TicTacToe.Shared;

namespace TicTacToe.Server
{
    public interface IAnalysis
    {
        bool WinnerDefiner(CellModel[,] field, out IEnumerable<ValueTuple<int, int>> winPositions);
    }
}