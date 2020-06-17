using System;
using System.Collections;
using System.Collections.Generic;
using TicTacToe.Shared;

namespace TicTacToe.Server
{
    public sealed class GameFieldAnalysis : IAnalysis
    {
        private struct Direction
        {
            public readonly int X;
            public readonly int Y;

            public Direction(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        private readonly Direction[] m_winDirections = new[]
        {
            new Direction(1, 0), // horizontal
            new Direction(0, 1), // vertical
            new Direction(1, 1), //  diagonal
            new Direction(-1, -1) //  diagonal - reverting
        };

        private const uint WinLength = 3;

        bool IAnalysis.WinnerDefiner(CellModel[,] field, out IEnumerable<ValueTuple<int, int>> winPositions)
        {
            winPositions = null;
            List<ValueTuple<int, int>> potentialWinCells = new List<ValueTuple<int, int>>();
            bool draw = true;
            for (int y = 0; y < FieldSize.Value; y++)
            {
                for (int x = 0; x < FieldSize.Value; x++)
                {
                    if (draw && field[y, x] == null)
                    {
                        draw = false;
                    }
                    foreach (Direction direction in m_winDirections)
                    {
                        if (x + (direction.X * WinLength) <= FieldSize.Value &&
                            x + (direction.X * WinLength) >= 0 &&
                            y + (direction.Y * WinLength) <= FieldSize.Value &&
                            y + (direction.Y * WinLength) >= 0)
                        {
                            potentialWinCells.Clear();
                            CellModel lastCell = field[y, x];
                            potentialWinCells.Add((y, x));
                            if (lastCell != null)
                            {
                                for (int k = 1; k < WinLength; k++)
                                {
                                    CellModel cell = field[y + (direction.Y * k), x + (direction.X * k)];
                                    if (cell == null || cell.Symbol != lastCell.Symbol)
                                    {
                                        lastCell = null;
                                        break;
                                    }
                                    else
                                    {
                                        lastCell = cell;
                                        potentialWinCells.Add((y + (direction.Y * k), x + (direction.X * k)));
                                    }
                                }
                            }

                            if (lastCell != null)
                            {
                                winPositions = potentialWinCells;
                                return true;
                            }
                        }
                    }
                }
            }
            return draw;
        }
    }
}