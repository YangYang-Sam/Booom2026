using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
public class ConditionalFieldAttribute : PropertyAttribute
{
    public string FieldToCheck;
    public bool Inverse;

    public ConditionalFieldAttribute(string fieldToCheck, bool inverse = false)
    {
        FieldToCheck = fieldToCheck;
        Inverse = inverse;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ConditionalFieldAttribute))]
public class ConditionalFieldDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalFieldAttribute conditional = (ConditionalFieldAttribute)attribute;
        SerializedProperty targetProperty = property.serializedObject.FindProperty(conditional.FieldToCheck);

        if (targetProperty != null)
        {
            bool enabled = targetProperty.boolValue;
            if (conditional.Inverse) enabled = !enabled;

            if (enabled)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Error: Field not found");
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalFieldAttribute conditional = (ConditionalFieldAttribute)attribute;
        SerializedProperty targetProperty = property.serializedObject.FindProperty(conditional.FieldToCheck);

        if (targetProperty != null)
        {
            bool enabled = targetProperty.boolValue;
            if (conditional.Inverse) enabled = !enabled;

            if (enabled)
            {
                return EditorGUI.GetPropertyHeight(property, label, true);
            }
        }

        return 0f;
    }
}
#endif
