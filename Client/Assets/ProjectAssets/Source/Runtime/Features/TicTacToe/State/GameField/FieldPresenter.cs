using System.Collections.Generic;
using Innovecs.DialoguesSystem;
using TicTacToe.Shared;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace TicTacToe.Client
{
    public sealed class FieldPresenter : BaseReferencePresenter<ClientFieldState>
    {
        [SerializeField] private CellPresenter m_prefab = default;
        [SerializeField] private Grid m_grid = default;
        private readonly Dictionary<Vector2Int, CellPresenter> m_presenters = new Dictionary<Vector2Int, CellPresenter>(FieldSize.Value * FieldSize.Value);
        private readonly CompositeDisposable m_subscriptions = new CompositeDisposable();

        private void Awake()
        {
            Assert.IsNotNull(m_prefab);
            Assert.IsNotNull(m_grid);
            CreatePresenters();
        }

        private void OnDestroy()
        {
            m_subscriptions.Clear();
            m_subscriptions.Dispose();
        }

        public override void Show(ClientFieldState target)
        {
            m_subscriptions.Clear();
            base.Show(target);
            Target
                .Field
                .ObserveAdd()
                .Subscribe(item => { m_presenters[item.Key].Show(item.Value); })
                .AddTo(m_subscriptions);
        }

        public override void Hide()
        {
            m_subscriptions.Clear();
            base.Hide();
        }

        private void CreatePresenters()
        {
            for (int i = 0; i < FieldSize.Value; i++)
            {
                for (int j = 0; j < FieldSize.Value; j++)
                {
                    Vector3 position = m_grid.CellToLocal(new Vector3Int(i, j, 0));
                    CellPresenter presenter = Instantiate(m_prefab, m_grid.transform, true);
                    presenter.transform.localPosition = position;
                    presenter.Hide();
                    m_presenters.Add(new Vector2Int(i, j), presenter);
                }
            }
        }
    }
}