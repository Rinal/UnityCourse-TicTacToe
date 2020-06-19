using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Innovecs.DialoguesSystem
{
    /// <summary>
    /// Class allow bind system that allow to instantiate dialogs from internal dialog settings
    /// </summary>
    public sealed class SettingsDialogSystemInstaller : BaseDialoguesSystemInstaller
    {
        [SerializeField] private DialoguesPrefabMap m_settings = null;

        public override void InstallBindings()
        {
            Assert.IsNotNull(m_settings);

            base.InstallBindings();

            Container
                .Bind<DialoguesSystem>()
                .To<SettingsDialoguesSystem>()
                .AsSingle()
                .WithArguments(m_settings, m_canvasPrefab);
        }
    }
}