using TicTacToe.Shared;
using UnityEngine;
using UnityEngine.Assertions;

namespace TicTacToe.Client
{
    public sealed class SymbolPresenter2D : SymbolPresenter
    {
        [SerializeField] private SpriteRenderer m_sprite = default;

        private void Awake()
        {
            Assert.IsNotNull(m_sprite);
        }

        public override void Show(Symbols target)
        {
            base.Show(target);
            m_sprite.gameObject.SetActive(true);
            if (Target == Symbols.O)
            {
                m_sprite.color = Color.yellow;
            }
            else
            {
                m_sprite.color = Color.green;
            }
        }

        public override void Hide()
        {
            m_sprite.gameObject.SetActive(false);
            base.Hide();
        }
    }
}