using UnityEngine;
using Assets.Scripts.Shape;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Game
{
    public class ShapesPreset : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> shapes = new List<GameObject>();
        [SerializeField]
        private List<GameObject> filteredShapes = new List<GameObject>();
        public ShapeType[] types;

        private void Awake()
        {
            foreach(Transform shapeTransform in transform)
            {
                shapes.Add(shapeTransform.gameObject);
            }
        }

        public void Filter(ShapeType[] types)
        {
            bool add = true;
            types = types.Distinct().ToArray();
            filteredShapes.Clear();
            types = types.Except(RotationDescriptor.basicTypes).ToArray();
            foreach (var shape in shapes)
            {
                add = true;
                var rotations = shape.GetComponent<RandomRotation>().randomRotations;
                foreach(var rotation in rotations)
                {
                    if (rotation.IsBasic)
                        continue;
                    if (types.Contains(rotation.type))
                        continue;
                    else
                    {
                        add = false;
                        break;
                    }
                }
                if (add)
                    filteredShapes.Add(shape);
            }
            this.types = types.Union(RotationDescriptor.basicTypes).ToArray();
            
        }

        public GameObject this[int index]
        {
            get
            {
                return filteredShapes[index];
            }
        }


        public int Length
        {
            get
            {
                return filteredShapes.Count;
            }
        }
    }
}
