using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.UI.Theme;
using Assets.Scripts.Game;

namespace Assets.Scripts.UI.Panels
{
    public class Background : BasePanel
    {
        [SerializeField]
        private Image background;
        [SerializeField]
        private Sprite background916, background918;

        private void Start()
        {
            UIColorsPresets.Instance.OnUIColorPresetApply += SetupBackground;
        }
        
        private void SetupBackground(ColorsPreset preset)
        {
            background916 = Sprite.Create(preset.background916, new Rect(0.0f, 0.0f, preset.background916.width, preset.background916.height), new Vector2(0.5f, 0.5f), 100.0f);
            background918 = Sprite.Create(preset.background918, new Rect(0.0f, 0.0f, preset.background918.width, preset.background918.height), new Vector2(0.5f, 0.5f), 100.0f);
            StartCoroutine(SetupBackgroundCourotine());
        }

        private IEnumerator SetupBackgroundCourotine()
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

