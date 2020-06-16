using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TicTacToe.Server;
using UniRx;
using UnityEngine;
using Zenject;

namespace TicTacToe.Client
{
    public sealed class ClientUsersState :
        IInitializable
        , IActiveUserObservable
        , IDisposable
    {
        private readonly ReactiveCollection<UserModel> m_users = new ReactiveCollection<UserModel>();
        private readonly IStateChangedObservable m_stateChangedObservable = default;
        private IDisposable m_onStateChanged = default;

        [Inject]
        public ClientUsersState(IStateChangedObservable stateChangedObservable)
        {
            m_stateChangedObservable = stateChangedObservable;
        }

        public IReactiveCollection<UserModel> Users => m_users;

        void IInitializable.Initialize()
        {
            m_onStateChanged = m_stateChangedObservable.OnStateChanged().Subscribe(OnStateChanged);
        }

        private void OnStateChanged(JObject delta)
        {
            if (delta.TryGetValue(nameof(UserModel), out JToken token))
            {
                Debug.Log($"{nameof(ClientUsersState)}: got model changes {token}.");
                List<UserModel> users = JsonConvert.DeserializeObject<List<UserModel>>(token.ToString());
                m_users.Clear();
                foreach (UserModel model in users)
                {
                    m_users.Add(model);
                }
            }
        }

        void IDisposable.Dispose()
        {
            m_onStateChanged?.Dispose();
            m_onStateChanged = default;
        }

        private readonly ReactiveProperty<string> m_activeUserId = new ReactiveProperty<string>(string.Empty);
        IReadOnlyReactiveProperty<string> IActiveUserObservable.Id => m_activeUserId;
    }
}