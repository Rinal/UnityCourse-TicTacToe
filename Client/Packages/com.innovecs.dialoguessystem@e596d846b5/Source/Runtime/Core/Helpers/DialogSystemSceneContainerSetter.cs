using System;
using UnityEngine;
using Zenject;

namespace Innovecs.DialoguesSystem
{
    public sealed class DialogSystemSceneContainerSetter : MonoBehaviour, IInitializable, IDisposable
    {
        private DiContainer m_container = null;
        private DialoguesSystem dialoguesSystem = null;

        [Inject]
        private void Construct(DiContainer container, DialoguesSystem dialoguesSystem)
        {
            m_container = container;
            this.dialoguesSystem = dialoguesSystem;
        }

        void IInitializable.Initialize()
        {
            dialoguesSystem.SetSceneContainer(m_container);
        }

        void IDisposable.Dispose()
        {
            dialoguesSystem.SetSceneContainer(null);
        }
    }
}