using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Innovecs.UnityHelpers.EditorClasses
{
    /// <summary>
    /// This Editor utility helps to refactor Unity animations.
    /// </summary>
    public sealed class AnimationHierarchyEditor : EditorWindow
    {
        private const int ColumnWidth = 300;
        private Animator m_animatorObject = default;
        private readonly List<AnimationClip> m_animationClips = default;
        private ArrayList m_pathsKeys = default;
        private Hashtable m_paths = default;
        private readonly Dictionary<string, string> m_tempPathOverrides = default;
        private Vector2 m_scrollPos = Vector2.zero;
        private string m_originalRoot = "Root";
        private string m_newRoot = "SomeNewObject/Root";
        private string m_replacementOldRoot = default;
        private string m_replacementNewRoot = default;

        [MenuItem("Window/Animation Hierarchy Editor")]
        private static void ShowWindow()
        {
            EditorWindow.GetWindow<AnimationHierarchyEditor>();
        }

        public AnimationHierarchyEditor()
        {
            m_animationClips = new List<AnimationClip>();
            m_tempPathOverrides = new Dictionary<string, string>();
        }

        private void OnSelectionChange()
        {
            if (Selection.objects.Length > 1)
            {
                Debug.Log("Length? " + Selection.objects.Length);
                m_animationClips.Clear();
                foreach (Object o in Selection.objects)
                {
                    if (o is AnimationClip) m_animationClips.Add((AnimationClip) o);
                }
            }
            else if (Selection.activeObject is AnimationClip)
            {
                m_animationClips.Clear();
                m_animationClips.Add((AnimationClip) Selection.activeObject);
                FillModel();
            }
            else
            {
                m_animationClips.Clear();
            }

            this.Repaint();
        }

        private void OnGUI()
        {
            if (Event.current.type == EventType.ValidateCommand)
            {
                switch (Event.current.commandName)
                {
                    case "UndoRedoPerformed":
                        FillModel();

                        break;
                }
            }

            if (m_animationClips.Count > 0)
            {
                m_scrollPos = GUILayout.BeginScrollView(m_scrollPos, GUIStyle.none);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Referenced Animator (Root):", GUILayout.Width(ColumnWidth));

                m_animatorObject = ((Animator) EditorGUILayout.ObjectField(
                        m_animatorObject,
                        typeof(Animator),
                        true,
                        GUILayout.Width(ColumnWidth))
                    );

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Animation Clip:", GUILayout.Width(ColumnWidth));

                if (m_animationClips.Count == 1)
                {
                    m_animationClips[0] = ((AnimationClip) EditorGUILayout.ObjectField(
                            m_animationClips[0],
                            typeof(AnimationClip),
                            true,
                            GUILayout.Width(ColumnWidth))
                        );
                }
                else
                {
                    GUILayout.Label("Multiple Anim Clips: " + m_animationClips.Count, GUILayout.Width(ColumnWidth));
                }

                EditorGUILayout.EndHorizontal();

                GUILayout.Space(20);

                EditorGUILayout.BeginHorizontal();

                m_originalRoot = EditorGUILayout.TextField(m_originalRoot, GUILayout.Width(ColumnWidth));
                m_newRoot = EditorGUILayout.TextField(m_newRoot, GUILayout.Width(ColumnWidth));
                if (GUILayout.Button("Replace Root"))
                {
                    Debug.Log("O: " + m_originalRoot + " N: " + m_newRoot);
                    ReplaceRoot(m_originalRoot, m_newRoot);
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Reference path:", GUILayout.Width(ColumnWidth));
                GUILayout.Label("Animated properties:", GUILayout.Width(ColumnWidth * 0.5f));
                GUILayout.Label("(Count)", GUILayout.Width(60));
                GUILayout.Label("Object:", GUILayout.Width(ColumnWidth));
                EditorGUILayout.EndHorizontal();

                if (m_paths != null)
                {
                    foreach (string path in m_pathsKeys)
                    {
                        GuiCreatePathItem(path);
                    }
                }

                GUILayout.Space(40);
                GUILayout.EndScrollView();
            }
            else
            {
                GUILayout.Label("Please select an Animation Clip");
            }
        }

        private void GuiCreatePathItem(string path)
        {
            string newPath = path;
            GameObject obj = FindObjectInRoot(path);
            GameObject newObj;
            ArrayList properties = (ArrayList) m_paths[path];

            string pathOverride = path;

            if (m_tempPathOverrides.ContainsKey(path)) pathOverride = m_tempPathOverrides[path];

            EditorGUILayout.BeginHorizontal();

            pathOverride = EditorGUILayout.TextField(pathOverride, GUILayout.Width(ColumnWidth));
            if (pathOverride != path) m_tempPathOverrides[path] = pathOverride;

            if (GUILayout.Button("Change", GUILayout.Width(60)))
            {
                newPath = pathOverride;
                m_tempPathOverrides.Remove(path);
            }

            EditorGUILayout.LabelField(
                properties != null ? properties.Count.ToString() : "0",
                GUILayout.Width(60)
            );

            Color standardColor = GUI.color;

            if (obj != null)
            {
                GUI.color = Color.green;
            }
            else
            {
                GUI.color = Color.red;
            }

            newObj = (GameObject) EditorGUILayout.ObjectField(
                obj,
                typeof(GameObject),
                true,
                GUILayout.Width(ColumnWidth)
            );

            GUI.color = standardColor;

            EditorGUILayout.EndHorizontal();

            try
            {
                if (obj != newObj)
                {
                    UpdatePath(path, ChildPath(newObj));
                }

                if (newPath != path)
                {
                    UpdatePath(path, newPath);
                }
            }
            catch (UnityException ex)
            {
                Debug.LogError(ex.Message);
            }
        }

        private void OnInspectorUpdate()
        {
            this.Repaint();
        }

        private void FillModel()
        {
            m_paths = new Hashtable();
            m_pathsKeys = new ArrayList();

            foreach (AnimationClip animationClip in m_animationClips)
            {
                FillModelWithCurves(AnimationUtility.GetCurveBindings(animationClip));
                FillModelWithCurves(AnimationUtility.GetObjectReferenceCurveBindings(animationClip));
            }
        }

        private void FillModelWithCurves(EditorCurveBinding[] curves)
        {
            foreach (EditorCurveBinding curveData in curves)
            {
                string key = curveData.path;

                if (m_paths.ContainsKey(key))
                {
                    ((ArrayList) m_paths[key]).Add(curveData);
                }
                else
                {
                    ArrayList newProperties = new ArrayList();
                    newProperties.Add(curveData);
                    m_paths.Add(key, newProperties);
                    m_pathsKeys.Add(key);
                }
            }
        }

        private void ReplaceRoot(string oldRoot, string newRoot)
        {
            float fProgress = 0.0f;
            m_replacementOldRoot = oldRoot;
            m_replacementNewRoot = newRoot;

            AssetDatabase.StartAssetEditing();

            for (int iCurrentClip = 0; iCurrentClip < m_animationClips.Count; iCurrentClip++)
            {
                AnimationClip animationClip = m_animationClips[iCurrentClip];
                Undo.RecordObject(animationClip, "Animation Hierarchy Root Change");

                for (int iCurrentPath = 0; iCurrentPath < m_pathsKeys.Count; iCurrentPath++)
                {
                    string path = m_pathsKeys[iCurrentPath] as string;
                    ArrayList curves = (ArrayList) m_paths[path];

                    for (int i = 0; i < curves.Count; i++)
                    {
                        EditorCurveBinding binding = (EditorCurveBinding) curves[i];

                        if (path.Contains(m_replacementOldRoot))
                        {
                            if (!path.Contains(m_replacementNewRoot))
                            {
                                string sNewPath = Regex.Replace(path, "^" + m_replacementOldRoot, m_replacementNewRoot);

                                AnimationCurve curve = AnimationUtility.GetEditorCurve(animationClip, binding);
                                if (curve != null)
                                {
                                    AnimationUtility.SetEditorCurve(animationClip, binding, null);
                                    binding.path = sNewPath;
                                    AnimationUtility.SetEditorCurve(animationClip, binding, curve);
                                }
                                else
                                {
                                    ObjectReferenceKeyframe[] objectReferenceCurve =
                                        AnimationUtility.GetObjectReferenceCurve(animationClip, binding);
                                    AnimationUtility.SetObjectReferenceCurve(animationClip, binding, null);
                                    binding.path = sNewPath;
                                    AnimationUtility.SetObjectReferenceCurve(animationClip, binding,
                                        objectReferenceCurve);
                                }
                            }
                        }
                    }

                    // Update the progress meter
                    float fChunk = 1f / m_animationClips.Count;
                    fProgress = (iCurrentClip * fChunk) + fChunk * ((float) iCurrentPath / (float) m_pathsKeys.Count);

                    EditorUtility.DisplayProgressBar(
                        "Animation Hierarchy Progress",
                        "How far along the animation editing has progressed.",
                        fProgress);
                }
            }

            AssetDatabase.StopAssetEditing();
            EditorUtility.ClearProgressBar();

            FillModel();
            this.Repaint();
        }

        private void UpdatePath(string oldPath, string newPath)
        {
            if (m_paths[newPath] != null)
            {
                throw new UnityException("Path " + newPath + " already exists in that animation!");
            }

            AssetDatabase.StartAssetEditing();
            for (int iCurrentClip = 0; iCurrentClip < m_animationClips.Count; iCurrentClip++)
            {
                AnimationClip animationClip = m_animationClips[iCurrentClip];
                Undo.RecordObject(animationClip, "Animation Hierarchy Change");

                //recreating all curves one by one
                //to maintain proper order in the editor - 
                //slower than just removing old curve
                //and adding a corrected one, but it's more
                //user-friendly
                for (int iCurrentPath = 0; iCurrentPath < m_pathsKeys.Count; iCurrentPath++)
                {
                    string path = m_pathsKeys[iCurrentPath] as string;
                    ArrayList curves = (ArrayList) m_paths[path];

                    for (int i = 0; i < curves.Count; i++)
                    {
                        EditorCurveBinding binding = (EditorCurveBinding) curves[i];
                        AnimationCurve curve = AnimationUtility.GetEditorCurve(animationClip, binding);
                        ObjectReferenceKeyframe[] objectReferenceCurve =
                            AnimationUtility.GetObjectReferenceCurve(animationClip, binding);

                        if (curve != null)
                            AnimationUtility.SetEditorCurve(animationClip, binding, null);
                        else
                            AnimationUtility.SetObjectReferenceCurve(animationClip, binding, null);

                        if (path == oldPath)
                            binding.path = newPath;

                        if (curve != null)
                            AnimationUtility.SetEditorCurve(animationClip, binding, curve);
                        else
                            AnimationUtility.SetObjectReferenceCurve(animationClip, binding, objectReferenceCurve);

                        float fChunk = 1f / m_animationClips.Count;
                        float fProgress = (iCurrentClip * fChunk) +
                            fChunk * ((float) iCurrentPath / (float) m_pathsKeys.Count);

                        EditorUtility.DisplayProgressBar(
                            "Animation Hierarchy Progress",
                            "How far along the animation editing has progressed.",
                            fProgress);
                    }
                }
            }

            AssetDatabase.StopAssetEditing();
            EditorUtility.ClearProgressBar();
            FillModel();
            this.Repaint();
        }

        private GameObject FindObjectInRoot(string path)
        {
            if (m_animatorObject == null)
            {
                return null;
            }

            Transform child = m_animatorObject.transform.Find(path);

            if (child != null)
            {
                return child.gameObject;
            }
            else
            {
                return null;
            }
        }

        private string ChildPath(GameObject obj, bool sep = false)
        {
            if (m_animatorObject == null)
            {
                throw new UnityException("Please assign Referenced Animator (Root) first!");
            }

            if (obj == m_animatorObject.gameObject)
            {
                return "";
            }
            else
            {
                if (obj.transform.parent == null)
                {
                    throw new UnityException("Object must belong to " + m_animatorObject.ToString() + "!");
                }
                else
                {
                    return ChildPath(obj.transform.parent.gameObject, true) + obj.name + (sep ? "/" : "");
                }
            }
        }
    }
}