using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Innovecs.DialoguesSystem;
using TicTacToe.Server;
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
        [SerializeField] private LineRenderer m_lineRenderer = default;
        private readonly Dictionary<Vector2Int, CellPresenter> m_presenters = new Dictionary<Vector2Int, CellPresenter>(FieldSize.Value * FieldSize.Value);
        private readonly CompositeDisposable m_subscriptions = new CompositeDisposable();


        private void Awake()
        {
            Assert.IsNotNull(m_prefab);
            Assert.IsNotNull(m_grid);
            Assert.IsNotNull(m_lineRenderer);
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

            Target
                .WinCellsPosition
                .Subscribe(AddWinCells)
                .AddTo(m_subscriptions);
        }


        private void AddWinCells(IEnumerable<Vector2Int> positions)
        {
            m_lineRenderer.enabled = true;
            m_lineRenderer.widthMultiplier = 0.1f;
            m_lineRenderer.positionCount = 2;
            switch (m_presenters[positions.First()].Target.Symbol)
            {
                case Symbols.X:
                    m_lineRenderer.startColor = new Color(0.33f, 0.33f, 0.33f, 1f);
                    m_lineRenderer.endColor = new Color(0.33f, 0.33f, 0.33f, 1f);
                    break;
                case Symbols.O:
                    m_lineRenderer.startColor = new Color(0.95f, 0.92f, 0.83f, 1f);
                    m_lineRenderer.endColor = new Color(0.95f, 0.92f, 0.83f, 1f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            float duration = 1.25f;

            m_lineRenderer.SetPosition(0, m_presenters[positions.First()].transform.position);
            m_lineRenderer.SetPosition(1, m_presenters[positions.First()].transform.position);

            Tweener tween = DOTween.To((current) =>
            {
                float distance =
                    Vector2.Distance(m_presenters[positions.First()].transform.position,
                        m_presenters[positions.Last()].transform.position);
                m_lineRenderer.SetPosition(1, Vector2.MoveTowards(m_presenters[positions.First()].transform.position, m_presenters[positions.Last()].transform.position, distance * current));
            }, 0, 1, duration);

            tween.SetEase(Ease.Linear);
            tween.onComplete = () =>
            {
                m_lineRenderer.enabled = false;
                for (int i = 0; i < positions.Count(); i++)
                {
                    m_presenters[positions.ElementAt(i)].transform
                        .DOScale(m_presenters[positions.ElementAt(i)].transform.localScale * 3f, 0.75f);
                    m_presenters[positions.ElementAt(i)].transform.DOMove(Vector3.zero, 0.75f);
                }
            };


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
        [Inject] private IUserIdObservable m_userIdObservable = default;
        [Inject] private ClientUsersState m_clientUsersState = default;

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            Vector3Int cell = m_grid.WorldToCell(eventData.pointerCurrentRaycast.worldPosition);
            if (m_checkableField.IsEmpty(cell.x, cell.y))
            {
                UserModel me = m_clientUsersState.Users.FirstOrDefault(u => u.Id.Equals(m_userIdObservable.Id.Value));
                if (me == null)
                {
                    Debug.LogError($"{nameof(FieldPresenter)}: failed to find my user!");
                    return;
                }

                m_settableField.Set(me.Symbol, cell.x, cell.y);
            }
            else
            {
                Debug.Log($"{nameof(FieldPresenter)}: {cell} is not empty!");
            }
        }
    }
}