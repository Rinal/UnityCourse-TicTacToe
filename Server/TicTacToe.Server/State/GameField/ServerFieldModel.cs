using System;
using Microsoft.Extensions.Logging;
using TicTacToe.Shared;

namespace TicTacToe.Server
{
    public sealed class ServerFieldModel :
        ICheckableField
        , ISettableField
    {
        private readonly ILogger<ServerFieldModel> m_logger = default;
        private readonly CellModel[,] m_field = new CellModel[FieldSize.Value, FieldSize.Value];

        public ServerFieldModel(ILogger<ServerFieldModel> logger)
        {
            m_logger = logger;
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
            m_logger.LogInformation($"Filed was changed ({x},{y}) to {symbol}");
        }

        private bool IsValid(int x, int y)
        {
            return x >= 0 && x < FieldSize.Value && y >= 0 && y < FieldSize.Value;
        }
    }
}