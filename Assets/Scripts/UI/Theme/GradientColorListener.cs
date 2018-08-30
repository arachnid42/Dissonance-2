using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;

namespace Assets.Scripts.UI.Theme
{
    public class GradientColorListener : BaseThemeListener
    {
        [SerializeField]
        private Image gradient;

        public override void OnApplyColorTheme(UIColorsPreset preset)
        {
            gradient.color = preset.gradient;
        }
    }
}

    