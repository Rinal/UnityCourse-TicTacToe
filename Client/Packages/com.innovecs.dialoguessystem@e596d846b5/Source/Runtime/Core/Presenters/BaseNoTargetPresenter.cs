using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Innovecs.DialoguesSystem
{
    public abstract class BaseNoTargetPresenter : BasePresenter
    {
        [SerializeField] private UnityEvent m_onShow = new UnityEvent();
        private readonly BoolReactiveProperty m_isOpen = new BoolReactiveProperty(false);
        public override IReadOnlyReactiveProperty<bool> IsOpen => m_isOpen;

        public virtual void Show()
        {
            m_isOpen.Value = true;
            m_onShow.Invoke();
        }

        public override void Hide()
        {
            m_isOpen.Value = false;
            base.Hide();
        }
    }
}