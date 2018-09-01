using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;

namespace Assets.Scripts.UI.Theme
{
    public enum ButtonsColors
    {
        color1,
        color2,
        color3,
        color4,
        color5
    }

    public class ButtonColorListener : BaseThemeListener
    {
        [SerializeField]
        private Button button;
        [SerializeField]
        private ButtonsColors buttonColor;

        public override void OnApplyColorTheme(ColorsPreset preset)
        {
            button.GetComponent<Image>().color = preset.uiColorPreset.buttonsColor[buttonColor];
        }
    }
}

    