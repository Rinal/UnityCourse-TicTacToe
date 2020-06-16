using System.Collections.Generic;
using Innovecs.DialoguesSystem;
using TicTacToe.Server;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace TicTacToe.Client
{
    public sealed class UsersStatePresenter : BaseReferencePresenter<ClientUsersState>
    {
        [SerializeField] private GameObject m_content = default;
        [SerializeField] private List<UserPresenter> m_presenters = new List<UserPresenter>(RoomSize.Value);
        private readonly CompositeDisposable m_disposable = new CompositeDisposable();

        private void Awake()
        {
            Assert.IsNotNull(m_content);
            m_presenters.RemoveAll(p => p == null);
            Assert.IsTrue(m_presenters.Count == RoomSize.Value);
        }

        public override void Show(ClientUsersState target)
        {
            m_disposable.Clear();
            OnCollectionReset(Unit.Default);
            base.Show(target);
            Target.Users.ObserveAdd().Subscribe(OnUserAdded).AddTo(m_disposable);
            Target.Users.ObserveRemove().Subscribe(OnUserRemoved).AddTo(m_disposable);
            Target.Users.ObserveReset().Subscribe(OnCollectionReset).AddTo(m_disposable);
            int index = 0;
            foreach (UserModel model in Target.Users)
            {
                OnUserAdded(new CollectionAddEvent<UserModel>(index++, model));
            }

            m_content.SetActive(true);
        }

        private void OnUserAdded(CollectionAddEvent<UserModel> add)
        {
            m_presenters[add.Index].Show(add.Value);
        }

        private void OnUserRemoved(CollectionRemoveEvent<UserModel> remove)
        {
            m_presenters[remove.Index].Hide();
        }

        private void OnCollectionReset(Unit _)
        {
            foreach (UserPresenter presenter in m_presenters)
            {
                presenter.Hide();
            }
        }

        public override void Hide()
        {
            OnCollectionReset(Unit.Default);
            m_disposable.Clear();
            m_content.SetActive(false);
            base.Hide();
        }
    }
}