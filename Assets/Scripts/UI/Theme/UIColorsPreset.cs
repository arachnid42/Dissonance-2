using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.UI.Theme
{
    public class UIColorsPreset : MonoBehaviour
    {
        [System.Serializable]
        public class ButtonsColor
        {
            public Color color1, color2, color3, color4, color5;
            public Color this[ButtonsColors color]
            {
                get { return GetColorByname(color); }
            }

            private Color GetColorByname(ButtonsColors color)
            {
                switch (color)
                {
                    case ButtonsColors.color1:
                        return color1;
                    case ButtonsColors.color2:
                        return color2;
                    case ButtonsColors.color3:
                        return color3;
                    case ButtonsColors.color4:
                        return color4;
                    case ButtonsColors.color5:
                        return color5;
                }
                return color3;
            }
        }

        [System.Serializable]
        public class PlayButtonSprites
        {
            public Sprite normal, pressed;
        }

        [System.Serializable]
        public class TextColor
        {
            public Color primary, secondary1, secondary2;
            public Color this[TextColors color]
            {
                get { return GetColorByname(color); }
            }

            private Color GetColorByname(TextColors color)
            {
                switch (color)
                {
                    case TextColors.primary:
                        return primary;
                    case TextColors.secondary1:
                        return secondary1;
                    case TextColors.secondary2:
                        return secondary2;
                }
                return primary;
            }
        }

        [System.Serializable]
        public class ToggleColor
        {
            public Color background1, background2, foreground;

            public Color GetBackgroundColorByName(ToggleBackgroundColors color)
            {
                switch (color)
                {
                    case ToggleBackgroundColors.background1:
                        return background1;
                    case ToggleBackgroundColors.background2:
                        return background2;
                }
                return background1;
            }

            public Color GetForegroundColorByName(ToggleForegroundColors color)
            {
                switch (color)
                {
                    case ToggleForegroundColors.foreground:
                        return foreground;
                }
                return foreground;
            }
        }

        [System.Serializable]
        public class SliderColor
        {
            public Color header, value, buttons;
        }

        [System.Serializable]
        public class SpoilerColor
        {
            public Color headerBG, contentBG, border, header, closedIcon, openedIcon;
        }

        [System.Serializable]
        public class GradientColor
        {
            public Color gradient, modal, modalIconGradient, modalCloseButton;
            public Color this[GradientColors color]
            {
                get { return GetColorByname(color); }
            }

            private Color GetColorByname(GradientColors color)
            {
                switch (color)
                {
                    case GradientColors.gradient:
                        return gradient;
                    case GradientColors.modal:
                        return modal;
                    case GradientColors.modalIconGradient:
                        return modalIconGradient;
                    case GradientColors.modalCloseButton:
                        return modalCloseButton;
                }
                return gradient;
            }
        }

        public Sprite logo;
        public PlayButtonSprites playButton = new PlayButtonSprites();
        public ButtonsColor buttonsColor = new ButtonsColor();
        public TextColor textColor = new TextColor();
        public ToggleColor toggleColor = new ToggleColor();
        public SliderColor sliderColor = new SliderColor();
        public SpoilerColor spoilerColor = new SpoilerColor();
        public GradientColor gradientColor = new GradientColor();

    }
}

    