using Assets.Scripts.Shape;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

namespace Assets.Scripts.Game
{
#if UNITY_EDITOR
    [CustomEditor(typeof(Difficulty)), CanEditMultipleObjects]
    public class DifficultyEditor : Editor
    {
        private static ShapeType[] additional = new ShapeType[]
        {
            ShapeType.Circle,
            ShapeType.Diamond,
            ShapeType.Pentagon,
            ShapeType.Pentagram,
            ShapeType.Hexagon,
            ShapeType.Hexagram,
            ShapeType.Octogram,
            ShapeType.Decagram
        };

        private static ShapeType[] basic = new ShapeType[]
        {
            ShapeType.Square,
            ShapeType.Triangle
        };

        private bool visible = false;
        private Difficulty difficulty = null;
        private SerializedProperty additionalShapes = null;
        public void Awake()
        {
            difficulty = (Difficulty)target;
            
        }


        private int IndexOf(ShapeType type)
        {
            for (int i = 0; i < additionalShapes.arraySize; i++)
                if (additionalShapes.GetArrayElementAtIndex(i).enumValueIndex == (int)type)
                    return i;
            return -1;
        }

        private bool Contains(ShapeType type)
        {
            return IndexOf(type) >= 0;
        }

        private void Remove(ShapeType type)
        {
            int index = IndexOf(type);
            if (index >= 0)
            {
                additionalShapes.DeleteArrayElementAtIndex(index);
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void Add(ShapeType type)
        {
            //Debug.Log("Add");
            if (IndexOf(type) < 0)
            {
                //Debug.Log("INDEX CHECK PASSED!");
                int index = additionalShapes.arraySize;
                additionalShapes.InsertArrayElementAtIndex(index);
                additionalShapes.GetArrayElementAtIndex(index).enumValueIndex = (int)type;
                serializedObject.ApplyModifiedProperties();
            }

        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            //GUILayout.Label(string.Format("IS ARRAY:{0} SIZE:{1}", additionalShapes.isArray, additionalShapes.arraySize));
            additionalShapes = serializedObject.FindProperty("additionalShapes");
            visible = EditorGUILayout.Foldout(visible, "Shape Types:" + (additionalShapes.arraySize + RotationDescriptor.basicTypes.Length), true);
            if (!visible)
                return;

            EditorGUI.indentLevel++;
            EditorGUI.BeginDisabledGroup(true);
            foreach (var type in basic)
            {
                GUILayout.Toggle(true, type.ToString());
            }
            EditorGUI.EndDisabledGroup();
            foreach (var type in additional)
            {

                bool value = Contains(type);
                if (GUILayout.Toggle(value, type.ToString()))
                {
                    if (!value)
                    {
                        Add(type);
                    }

                }
                else
                {
                    if (value)
                    {
                        Remove(type);
                    }

                }
            }
            EditorGUI.indentLevel--;
        }
    }

#endif
}
