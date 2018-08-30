using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        public override void OnApplyColorTheme(UIColorsPreset preset)
        {
            text.color = preset.textColor[textColor];
        }
    }
}

    