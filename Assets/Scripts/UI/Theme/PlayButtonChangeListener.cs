using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;

namespace Assets.Scripts.UI.Theme
{
    public class PlayButtonChangeListener : BaseThemeListener
    {
        [SerializeField]
        private Button playButton;

        public override void OnApplyColorTheme(ColorsPreset preset)
        {
            playButton.GetComponent<Image>().sprite = preset.uiColorPreset.playButton.normal;
            SpriteState spriteState = new SpriteState();
            spriteState = playButton.spriteState;
            spriteState.pressedSprite = preset.uiColorPreset.playButton.pressed;
            playButton.spriteState = spriteState;
        }
    }
}

    