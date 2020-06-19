using UnityEditor;
using UnityEngine;

namespace Innovecs.UnityHelpers.EditorClasses
{
    [CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
    public class EnumFlagsAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            property.intValue = EditorGUI.MaskField(rect, label, property.intValue, property.enumNames);
        }
    }
}