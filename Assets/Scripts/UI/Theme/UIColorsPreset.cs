using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        public Sprite background916, background918, logo;
        public Color gradient;
        public PlayButtonSprites playButton = new PlayButtonSprites();
        public ButtonsColor buttonsColor = new ButtonsColor();
        public TextColor textColor = new TextColor();


    }
}

    