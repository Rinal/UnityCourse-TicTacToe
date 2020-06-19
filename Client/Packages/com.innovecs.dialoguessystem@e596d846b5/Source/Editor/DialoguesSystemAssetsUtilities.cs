using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Innovecs.DialoguesSystem.EditorClasses
{
    public static class DialoguesSystemAssetsUtilities
    {
        [MenuItem("Assets/Create/Dialogues System/Settings System/Installer", false, 1)]
        public static void CreateSettingsSystemInstaller()
        {
            CreateInstaller<SettingsDialogSystemInstaller>();
        }

        [MenuItem("Assets/Create/Dialogues System/Settings System/Prefabs map", false, 1)]
        public static void CreateSettingsPrefabMap()
        {
            string path = GetSelectedFolderPath();

            if (string.IsNullOrEmpty(path))
            {
                throw new Exception($"{nameof(DialoguesSystemAssetsUtilities)}: failed to create {typeof(DialoguesPrefabMap).Name} due to invalid path value!");
            }

            path = Path.Combine(path, nameof(DialoguesPrefabMap));
            Create(typeof(DialoguesPrefabMap), path);
        }

        private static void CreateInstaller<TInstaller>() where TInstaller : ScriptableObjectInstaller
        {
            string path = GetSelectedFolderPath();

            if (string.IsNullOrEmpty(path))
            {
                throw new Exception($"{nameof(DialoguesSystemAssetsUtilities)}: failed to create {typeof(TInstaller).Name} due to invalid path value!");
            }

            path = Path.Combine(path, typeof(TInstaller).Name);
            Create(typeof(TInstaller), path);
        }

        private static string GetSelectedFolderPath()
        {
            Object target = Selection.activeObject;
            string path = AssetDatabase.GetAssetPath(target);
            //Define if user clicked on the asset instead of folder
            if (target.GetType() != typeof(DefaultAsset))
            {
                path = Path.GetDirectoryName(path);
            }

            return path;
        }

        private static void Create(Type type, string assetPath)
        {
            string directory = Path.GetDirectoryName(assetPath);
            string fileName = Path.GetFileNameWithoutExtension(assetPath);
            ScriptableObject instance = ScriptableObject.CreateInstance(type);
            string path = AssetDatabase.GenerateUniqueAssetPath(directory + Path.DirectorySeparatorChar + fileName + ".asset");
            AssetDatabase.CreateAsset(instance, path);
            Object created = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
            EditorGUIUtility.PingObject(created);
        }
    }
}