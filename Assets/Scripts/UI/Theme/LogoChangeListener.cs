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

        public override void OnApplyColorTheme(ColorsPreset preset)
        {
            logo.sprite = preset.uiColorPreset.logo;
        }
    }
}

    