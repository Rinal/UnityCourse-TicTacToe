using System;
using TicTacToe.Shared;

namespace TicTacToe.Server
{
    public sealed class ServerFieldModel :
        ICheckableField,
        ISettableField,
        IFieldElements
    {
        private readonly CellModel[,] m_field = new CellModel[FieldSize.Value, FieldSize.Value];
        CellModel[,] IFieldElements.Field => m_field;

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
}