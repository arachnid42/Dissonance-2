using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;

namespace Assets.Scripts.UI.Theme
{
    public class LogoChangeListener : BaseThemeListener
    {
        [SerializeField]
        private Image logo;

        public override void OnApplyColorTheme(UIColorsPreset preset)
        {
            logo.sprite = preset.logo;
        }
    }
}

    