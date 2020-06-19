using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Object = UnityEngine.Object;

namespace Innovecs.DialoguesSystem
{
    /// <summary>
    /// Base dialog system allow to get dialog by dialog type.
    /// Loading dialog must implemented in child class
    /// </summary>
    public abstract class DialoguesSystem : IInitializable, IDisposable
    {
        private readonly Dictionary<Type, BasePresenter> m_dialogs =
            new Dictionary<Type, BasePresenter>();

        protected DiContainer Container
        {
            get
            {
                if (m_sceneContainer != null)
                {
                    return m_sceneContainer;
                }

                return m_projectContainer;
            }
        }

        private readonly DiContainer m_projectContainer = null;
        private DiContainer m_sceneContainer = null;

        protected readonly Canvas CanvasHolderPrefab = null;

        private Canvas m_dialogCanvas = null;

        protected Canvas DialogCanvas
        {
            get
            {
                if (m_dialogCanvas == null)
                {
                    m_dialogCanvas = Object.Instantiate(CanvasHolderPrefab);
                    Object.DontDestroyOnLoad(m_dialogCanvas);
                }

                return m_dialogCanvas;
            }
        }

        protected DialoguesSystem(DiContainer container, Canvas canvasPrefab)
        {
            CanvasHolderPrefab = canvasPrefab;
            m_projectContainer = container;
        }

        /// <summary>
        /// Add dialog by type if dictionary not contains this dialog
        /// </summary>
        /// <param name="dialogType"></param>
        /// <param name="prefab"></param>
        protected void Add(Type dialogType, BasePresenter prefab)
        {
            if (!m_dialogs.ContainsKey(dialogType))
            {
                m_dialogs.Add(dialogType, prefab);
            }
            else
            {
                throw new Exception($"dialog with type {dialogType} already exists in Dialogs dictionary");
            }
        }

        public virtual void Initialize()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        /// <summary>
        /// Base dialog function allow to get dialog by  DialogType. Return specific dialog as observable
        /// </summary>
        /// <typeparam name="TDialog"></typeparam>
        /// <returns></returns>
        public IObservable<TDialog> Get<TDialog>() where TDialog : BasePresenter
        {
            if (m_dialogs.ContainsKey(typeof(TDialog)))
            {
                return Observable.Return(Get(typeof(TDialog)) as TDialog);
            }

            return Load<TDialog>();
        }

        /// <summary>
        /// Allow get dialog from pool(dictionary) if this dialog already instantiated
        /// </summary>
        /// <param name="dialogType"></param>
        /// <returns></returns>
        private BasePresenter Get(Type dialogType)
        {
            BasePresenter dialog = null;
            if (m_dialogs.TryGetValue(dialogType, out dialog))
            {
                return dialog;
            }

            throw new Exception($"Failed to get dialog {dialogType} from DialogDictionary");
        }

        /// <summary>
        /// Implementing this method by system allow load dialog from spcific resource (settings, folder, asset bundle, etc.)
        /// </summary>
        /// <typeparam name="TDialog">Presenter class of dialog that use for loading</typeparam>
        /// <returns></returns>
        protected abstract IObservable<TDialog> Load<TDialog>() where TDialog : BasePresenter;

        void IDisposable.Dispose()
        {
            m_dialogs.Clear();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void SetSceneContainer(DiContainer diContainer)
        {
            m_sceneContainer = diContainer;
        }

        /// <summary>
        /// This method is needed to reset camera after we load the scene
        /// Basically, it is not perfect way to do it, but we didn't find another option at the moment
        /// If we don't do this, the canvas camera doesn't render the dialogs on the new scene loaded
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="loadSceneMode"></param>
        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (m_dialogCanvas == null
                || m_dialogCanvas.worldCamera == null
                || m_dialogCanvas.worldCamera.gameObject == null
            )
            {
                return;
            }

            m_dialogCanvas.worldCamera.gameObject.SetActive(false);
            m_dialogCanvas.worldCamera.gameObject.SetActive(true);
        }
    }
}