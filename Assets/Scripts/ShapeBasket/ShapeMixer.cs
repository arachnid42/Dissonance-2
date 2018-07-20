using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Shape;
using System.Linq;
using Assets.Scripts.Game;

namespace Assets.Scripts.ShapeBasket
{
    public class ShapeMixer : MonoBehaviour
    {
        [SerializeField]
        private BasketBuilder basket;
        [SerializeField]
        private GameObject[] shapes;
        private ShapeType[] shapesTypes = new ShapeType[0];
        private List<GameObject> availableShapes = null;

        public ShapeType[] ShapeTypes
        {
            get { return shapesTypes; }
            set
            {
                shapesTypes = value;
                availableShapes = null;
                tileToShape = null;
            }
        }

        private Coroutine mixCoroutine;
        public GameObject[] Tiles
        {
            get { return basket.Tiles; }
        }


        public Dictionary<GameObject, GameObject> tileToShape = null;


        private void UpdateAvailableShapesIfNeeded()
        {
            if (availableShapes != null)
                return;
            List<GameObject> newAvailableShapes = new List<GameObject>(shapesTypes.Length);
            foreach (var shape in shapes)
            {
                if (shapesTypes.Contains(shape.GetComponent<ShapeToTile>().shapeType))
                {
                    newAvailableShapes.Add(shape);
                    shape.SetActive(true);
                }
                else
                {
                    shape.SetActive(false);
                }
            }
            availableShapes = newAvailableShapes;
        }

        private Dictionary<GameObject, GameObject> GetRandomizedTileToShapeMapping()
        {
            Dictionary<GameObject, GameObject> newTileToShape = new Dictionary<GameObject, GameObject>();
            UpdateAvailableShapesIfNeeded();
            var availableShapesCopy = new List<GameObject>(availableShapes);
            int selectedShapeIndex = -1;
            foreach (GameObject tile in Tiles)
            {
                selectedShapeIndex = Random.Range(0, availableShapesCopy.Count);
                newTileToShape[tile] = availableShapesCopy[selectedShapeIndex];
                availableShapesCopy.RemoveAt(selectedShapeIndex);
            }
            return newTileToShape;
        }


        private bool IsMappingEqualToExisting(Dictionary<GameObject, GameObject> newShapeToTile)
        {
            if(tileToShape == null)
            {
                return false;
            }

            foreach(GameObject tile in Tiles)
            {
                if (tileToShape[tile] != newShapeToTile[tile])
                {
                    return false;
                }
            }
            return true;
        }

        

        private IEnumerator MixCoroutine(bool immidiate)
        {
            while (!basket.IsReady)
                yield return null;
            Dictionary<GameObject, GameObject> newTileToShape = null;
            do
            {
                newTileToShape = GetRandomizedTileToShapeMapping();
            } while (IsMappingEqualToExisting(newTileToShape));
            foreach (GameObject tile in Tiles)
            {
                newTileToShape[tile].GetComponent<ShapeToTile>().SetTile(tile,immidiate);
            }
            tileToShape = newTileToShape;
            mixCoroutine = null;
        }

        public void Mix(bool immediate = false)
        {
            if (mixCoroutine == null)
            {
                var coroutine = MixCoroutine(immediate);
                mixCoroutine = StartCoroutine(coroutine);
            }
                
        }

        private IEnumerator Start()
        {
            while (!ColorsPresetsManager.Ready)
                yield return null;
            ApplyColorPreset();
        }

        private void OnEnable()
        {
            if (ColorsPresetsManager.Ready)
                ApplyColorPreset();
        }

        private void ApplyColorPreset()
        {
            foreach (var shape in shapes)
                shape.GetComponent<MeshRenderer>().sharedMaterial = ColorsPresetsManager.Instance.Materials.tileShapes;
        }

    }
}

