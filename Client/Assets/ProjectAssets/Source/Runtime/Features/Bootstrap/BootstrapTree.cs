using System;
using Newtonsoft.Json;
using TicTacToe.Shared;
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
            StartConnectionAction()
                .ContinueWith(_ =>
                {
                    Debug.Log($"{nameof(BootstrapTree)}: the connection was open!");
                    return Observable.ReturnUnit();
                })
                .ContinueWith(JoinAction())
                .Subscribe();
            m_fieldChangedEventsObservable
                .OnStateChanged()
                .Subscribe(_ => { Debug.LogError($"STATE CHANGED {_.ToString()}"); });
        }

        [Inject] private IFieldChangedEventsObservable m_fieldChangedEventsObservable = default;
        [Inject] private IJoinOperation m_joinOperation = default;

        private IObservable<Unit> StartConnectionAction()
        {
            return m_hubConnection
                .Start()
                .Catch<Unit, Exception>(ex =>
                {
                    Debug.LogException(ex);
                    return Observable.Never(Unit.Default);
                });
        }

        private IObservable<Unit> JoinAction()
        {
            return m_joinOperation
                .Join(new JoinOperationRequest("Stas").ToJson())
                .ToObservable()
                .Catch<string, Exception>(ex =>
                {
                    Debug.LogException(ex);
                    return Observable.Never<string>();
                })
                .ContinueWith(json =>
                {
                    JoinOperationResponse response = JsonConvert.DeserializeObject<JoinOperationResponse>(json);
                    if (string.IsNullOrEmpty(response.Error))
                    {
                        Debug.Log($"{nameof(BootstrapTree)}: User joined to the room!");
                        return Observable.ReturnUnit();
                    }

                    Debug.LogError($"{nameof(BootstrapTree)}: join failed {response.Error}.");
                    return Observable.Never(Unit.Default);
                });
        }

        [ContextMenu("Join")]
        private void Test1Operation()
        {
            JoinAction().Subscribe();
        }

        [Inject] private ISelectOperation m_selectOperation = default;

        [ContextMenu("Select")]
        private void SelectOperation()
        {
            m_selectOperation
                .Select(new SelectOperationRequest(1, 2).ToJson())
                .ToObservable()
                .Catch<string, Exception>(ex =>
                {
                    Debug.LogException(ex);
                    return Observable.Never<string>();
                })
                .Subscribe(jsonResponse =>
                {
                    SelectOperationResponse response = JsonConvert.DeserializeObject<SelectOperationResponse>(jsonResponse);
                    Debug.LogError(" Response ! " + response.Error);
                });
        }
    }
}