using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Innovecs.DialoguesSystem
{
    public abstract class BaseReferencePresenter<TTarget> : BasePresenter
        where TTarget : class
    {
        [SerializeField] private UnityEvent m_onShow = new UnityEvent();
        private readonly BoolReactiveProperty m_isOpen = new BoolReactiveProperty(false);
        public override IReadOnlyReactiveProperty<bool> IsOpen => m_isOpen;

        public TTarget Target { get; private set; } = default;

        public virtual void Show(TTarget target)
        {
            Target = target;
            m_isOpen.Value = true;
            m_onShow.Invoke();
        }

        public override void Hide()
        {
            m_isOpen.Value = false;
            Target = default;
            base.Hide();
        }
    }

    public abstract class BaseReferencePresenter<TTarget1, TTarget2> : BasePresenter
        where TTarget1 : class
        where TTarget2 : class
    {
        [SerializeField] private UnityEvent m_onShow = new UnityEvent();
        private readonly BoolReactiveProperty m_isOpen = new BoolReactiveProperty(false);
        public override IReadOnlyReactiveProperty<bool> IsOpen => m_isOpen;

        public TTarget1 Target1 { get; private set; } = default;
        public TTarget2 Target2 { get; private set; } = default;

        public virtual void Show(TTarget1 target1, TTarget2 target2)
        {
            Target1 = target1;
            Target2 = target2;
            m_isOpen.Value = true;
            m_onShow.Invoke();
        }

        public override void Hide()
        {
            m_isOpen.Value = false;
            Target1 = default;
            Target2 = default;
            base.Hide();
        }
    }
}