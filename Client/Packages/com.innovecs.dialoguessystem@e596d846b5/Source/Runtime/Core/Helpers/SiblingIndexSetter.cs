using UnityEngine;

namespace Innovecs.DialoguesSystem
{
    /// <summary>
    /// This class is useful when you to set sibling index for the presenter
    /// </summary>
    public sealed class SiblingIndexSetter : MonoBehaviour
    {
        [SerializeField] private int m_siblingIndex = -1;
        [SerializeField] private bool m_isLast = false;
        private BasePresenter dialog = null;

        /// <summary>
        /// This method is responsible for setting the sibling index of current dialog.
        /// It's public because we attach it to Unity event in the editor.
        /// </summary>
        public void Set()
        {
            if (m_siblingIndex < 0)
            {
                return;
            }

            if (dialog == null)
            {
                dialog = gameObject.GetComponent<BasePresenter>();
            }

            if (dialog != null)
            {
                if (!m_isLast)
                {
                    dialog.transform.SetSiblingIndex(m_siblingIndex);
                }
                else
                {
                    dialog.transform.SetAsLastSibling();
                }
            }
        }
    }
}