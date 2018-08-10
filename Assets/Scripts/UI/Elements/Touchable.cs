using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Assets.Scripts.UI.Elements
{
    public class Touchable : Text
    {
        protected override void Awake()
        {
            base.Awake();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Touchable))]
    public class Touchable_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Do nothing
        }
    }
#endif
}
