using System.Collections.Generic;
using Innovecs.DialoguesSystem;
using TicTacToe.Shared;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using Zenject;

namespace TicTacToe.Client
{
    public sealed class FieldPresenter : BaseReferencePresenter<ClientFieldState>
        , IPointerClickHandler
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
                .ObserveReplace()
                .Subscribe(OnCellChanged)
                .AddTo(m_subscriptions);
            foreach (KeyValuePair<Vector2Int, CellModel> pair in Target.Field)
            {
                OnCellChanged(new DictionaryReplaceEvent<Vector2Int, CellModel>(pair.Key, null, pair.Value));
            }
        }

        private void OnCellChanged(DictionaryReplaceEvent<Vector2Int, CellModel> replace)
        {
            m_presenters[replace.Key].Show(replace.NewValue);
        }

        public override void Hide()
        {
            m_subscriptions.Clear();
            foreach (CellPresenter presenter in m_presenters.Values)
            {
                presenter.Hide();
            }

            base.Hide();
        }

        private void CreatePresenters()
        {
            Vector3 halfSize = m_grid.cellSize * 0.5f;
            for (int i = 0; i < FieldSize.Value; i++)
            {
                for (int j = 0; j < FieldSize.Value; j++)
                {
                    Vector3 position = m_grid.CellToLocal(new Vector3Int(i, j, 0));
                    CellPresenter presenter = Instantiate(m_prefab, m_grid.transform, true);
                    presenter.transform.localPosition = new Vector3(position.x + halfSize.x, position.y + halfSize.y);
                    presenter.Hide();
                    m_presenters.Add(new Vector2Int(i, j), presenter);
                }
            }
        }

        [Inject] private ISettableField m_settableField = default;
        [Inject] private ICheckableField m_checkableField = default;

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            Vector3Int cell = m_grid.WorldToCell(eventData.pointerCurrentRaycast.worldPosition);
            if (m_checkableField.IsEmpty(cell.x, cell.y))
            {
                //TODO Tem hardcoded!
                m_settableField.Set(Symbols.X, cell.x, cell.y);
            }
            else
            {
                Debug.Log($"{nameof(FieldPresenter)}: {cell} is not empty!");
            }

            
        }
    }
}