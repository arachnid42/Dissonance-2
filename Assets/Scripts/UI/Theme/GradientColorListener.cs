using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;

namespace Assets.Scripts.UI.Theme
{
    public enum GradientColors
    {
        gradient,
        modal,
        modalIconGradient,
        modalCloseButton
    }

    public class GradientColorListener : BaseThemeListener
    {
        [SerializeField]
        private Image gradient;
        [SerializeField]
        private GradientColors gradientColor;

        public override void OnApplyColorTheme(ColorsPreset preset)
        {
            var color = preset.uiColorPreset.gradientColor[gradientColor];
            color.a = gradient.color.a;
            gradient.color = color;
        }
    }
}

    