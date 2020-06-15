using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using TicTacToe.Shared;
using UniRx;
using UnityEngine;
using Zenject;

namespace TicTacToe.Client
{
    public sealed class GameHubClient : IHubConnection
        , IJoinOperation
        , IFieldChangedEventsObservable
    {
        private readonly HubConnection m_connection = default;
        private readonly string m_uri = default;
        private IObservable<Unit> m_startConnectionProcess = default;
        private IObservable<Unit> m_stopConnectionProcess = default;

        [Inject]
        public GameHubClient(string host)
        {
            m_uri = $"{host}{Paths.GameHub}";
            m_connection = new HubConnectionBuilder()
                .WithUrl(m_uri)
                .Build();
        }

        public IObservable<Unit> Start()
        {
            if (m_startConnectionProcess != null)
            {
                return m_startConnectionProcess;
            }

            Debug.Log($"{nameof(GameHubClient)}: going to start hub connection to {m_uri}");
            m_startConnectionProcess = m_connection
                .StartAsync()
                .ToObservable()
                .ContinueWith(_ =>
                {
                    Debug.Log($"{nameof(GameHubClient)}: hub connection was open.");
                    m_startConnectionProcess = null;
                    return Observable.ReturnUnit();
                });
            return m_startConnectionProcess;
        }

        public IObservable<Unit> Stop()
        {
            if (m_stopConnectionProcess != null)
            {
                return m_stopConnectionProcess;
            }

            m_stopConnectionProcess = m_connection
                .StopAsync()
                .ToObservable()
                .ContinueWith(_ =>
                {
                    Debug.Log($"{nameof(GameHubClient)}: hub connection was closed.");
                    m_stopConnectionProcess = null;
                    return Observable.ReturnUnit();
                });
            return m_stopConnectionProcess;
        }

        public async Task Join(string json)
        {
            await m_connection.InvokeAsync(nameof(IJoinOperation.Join), json);
        }

        private Subject<FieldChangedData> m_hubEvent = default;

        public IObservable<FieldChangedData> OnFieldChanged()
        {
            if (m_hubEvent == null)
            {
                m_hubEvent = new Subject<FieldChangedData>();
                m_connection
                    .On<string>(nameof(IFieldChangedEvent.OnFieldChanged), (json) =>
                    {
                        Debug.Log($"{nameof(GameHubClient)}: got event with data {json}");
                        // JObject jo = JObject.Parse(json);
                        // m_hubEvent.OnNext(jo);
                    });
            }

            return m_hubEvent;
        }
    }
}