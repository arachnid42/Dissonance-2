using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Game;
using System.Linq;

namespace Assets.Scripts.Shape
{
    public class Colors : MonoBehaviour
    {
        public void Start()
        {

            var mr = GetComponentInChildren<MeshRenderer>();
            List<Material> available = new List<Material>();
            Material[] materials = new Material[mr.sharedMaterials.Length];
            int numberOfColors = RotationDescriptor.basicTypes.Length + DifficultyLevels.Instance.CurrentDifficulty.additionalShapes.Count;
            for (int i = 0; i < materials.Length; i++)
            {
                if (available.Count == 0)
                {
                    available.AddRange(ColorsPresetsManager.Instance.Materials.main.Take(numberOfColors));
                }
                int selected = Random.Range(0, available.Count);
                materials[i] = available[selected];
                available.RemoveAt(selected);
            }

            mr.sharedMaterials = materials;
        }
        
    }
}

