using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace TicTacToe.Client
{
    public sealed class TicTacToeTree : MonoBehaviour
    {
        private void Start()
        {
            ShowUsersStatePresenterAction()
                .Subscribe();
        }

        [Inject] private UsersStatePresenter m_usersStatePresenter = default;
        [Inject] private ClientUsersState m_clientUsersState = default;

        private IObservable<Unit> ShowUsersStatePresenterAction()
        {
            m_usersStatePresenter.Show(m_clientUsersState);
            return Observable.ReturnUnit();
        }
    }
}