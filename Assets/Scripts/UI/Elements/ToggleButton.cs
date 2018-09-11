using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Localization;
using Assets.Scripts.UI.Theme;
using Assets.Scripts.Game;


namespace Assets.Scripts.UI.Elements
{

    public class ToggleButton : BaseThemeListener
    {
        [SerializeField]
        private bool value;
        [SerializeField]
        private string activeText, inactiveText;
        [SerializeField]
        private Image backgroundImage;
        [SerializeField]
        private Text toggleText;
        [SerializeField]
        private ButtonsColors toggleActiveColor, toggleInactiveColor;

        private Color activeColor, inactiveColor;

        private new void Start()
        {
            base.Start();
        }

        public void OnToggleClick()
        {
            Value = !value;
        }

        public bool Value
        {
            get { return value; }
            set {
                this.value = value;
                UpdateValue();
            }
        }

        private void UpdateValue()
        {
            if (value)
            {
                toggleText.text = LocalizationManager.Instance[activeText];
                backgroundImage.color = activeColor;
            }
            else
            {
                toggleText.text = LocalizationManager.Instance[inactiveText];
                backgroundImage.color = inactiveColor;
            }
        }

        public override void OnApplyColorTheme(ColorsPreset preset)
        {
            activeColor = preset.uiColorPreset.buttonsColor[toggleActiveColor];
            inactiveColor = preset.uiColorPreset.buttonsColor[toggleInactiveColor];
            UpdateValue();
        }
    }
}

    