using System;
using TicTacToe.Shared;
using UnityEngine;
using UnityEngine.Assertions;

namespace TicTacToe.Client
{
    public sealed class SymbolPresenter2D : SymbolPresenter
    {
        private const string SymbolXAnimationName = "XTurnClip";
        private const string SymbolOAnimationName = "OTurnClip";
        private const string IdleAnimationName = "IDLE";

        [SerializeField] private Animator m_symbolAnimator = default;

        private void Awake()
        {
            Assert.IsNotNull(m_symbolAnimator);
        }

        public override void Show(Symbols target)
        {
            base.Show(target);
            gameObject.SetActive(true);
            Debug.Log("Turn: " + target);
            switch (target)
            {
                case Symbols.X:
                    m_symbolAnimator.Play(SymbolXAnimationName);
                    break;
                case Symbols.O:
                    m_symbolAnimator.Play(SymbolOAnimationName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(target), target, null);
            }
        }

        public override void Hide()
        {
            m_symbolAnimator.Play(IdleAnimationName);
            gameObject.SetActive(false);
            base.Hide();
        }
    }
}