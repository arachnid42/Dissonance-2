using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Localization;
using Assets.Scripts.UI.Theme;
using Assets.Scripts.Game;


namespace Assets.Scripts.UI.Elements
{
    public enum ToggleBackgroundColors
    {
        background1,
        background2
    }

    public enum ToggleForegroundColors
    {
        tile1,
        tile2,
        tile3,
        tile4,
        tile5,
        tile6,
        tile7,
        tile8,
        tile9,
        tile10,
        foreground
    }

    public class Toggle : BaseThemeListener
    {
        [SerializeField]
        private string name = null;
        [SerializeField]
        private bool isOn, allowSwitchOff;

        [SerializeField]
        private Text titleText = null;
        [SerializeField]
        private Image backgroundImage, foregroundImage, contentImage;
        [SerializeField]
        private Sprite image = null;
        [SerializeField]
        private ToggleGroup toggleGroup = null;
        [SerializeField]
        private ToggleBackgroundColors toggleBackgroundColor;
        [SerializeField]
        private ToggleForegroundColors toggleForegroundColor;

        private Color backgroundColor, foregroundColor, backgroundOffColor, contentColor;

        private new void Start()
        {
            base.Start();
            if(toggleGroup != null)
            {
                toggleGroup.AddToggleToGroup(this);
            }
            Value = isOn;
        }

        private void OnEnable()
        {
            if(titleText != null)
                titleText.text = name !=null && name != ""? LocalizationManager.Instance[name]: "Title";
            Value = isOn;
        }

        public void OnToggleClick()
        {
            if(toggleGroup != null )
            {
                toggleGroup.OnToggleGroup(gameObject.name);
            }
            else
            {
                Value = !isOn;
            }
        }

        public bool Value
        {
            get { return isOn; }
            set {
                if (allowSwitchOff)
                {
                    isOn = value;
                }
                else
                {
                    isOn = true;
                }
                UpdateValue(); }
        }

        public string Name
        {
            get { return name; }
        }

        private void UpdateValue()
        {
            if (isOn)
            {
                backgroundImage.color = backgroundColor;
            }
            else
            {
                backgroundImage.color = backgroundOffColor;
            }
        }

        public override void OnApplyColorTheme(ColorsPreset preset)
        {
            backgroundColor = preset.uiColorPreset.toggleColor.GetBackgroundColorByName(toggleBackgroundColor);
            backgroundImage.color = backgroundColor;
            if(contentImage != null)
            {
                contentImage.sprite = image;
                int index = (int)toggleForegroundColor;
                if(index > 0 && index < preset.main.Length)
                {
                    foregroundImage.color = preset.main[(int)toggleForegroundColor];
                }
                else
                {
                    foregroundImage.color = preset.uiColorPreset.toggleColor.GetForegroundColorByName(toggleForegroundColor);
                }
                contentImage.color = preset.tileShapes;
            }
            else
            {
                foregroundImage.color = preset.uiColorPreset.toggleColor.GetForegroundColorByName(toggleForegroundColor);
            }
            backgroundOffColor = new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, 0);
        }
    }
}

    