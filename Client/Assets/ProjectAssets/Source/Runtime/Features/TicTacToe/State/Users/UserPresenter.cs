using Innovecs.DialoguesSystem;
using TicTacToe.Server;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace TicTacToe.Client
{
    public sealed class UserPresenter : BaseReferencePresenter<UserModel>
    {
        [SerializeField] private GameObject m_content = default;
        [SerializeField] private TextMeshProUGUI m_nameText = default;
        [SerializeField] private SymbolPresenter m_symbolPresenter = default;

        private void Awake()
        {
            Assert.IsNotNull(m_content);
            Assert.IsNotNull(m_nameText);
            Assert.IsNotNull(m_symbolPresenter);
        }

        public override void Show(UserModel target)
        {
            base.Show(target);
            m_nameText.text = Target.Name;
            m_symbolPresenter.Show(Target.Symbol);
            m_content.SetActive(true);
        }

        public override void Hide()
        {
            m_symbolPresenter.Hide();
            m_content.SetActive(false);
            base.Hide();
        }
    }
}