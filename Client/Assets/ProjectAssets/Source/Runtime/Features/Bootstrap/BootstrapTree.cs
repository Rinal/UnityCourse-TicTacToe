using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace TicTacToe.Client
{
    public sealed class BootstrapTree : MonoBehaviour
    {
        private IHubConnection m_hubConnection = default;

        [Inject]
        private void Construct(IHubConnection hubConnection)
        {
            m_hubConnection = hubConnection;
        }

        private void Start()
        {
            Debug.Log($"{nameof(BootstrapTree)}: going to start hub connection...");
            m_hubConnection
                .Start()
                .Catch<Unit, Exception>(ex =>
                {
                    Debug.LogException(ex);
                    return Observable.Never<Unit>();
                })
                .Subscribe(_ => { Debug.Log($"{nameof(BootstrapTree)}: the connection was open!"); });
        }
    }
}