using UnityEngine;
using Zenject;

namespace TicTacToe.Client
{
    public sealed class GameStateInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<ClientFieldState>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<ClientUsersState>()
                .AsSingle();
        }
    }
}