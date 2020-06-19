using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Innovecs.DialoguesSystem
{
    public abstract class BaseValueAsyncDialog<TModel> : BaseValueAsyncPresenter<TModel> where TModel : struct
    {
        [SerializeField] private bool m_hideOnAwake = false;

        private SignalBus m_signalBus = null;
        private HideDialogSignal m_hideDialogSignal = null;
        private ShowDialogSignal m_showDialogSignal = null;

        [Inject]
        private void Construct(SignalBus signalBus, HideDialogSignal hideDialogSignal, ShowDialogSignal showDialogSignal)
        {
            m_signalBus = signalBus;
            m_hideDialogSignal = hideDialogSignal;
            m_showDialogSignal = showDialogSignal;
        }

        protected virtual void Awake()
        {
            if (m_hideOnAwake)
            {
                Hide();
            }
        }

        public override IObservable<Unit> Show(TModel model)
        {
            m_showDialogSignal.Type = GetType();
            m_signalBus.TryFire(m_showDialogSignal);

            return base.Show(model);
        }

        public override void Hide()
        {
            if (IsOpen.Value)
            {
                m_hideDialogSignal.Type = GetType();
                m_signalBus.TryFire(m_hideDialogSignal);
            }

            base.Hide();
        }
    }
}