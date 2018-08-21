using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;

namespace Assets.Scripts.UI.Panels
{
    public class Background : BasePanel
    {
        [SerializeField]
        private Image background;
        [SerializeField]
        private Sprite background916, background918;

        private void OnEnable()
        {
            StartCoroutine(SetupBackground());
        }

        private IEnumerator SetupBackground()
        {
            while (Field.Instance == null)
                yield return null;
            if (Field.Instance.Is918)
            {
                background.sprite = background918;
            }
            else
            {
                background.sprite = background916;
            }
        }
    }
}

