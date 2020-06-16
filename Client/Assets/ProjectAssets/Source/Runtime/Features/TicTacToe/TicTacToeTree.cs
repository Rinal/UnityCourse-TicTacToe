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
                .ContinueWith(ShowFieldPresenterAction())
                .Subscribe();
        }

        //Action 1
        [Inject] private UsersStatePresenter m_usersStatePresenter = default;
        [Inject] private ClientUsersState m_clientUsersState = default;

        private IObservable<Unit> ShowUsersStatePresenterAction()
        {
            m_usersStatePresenter.Show(m_clientUsersState);
            return Observable.ReturnUnit();
        }

        //Action 2
        [Inject] private FieldPresenter m_fieldPresenter = default;
        [Inject] private ClientFieldState m_clientFieldState = default;

        private IObservable<Unit> ShowFieldPresenterAction()
        {
            m_fieldPresenter.Show(m_clientFieldState);
            return Observable.ReturnUnit();
        }
    }
}