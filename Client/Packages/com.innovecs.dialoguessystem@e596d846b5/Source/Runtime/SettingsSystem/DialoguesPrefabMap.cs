using System.Collections.Generic;
using UnityEngine;

namespace Innovecs.DialoguesSystem
{
    /// <summary>
    /// Hold dialog prefabs (assign new dialog to ScriptableObject)
    /// </summary>
    public sealed class DialoguesPrefabMap : ScriptableObject
    {
        [SerializeField] private List<BasePresenter> m_prefabs = new List<BasePresenter>();

        public IEnumerable<BasePresenter> Prefabs => m_prefabs;
    }
}