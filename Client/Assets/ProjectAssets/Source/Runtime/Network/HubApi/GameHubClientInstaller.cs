using UnityEngine;
using Zenject;

namespace TicTacToe.Client
{
    public sealed class GameHubClientInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private string m_host = "https://localhost:5001";

        public override void InstallBindings()
        {
            Container
                .BindInterfacesTo<GameHubClient>()
                .AsSingle()
                .WithArguments(m_host);
        }
    }
}