using TicTacToe.Shared;
using UniRx;
using UnityEngine;

namespace TicTacToe.Client
{
    public sealed class ClientFieldModel
    {
        private readonly ReactiveDictionary<Vector2Int, SymbolModel> m_field = new ReactiveDictionary<Vector2Int, SymbolModel>();

        public IReadOnlyReactiveDictionary<Vector2Int, SymbolModel> Field => m_field;
    }
}