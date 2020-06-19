using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Innovecs.DialoguesSystem
{
    public abstract class BasePresenter : MonoBehaviour
    {
        [SerializeField] private UnityEvent m_onHide = new UnityEvent();

        public abstract IReadOnlyReactiveProperty<bool> IsOpen { get; }

        public virtual void Hide()
        {
            m_onHide.Invoke();
        }
    }
}