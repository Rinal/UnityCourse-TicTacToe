using UnityEngine;
using Zenject;

namespace Innovecs.DialoguesSystem
{
    public abstract class BaseReferenceDialog<TModel> : BaseReferencePresenter<TModel> where TModel : class
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

        public override void Show(TModel model)
        {
            base.Show(model);
            m_showDialogSignal.Type = GetType();
            m_signalBus.TryFire(m_showDialogSignal);
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