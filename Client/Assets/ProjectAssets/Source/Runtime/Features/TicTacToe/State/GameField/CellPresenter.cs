using Innovecs.DialoguesSystem;
using TicTacToe.Shared;
using UnityEngine;
using UnityEngine.Assertions;

namespace TicTacToe.Client
{
    public sealed class CellPresenter : BaseReferencePresenter<CellModel>
    {
        [SerializeField] private SpriteRenderer m_sprite = default;
        [SerializeField] private SymbolPresenter m_symbolPresenter = default;

        private void Awake()
        {
            Assert.IsNotNull(m_sprite);
            Assert.IsNotNull(m_symbolPresenter);
        }

        public override void Show(CellModel target)
        {
            base.Show(target);
            m_sprite.gameObject.SetActive(true);
            if (Target == null)
            {
                m_sprite.color = new Color(0, 0, 0, 0);
            }
            else
            {
                m_symbolPresenter.Show(Target.Symbol);
            }
        }

        public override void Hide()
        {
            m_sprite.color = Color.white;
            m_sprite.gameObject.SetActive(false);
            m_symbolPresenter.Hide();
            base.Hide();
        }
    }
}