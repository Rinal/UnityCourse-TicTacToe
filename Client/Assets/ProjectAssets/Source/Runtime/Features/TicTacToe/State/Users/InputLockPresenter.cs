using System;
using Innovecs.DialoguesSystem;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace TicTacToe.Client
{
    public sealed class InputLockPresenter : BaseNoTargetPresenter
    {
        [SerializeField] private BoxCollider2D m_boxCollider2D = default;
        [Inject] private IActiveUserObservable m_activeUserObservable = default;
        [Inject] private IUserIdObservable m_userIdObservable = default;
        private IDisposable m_onActiveChanged = default;

        private void Awake()
        {
            Assert.IsNotNull(m_boxCollider2D);
        }

        private void OnEnable()
        {
            m_onActiveChanged = m_activeUserObservable.Id.Subscribe(OnActiveChanged);
            OnActiveChanged(m_activeUserObservable.Id.Value);
        }

        private void OnDisable()
        {
            m_onActiveChanged?.Dispose();
        }

        private void OnActiveChanged(string id)
        {
            m_boxCollider2D.enabled = !string.IsNullOrEmpty(id) && m_userIdObservable.Id.Value.Equals(id);
        }
    }
}