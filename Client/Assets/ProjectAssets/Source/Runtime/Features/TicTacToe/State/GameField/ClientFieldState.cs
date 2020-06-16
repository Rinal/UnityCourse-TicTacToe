using TicTacToe.Shared;
using UniRx;
using UnityEngine;

namespace TicTacToe.Client
{
    public sealed class ClientFieldState
    {
        private readonly ReactiveDictionary<Vector2Int, CellModel> m_field = new ReactiveDictionary<Vector2Int, CellModel>();

        public IReadOnlyReactiveDictionary<Vector2Int, CellModel> Field => m_field;
    }
}