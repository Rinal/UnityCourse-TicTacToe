using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Innovecs.DialoguesSystem
{
    /// <summary>
    /// This class responsible for load dialog from settings by dialog type
    /// </summary>
    public sealed class SettingsDialoguesSystem : DialoguesSystem
    {
        private readonly DialoguesPrefabMap m_settings = null;

        [Inject]
        private SettingsDialoguesSystem(DiContainer diContainer, DialoguesPrefabMap settings, Canvas canvasPrefab) : base(diContainer, canvasPrefab)
        {
            m_settings = settings;
        }

        protected override IObservable<TDialog> Load<TDialog>()
        {
            BasePresenter prefab = null;

            foreach (BasePresenter dialog in m_settings.Prefabs)
            {
                if (dialog.GetType() == typeof(TDialog))
                {
                    prefab = dialog;
                }
            }

            if (prefab != null)
            {
                BasePresenter dialog = Container.InstantiatePrefabForComponent<TDialog>(prefab, DialogCanvas.transform);
                Add(dialog.GetType(), dialog);
                return Observable.Return(dialog as TDialog);
            }

            throw new Exception($"{GetType().Name}: Failed to find dialog by '{typeof(TDialog)}' type in DialogSettings!");
        }
    }
}