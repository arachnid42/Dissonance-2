using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Game;
using System.Linq;

namespace Assets.Scripts.ShapeBasket
{
    public class ColorsRandomizer : MonoBehaviour
    {
        [SerializeField]
        private BasketBuilder basket;
        [SerializeField]
        private float time = 0.25f;

        private Coroutine shuffleColorsCoroutine = null;
        private Color[] currentTileColors;
        public GameObject[] Tiles
        {
            get { return basket.Tiles; }
        }

        public void StartSmoothColorRandomization(float stage = 0)
        {
            if (shuffleColorsCoroutine != null)
                StopCoroutine(shuffleColorsCoroutine);
            shuffleColorsCoroutine = StartCoroutine(ShuffleColorsCoroutine(stage));
        }

        public void Reset()
        {
            StartSmoothColorRandomization(stage: 1);
        }

        private IEnumerator Start()
        {
            while (!basket.IsReady)
                yield return null;
            Reset();
        }


        private bool AreColorsEqualWithCurrentColors(List<Color> newColors)
        {
            for(int i = 0; i < currentTileColors.Length; i++)
            {
                if(currentTileColors[i] != newColors[i])
                {
                    return false;
                }
            }
            return true;
        }
        
        private List<Color> GetRandomizedColors()
        {
            List<Color> availableColors = new List<Color>(ColorsPresetsManager.Instance.CurrentPreset.main.Take(Tiles.Length));
            List<Color> randomizedColors = new List<Color>(availableColors.Count);
            for(int i = 0; i < randomizedColors.Capacity; i++)
            {
                int selectedColorIndex = Random.Range(0, availableColors.Count);
                randomizedColors.Add(availableColors[selectedColorIndex]);
                availableColors.RemoveAt(selectedColorIndex);
            }
            return randomizedColors;  
        }


        private IEnumerator ShuffleColorsCoroutine(float stage = 0)
        {
            if (!basket.IsReady)
                yield return null;

            Color[] start;
            List<Color> end;
            MeshRenderer[] tileRenderers = (from tile in Tiles select tile.GetComponent<MeshRenderer>()).ToArray();

            start = (from r in tileRenderers select r.sharedMaterial.color).ToArray();
            currentTileColors = currentTileColors ?? start;

            do
            {
                end = GetRandomizedColors();
            } while (AreColorsEqualWithCurrentColors(end));

            currentTileColors = end.ToArray();

            do
            {
                stage += Time.unscaledDeltaTime / time;
                for (int i = 0; i < start.Length; i++)
                    tileRenderers[i].sharedMaterial.color = Color.Lerp(start[i], end[i], stage);
                yield return null;
            } while (stage < 1);

            shuffleColorsCoroutine = null;
        }

    }
}

