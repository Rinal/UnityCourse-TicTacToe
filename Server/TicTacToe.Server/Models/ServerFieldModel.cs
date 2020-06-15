using System;
using TicTacToe.Shared;

namespace TicTacToe.Server
{
    public sealed class ServerFieldModel
    {
        private readonly SymbolModel[,] m_field = new SymbolModel[FieldSize.Value, FieldSize.Value];

        public bool IsEmpty(int x, int y)
        {
            if (!IsValid(x, y))
            {
                throw new Exception("Invalid field coordinates!");
            }

            return m_field[x, y] == null;
        }

        private bool IsValid(int x, int y)
        {
            return x >= 0 && x < FieldSize.Value && y >= 0 && y < FieldSize.Value;
        }
    }
}