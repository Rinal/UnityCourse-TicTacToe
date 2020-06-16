using System;
using Innovecs.DialoguesSystem;
using TicTacToe.Server;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace TicTacToe.Client
{
    public sealed class ActiveUserPresenter : BaseReferencePresenter<UserModel>
    {
        [SerializeField] private TextMeshProUGUI m_activeText = default;
        [Inject] private IActiveUserObservable m_activeUserObservable = default;
        private IDisposable m_onActiveChanged = default;

        private void Awake()
        {
            Assert.IsNotNull(m_activeText);
        }

        public override void Show(UserModel target)
        {
            m_onActiveChanged?.Dispose();
            base.Show(target);
            m_onActiveChanged = m_activeUserObservable.Id.Subscribe(OnActiveChanged);
            OnActiveChanged(m_activeUserObservable.Id.Value);
        }

        public override void Hide()
        {
            m_onActiveChanged?.Dispose();
            OnActiveChanged(string.Empty);
            base.Hide();
        }

        private void OnActiveChanged(string id)
        {
            m_activeText.gameObject.SetActive(Target != null && !string.IsNullOrEmpty(id) && Target.Id.Equals(id));
        }
    }
}