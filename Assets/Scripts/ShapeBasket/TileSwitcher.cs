using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Sound;

namespace Assets.Scripts.ShapeBasket
{
    public class TileSwitcher : MonoBehaviour
    {
        [SerializeField]
        private BasketBuilder basket;
        private GameObject tile = null;
        private Coroutine resetCoroutine;

        public GameObject Tile
        {
            get
            {
                return tile;
            }
            set
            {
                if (tile == value)
                    return;
                tile = value;
                for(int i = 0; i < basket.Tiles.Length; i++)
                {
                    bool isEnabled = basket.Tiles[i] == value;
                    basket.TilesIndicators[i].GetComponent<BasketIndicator>().SetEnabled(isEnabled);
                }
                SoundsController.PlaySound(SoundsController.SoundSFX.TILE_TAP);
            }
        }

        private IEnumerator ResetCoroutine()
        {
            while (!basket.IsReady)
                yield return null;
            var tileIndex = Random.Range(0, basket.Tiles.Length);
            for(int i = 0; i < basket.TilesIndicators.Length; i++)
            {
                basket.TilesIndicators[i].GetComponent<BasketIndicator>().SetEnabled(i == tileIndex, true);
            }
            tile = basket.Tiles[tileIndex];
            resetCoroutine = null;
        }

        public void Reset()
        {
            if (resetCoroutine == null)
                resetCoroutine = StartCoroutine("ResetCoroutine");
        }

    }

}
