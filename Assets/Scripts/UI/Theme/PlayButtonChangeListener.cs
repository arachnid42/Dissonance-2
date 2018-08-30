using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Theme
{
    public class PlayButtonChangeListener : BaseThemeListener
    {
        [SerializeField]
        private Button playButton;

        public override void OnApplyColorTheme(UIColorsPreset preset)
        {
            playButton.GetComponent<Image>().sprite = preset.playButton.normal;
            SpriteState spriteState = new SpriteState();
            spriteState = playButton.spriteState;
            spriteState.pressedSprite = preset.playButton.pressed;
            playButton.spriteState = spriteState;
        }
    }
}

    