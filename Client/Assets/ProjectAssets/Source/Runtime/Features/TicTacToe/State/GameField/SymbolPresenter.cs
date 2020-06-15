using Innovecs.DialoguesSystem;
using TicTacToe.Shared;
using UnityEngine;
using UnityEngine.Assertions;

namespace TicTacToe.Client
{
    public sealed class SymbolPresenter : BaseReferencePresenter<SymbolModel>
    {
        [SerializeField] private SpriteRenderer m_sprite = default;

        private void Awake()
        {
            Assert.IsNotNull(m_sprite);
        }

        public override void Show(SymbolModel target)
        {
            base.Show(target);
            m_sprite.gameObject.SetActive(true);
            if (target == null)
            {
                m_sprite.color = Color.red;
            }
        }

        public override void Hide()
        {
            m_sprite.gameObject.SetActive(false);
            base.Hide();
        }
    }
}