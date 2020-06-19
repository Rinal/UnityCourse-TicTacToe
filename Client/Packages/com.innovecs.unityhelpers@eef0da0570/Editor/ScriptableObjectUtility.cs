using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Innovecs.UnityHelpers.EditorClasses
{
    [CustomEditor(typeof(MonoScript))]
    public sealed class ScriptableObjectUtility : Editor
    {
        private Type m_type;
        private bool m_isScriptableObject;

        private void OnEnable()
        {
            MonoScript script = (MonoScript) target;
            m_type = script.GetClass();
            m_isScriptableObject = IsScriptableObject(m_type);
        }

        public override void OnInspectorGUI()
        {
            if (!m_isScriptableObject)
            {
                base.OnInspectorGUI();
                return;
            }

            if (GUILayout.Button("Create Instance"))
            {
                Create(m_type, AssetDatabase.GetAssetPath(target));
            }
        }

        /// <summary>
        /// IReturn true if this type is Scriptable Object
        /// </summary>
        /// <returns>True, if this type can be created.</returns>
        public static bool IsScriptableObject(Type type)
        {
            return type != null && type.IsSubclassOf(typeof(ScriptableObject)) && !type.IsAbstract
                   && !type.IsSubclassOf(typeof(Editor)) && !type.IsSubclassOf(typeof(EditorWindow));
        }

        /// <summary>
        /// Creates a new scriptable object.
        /// </summary>
        /// <param name="type">Type to create.</param>
        /// <param name="assetPath">Path to create the new asset.</param>
        public static void Create(Type type, string assetPath)
        {
            string directory = Path.GetDirectoryName(assetPath);
            string fileName = Path.GetFileNameWithoutExtension(assetPath);
            ScriptableObject instance = CreateInstance(type);
            string path = AssetDatabase.GenerateUniqueAssetPath(directory + Path.DirectorySeparatorChar + fileName + ".asset");
            AssetDatabase.CreateAsset(instance, path);
            Object created = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
            EditorGUIUtility.PingObject(created);
        }
    }
}