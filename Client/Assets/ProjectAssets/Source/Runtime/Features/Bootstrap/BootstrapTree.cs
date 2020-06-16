using System;
using Newtonsoft.Json;
using TicTacToe.Shared;
using UniRx;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Random = UnityEngine.Random;

namespace TicTacToe.Client
{
    public sealed class BootstrapTree : MonoBehaviour
    {
        private void Start()
        {
            Debug.Log($"{nameof(BootstrapTree)}: going to start hub connection...");
            StartConnectionAction()
                .ContinueWith(JoinAction())
                .ContinueWith(LoadGameAction())
                .Subscribe();
        }

        //Action 1
        [Inject] private IHubConnection m_hubConnection = default;

        private IObservable<Unit> StartConnectionAction()
        {
            return m_hubConnection
                .Start()
                .Catch<Unit, Exception>(ex =>
                {
                    Debug.LogException(ex);
                    return Observable.Never(Unit.Default);
                })
                .ContinueWith(_ =>
                {
                    Debug.Log($"{nameof(BootstrapTree)}: the connection was open!");
                    return Observable.ReturnUnit();
                });
        }

        //Action 2
        [Inject] private IJoinOperation m_joinOperation = default;

        private IObservable<Unit> JoinAction()
        {
            return m_joinOperation
                .Join(new JoinOperationRequest($"Player_{Random.Range(0, 100)}").ToJson())
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

        //Action 3
        private IObservable<Unit> LoadGameAction()
        {
            return SceneManager
                .LoadSceneAsync("TicTacToe")
                .ToUniTask()
                .ToObservable()
                .ContinueWith(_ => Observable.ReturnUnit());
        }
    }
}