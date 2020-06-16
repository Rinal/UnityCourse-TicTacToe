using Innovecs.DialoguesSystem;
using TicTacToe.Shared;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace TicTacToe.Client
{
    public sealed class SymbolPresenter : BaseValuePresenter<Symbols>
    {
        [SerializeField] private TextMeshProUGUI m_symbolText = default;

        private void Awake()
        {
            Assert.IsNotNull(m_symbolText);
        }

        public override void Show(Symbols target)
        {
            base.Show(target);
            m_symbolText.text = Target.ToString();
            m_symbolText.gameObject.SetActive(true);
        }

        public override void Hide()
        {
            m_symbolText.gameObject.SetActive(false);
            base.Hide();
        }
    }
}