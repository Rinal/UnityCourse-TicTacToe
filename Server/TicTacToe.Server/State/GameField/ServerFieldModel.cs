using System;
using TicTacToe.Shared;

namespace TicTacToe.Server
{
    public sealed class ServerFieldModel :
        ICheckableField
        , ISettableField
    {
        private readonly CellModel[,] m_field = new CellModel[FieldSize.Value, FieldSize.Value];

        private bool IsValid(int x, int y)
        {
            return x >= 0 && x < FieldSize.Value && y >= 0 && y < FieldSize.Value;
        }

        bool ICheckableField.IsEmpty(int x, int y)
        {
            if (!IsValid(x, y))
            {
                throw new Exception("Invalid field coordinates!");
            }

            return m_field[x, y] == null;
        }

        void ISettableField.Set(Symbols symbol, int x, int y)
        {
            if (!IsValid(x, y))
            {
                throw new Exception("Invalid field coordinates!");
            }

            m_field[x, y] = new CellModel(symbol);
        }
    }

    public sealed class GameFieldAnalysis
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

        public bool WinnerDefiner(CellModel[,] field, out Symbols? symbol)
        {
            symbol = null;
            for (int y = 0; y < FieldSize.Value; y++)
            {
                for (int x = 0; x < FieldSize.Value; x++)
                {
                    foreach (Direction direction in m_winDirections)
                    {
                        if (x + (direction.X * WinLength) <= FieldSize.Value &&
                            x + (direction.X * WinLength) >= 0 &&
                            y + (direction.Y * WinLength) <= FieldSize.Value &&
                            y + (direction.Y * WinLength) >= 0)
                        {
                            CellModel lastCell = field[y, x];
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
                                    }
                                }
                            }

                            if (lastCell != null)
                            {
                                symbol = lastCell.Symbol;
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}