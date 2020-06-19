using System;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Innovecs.DialoguesSystem
{
    public abstract class BaseDialoguesSystemInstaller : ScriptableObjectInstaller
    {
        [SerializeField] protected Canvas m_canvasPrefab = default;

        public override void InstallBindings()
        {
            Assert.IsNotNull(m_canvasPrefab);

            Container
                .Bind<HideDialogSignal>()
                .AsSingle();

            Container
                .Bind<ShowDialogSignal>()
                .AsSingle();

            Container
                .DeclareSignal<HideDialogSignal>()
                .OptionalSubscriber();

            Container
                .DeclareSignal<ShowDialogSignal>()
                .OptionalSubscriber();

            Container
                .Bind<IInitializable>()
                .To<DialoguesSystem>()
                .FromResolve();

            Container
                .Bind<IDisposable>()
                .To<DialoguesSystem>()
                .FromResolve();
        }
    }
}