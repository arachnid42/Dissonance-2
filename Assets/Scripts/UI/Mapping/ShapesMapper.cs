using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Assets.Scripts.Shape;
using Assets.Scripts.UI.Elements;


namespace Assets.Scripts.UI.Mapping
{
    public class ShapesMapper : MonoBehaviour
    {
        [SerializeField]
        private ToggleGroup toggleGroup;

        public List<ShapeType> GetData()
        {
            return GetValue();
        }

        public void SetData(List<ShapeType> additionalShapes)
        {
            SetValue(additionalShapes);
        }

        private List<ShapeType> GetValue()
        {
            List<ShapeType> shapes = new List<ShapeType>();
            foreach (Toggle toggle in toggleGroup.GetActiveToggles())
            {
                switch (toggle.gameObject.name)
                {
                    case "CircleToggle":
                        shapes.Add(ShapeType.Circle);
                        break;
                    case "DiamondToggle":
                        shapes.Add(ShapeType.Diamond);
                        break;
                    case "PentagonToggle":
                        shapes.Add(ShapeType.Pentagon);
                        break;
                    case "PentagramToggle":
                        shapes.Add(ShapeType.Pentagram);
                        break;
                    case "HexagonToggle":
                        shapes.Add(ShapeType.Hexagon);
                        break;
                    case "HexagramToggle":
                        shapes.Add(ShapeType.Hexagram);
                        break;
                    case "OctogramToggle":
                        shapes.Add(ShapeType.Octogram);
                        break;
                    case "DecagramToggle":
                        shapes.Add(ShapeType.Decagram);
                        break;
                }
            }

            return shapes;
        }

        private void SetValue(List<ShapeType> shapes)
        {
            toggleGroup.ResetValues();
            foreach(ShapeType shapeType in shapes)
            {
                switch (shapeType)
                {
                    case ShapeType.Circle:
                        toggleGroup.SetToggleValue("CircleToggle", true);
                        break;
                    case ShapeType.Diamond:
                        toggleGroup.SetToggleValue("DiamondToggle", true);
                        break;
                    case ShapeType.Pentagon:
                        toggleGroup.SetToggleValue("PentagonToggle", true);
                        break;
                    case ShapeType.Pentagram:
                        toggleGroup.SetToggleValue("PentagramToggle", true);
                        break;
                    case ShapeType.Hexagon:
                        toggleGroup.SetToggleValue("HexagonToggle", true);
                        break;
                    case ShapeType.Hexagram:
                        toggleGroup.SetToggleValue("HexagramToggle", true);
                        break;
                    case ShapeType.Octogram:
                        toggleGroup.SetToggleValue("OctogramToggle", true);
                        break;
                    case ShapeType.Decagram:
                        toggleGroup.SetToggleValue("DecagramToggle", true);
                        break;
                }
            }
        }
    }
}
