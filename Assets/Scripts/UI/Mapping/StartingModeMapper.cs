using UnityEngine;
using UnityEditor;
using Assets.Scripts.UI.Elements;
using Assets.Scripts.Indicator;

namespace Assets.Scripts.UI.Mapping
{
    public class StartingModeMapper : MonoBehaviour
    {
        [SerializeField]
        private ToggleGroup toggleGroup;

        public GameMode GetData()
        {
            return GetValue();
        }

        public void SetData(GameMode gameMode)
        {
            SetValue(gameMode);
        }

        private GameMode GetValue()
        {
            foreach (Toggle toggle in toggleGroup.GetActiveToggles())
            {
                if (toggle.gameObject.name == "ShapeToggle")
                    return GameMode.Shape;
                else if (toggle.gameObject.name == "ColorToggle")
                    return GameMode.Color;
                else
                    return GameMode.None;
            }

            return GameMode.None;
        }

        private void SetValue(GameMode gameMode)
        {
            switch (gameMode)
            {
                case GameMode.Shape:
                    toggleGroup.OnToggleGroup("ShapeToggle");
                    break;
                case GameMode.Color:
                    toggleGroup.OnToggleGroup("ColorToggle");
                    break;
                default:
                    toggleGroup.OnToggleGroup("RandomToggle");
                    break;
            }
        }
    }
}
