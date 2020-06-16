using System;
using TicTacToe.Shared;
using UniRx;
using UnityEngine;
using Zenject;

namespace TicTacToe.Client
{
    public sealed class ClientFieldState : IInitializable, IDisposable
    {
        private readonly ReactiveDictionary<Vector2Int, CellModel> m_field = new ReactiveDictionary<Vector2Int, CellModel>();

        public IReadOnlyReactiveDictionary<Vector2Int, CellModel> Field => m_field;

        void IInitializable.Initialize()
        {
            for (int x = 0; x < FieldSize.Value; x++)
            {
                for (int y = 0; y < FieldSize.Value; y++)
                {
                    m_field.Add(new Vector2Int(x, y), null);
                }
            }
        }

        void IDisposable.Dispose()
        {
        }
    }
}