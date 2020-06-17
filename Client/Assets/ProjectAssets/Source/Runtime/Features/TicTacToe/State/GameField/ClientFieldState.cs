using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TicTacToe.Shared;
using UniRx;
using UnityEngine;
using Zenject;

namespace TicTacToe.Client
{
    public sealed class ClientFieldState :
        IInitializable
        , ICheckableField
        , ISettableField
        , IDisposable
    {
        private readonly ReactiveDictionary<Vector2Int, CellModel> m_field = new ReactiveDictionary<Vector2Int, CellModel>();
        private readonly IStateChangedObservable m_stateChangedObservable = default;
        private readonly ISelectOperation m_selectOperation = default;
        private IDisposable m_onStateChanged = default;

        public IReadOnlyReactiveDictionary<Vector2Int, CellModel> Field => m_field;

        [Inject]
        public ClientFieldState(
            IStateChangedObservable stateChangedObservable,
            ISelectOperation selectOperation)
        {
            m_stateChangedObservable = stateChangedObservable;
            m_selectOperation = selectOperation;
        }

        void IInitializable.Initialize()
        {
            for (int x = 0; x < FieldSize.Value; x++)
            {
                for (int y = 0; y < FieldSize.Value; y++)
                {
                    m_field.Add(new Vector2Int(x, y), null);
                }
            }

            m_onStateChanged = m_stateChangedObservable
                .OnStateChanged()
                .Subscribe(OnStateChanged);
        }

        private void OnStateChanged(JObject delta)
        {
            if (delta.TryGetValue(nameof(FieldChangedEvent), out JToken token))
            {
                Debug.Log($"{nameof(ClientFieldState)}: got filed changes {token}.");
                FieldChangedEvent changed = JsonConvert.DeserializeObject<FieldChangedEvent>(token.ToString());
                m_field[new Vector2Int(changed.X, changed.Y)] = new CellModel(changed.Symbol);
            }
            
            if (delta.TryGetValue(nameof(GameOverEvent), out  token))
            {
                Debug.Log($"{nameof(ClientFieldState)}: got game over event {token}.");
                GameOverEvent gameOver = JsonConvert.DeserializeObject<GameOverEvent>(token.ToString());
                Debug.LogError($"THE GAME WAS OVER {gameOver.Symbol}");
            }
            
        }

        void IDisposable.Dispose()
        {
            m_onStateChanged?.Dispose();
        }

        bool ICheckableField.IsEmpty(int x, int y)
        {
            return m_field[new Vector2Int(x, y)] == null;
        }

        void ISettableField.Set(Symbols symbol, int x, int y)
        {
            m_selectOperation
                .Select(new SelectOperationRequest(symbol, x, y).ToJson())
                .ToObservable()
                .Subscribe(json =>
                {
                    SelectOperationResponse response = JsonConvert.DeserializeObject<SelectOperationResponse>(json);
                    if (string.IsNullOrEmpty(response.Error))
                    {
                        Debug.Log($"{nameof(ClientFieldState)}: select completed!");
                    }
                    else
                    {
                        Debug.LogError($"{nameof(ClientFieldState)}: select failed with {response.Error}!");
                    }
                });
        }
    }
}