using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;

namespace Assets.Scripts.UI.Theme
{
    public enum TextColors
    {
        primary,
        secondary1,
        secondary2
    }

    public class TextColorListener : BaseThemeListener
    {
        [SerializeField]
        private Text text;
        [SerializeField]
        private TextColors textColor;

        public override void OnApplyColorTheme(ColorsPreset preset)
        {
            text.color = preset.uiColorPreset.textColor[textColor];
        }
    }
}

    