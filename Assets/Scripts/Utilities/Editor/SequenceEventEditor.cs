using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace JTUtility.Editor
{
    [CustomEditor(typeof(SequenceEvent))]

    public class SequenceEventEditor : UnityEditor.Editor
    {
        private const float TopPadding = 2f;

        private static readonly string[] SequenceDetailPropertyNames =
        {

            "Label", "BeforeTriggerDelay", "AfterTriggerDelay", "luaCodes", "Event"

        };



        private ReorderableList list;



        private void OnEnable()

        {

            list = new ReorderableList(serializedObject,

                serializedObject.FindProperty("sequences"),

                true, true, true, true);



            list.elementHeightCallback = index =>

            {

                var element = list.serializedProperty.GetArrayElementAtIndex(index);

                return GetSequenceElementHeight(element);

            };



            list.drawElementCallback =

                (rect, index, isActive, isFocused) =>

                {

                    var element = list.serializedProperty.GetArrayElementAtIndex(index);

                    DrawSequenceElement(rect, element);

                };

        }



        private static float GetSequenceElementHeight(SerializedProperty element)

        {

            float spacing = EditorGUIUtility.standardVerticalSpacing;

            float h = TopPadding;

            h += EditorGUI.GetPropertyHeight(element, GUIContent.none, false);

            if (!element.isExpanded)

                return h;



            for (int i = 0; i < SequenceDetailPropertyNames.Length; i++)

            {

                h += spacing;

                h += EditorGUI.GetPropertyHeight(

                    element.FindPropertyRelative(SequenceDetailPropertyNames[i]), GUIContent.none, true);

            }



            return h;

        }



        private static void DrawSequenceElement(Rect rect, SerializedProperty element)

        {

            float spacing = EditorGUIUtility.standardVerticalSpacing;

            var drawRect = rect;

            drawRect.x += 10;

            drawRect.y += TopPadding;

            drawRect.height = EditorGUI.GetPropertyHeight(element, GUIContent.none, false);

            EditorGUI.PropertyField(drawRect, element, false);



            if (!element.isExpanded)

                return;



            drawRect.x -= 10;

            drawRect.y += drawRect.height + spacing;



            for (int i = 0; i < SequenceDetailPropertyNames.Length; i++)

            {

                var p = element.FindPropertyRelative(SequenceDetailPropertyNames[i]);

                drawRect.height = EditorGUI.GetPropertyHeight(p, GUIContent.none, true);

                EditorGUI.PropertyField(drawRect, p, true);

                drawRect.y += drawRect.height;

                if (i < SequenceDetailPropertyNames.Length - 1)

                    drawRect.y += spacing;

            }

        }



        public override void OnInspectorGUI()

        {

            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("startAtBeginning"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("manuallyExecute"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("canRepeat"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("unscaleTime"));

            list.DoLayoutList();

            serializedObject.ApplyModifiedProperties();

        }

    }

}


