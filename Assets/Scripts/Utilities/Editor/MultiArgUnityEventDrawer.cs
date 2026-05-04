using UnityEditor;
using UnityEngine;
using JTUtility;

namespace JTUtility.Editor
{
    /// <summary>
    /// 多参数 UnityEvent 在 Inspector 中的绘制器。
    /// 使用 Unity 默认的 UnityEvent 绘制逻辑，保证在 Editor 中可展开、配置监听与参数。
    /// </summary>
    [CustomPropertyDrawer(typeof(MultiArgUnityEvents.IntIntUnityEvent), true)]
    [CustomPropertyDrawer(typeof(MultiArgUnityEvents.IntFloatUnityEvent), true)]
    [CustomPropertyDrawer(typeof(MultiArgUnityEvents.FloatFloatUnityEvent), true)]
    [CustomPropertyDrawer(typeof(MultiArgUnityEvents.StringIntUnityEvent), true)]
    [CustomPropertyDrawer(typeof(MultiArgUnityEvents.StringStringUnityEvent), true)]
    [CustomPropertyDrawer(typeof(MultiArgUnityEvents.StringFloatUnityEvent), true)]
    [CustomPropertyDrawer(typeof(MultiArgUnityEvents.BoolIntUnityEvent), true)]
    [CustomPropertyDrawer(typeof(MultiArgUnityEvents.BoolFloatUnityEvent), true)]
    [CustomPropertyDrawer(typeof(MultiArgUnityEvents.IntStringUnityEvent), true)]
    [CustomPropertyDrawer(typeof(MultiArgUnityEvents.GameObjectIntUnityEvent), true)]
    [CustomPropertyDrawer(typeof(MultiArgUnityEvents.GameObjectFloatUnityEvent), true)]
    [CustomPropertyDrawer(typeof(MultiArgUnityEvents.GameObjectStringUnityEvent), true)]
    [CustomPropertyDrawer(typeof(MultiArgUnityEvents.IntIntIntUnityEvent), true)]
    [CustomPropertyDrawer(typeof(MultiArgUnityEvents.IntIntFloatUnityEvent), true)]
    [CustomPropertyDrawer(typeof(MultiArgUnityEvents.IntFloatFloatUnityEvent), true)]
    [CustomPropertyDrawer(typeof(MultiArgUnityEvents.FloatFloatFloatUnityEvent), true)]
    [CustomPropertyDrawer(typeof(MultiArgUnityEvents.StringIntIntUnityEvent), true)]
    [CustomPropertyDrawer(typeof(MultiArgUnityEvents.StringIntFloatUnityEvent), true)]
    [CustomPropertyDrawer(typeof(MultiArgUnityEvents.StringStringIntUnityEvent), true)]
    [CustomPropertyDrawer(typeof(MultiArgUnityEvents.BoolIntIntUnityEvent), true)]
    [CustomPropertyDrawer(typeof(MultiArgUnityEvents.GameObjectIntIntUnityEvent), true)]
    [CustomPropertyDrawer(typeof(MultiArgUnityEvents.IntIntIntIntUnityEvent), true)]
    [CustomPropertyDrawer(typeof(MultiArgUnityEvents.IntIntFloatFloatUnityEvent), true)]
    [CustomPropertyDrawer(typeof(MultiArgUnityEvents.StringIntIntIntUnityEvent), true)]
    public class MultiArgUnityEventDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(position, property, label, true);
            EditorGUI.EndProperty();
        }
    }
}
