﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;
using Assets.Scripts.Sound;
using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.UI.Panels
{
    public class SoundOverlay : BasePanel
    {

        [SerializeField]
        private Text music, sounds = null;
        [SerializeField]
        private ToggleButton musicToggleButton, soundsToggleButton;

        private void OnEnable()
        {
            SetLabels(UpdateOverlayInformation);
            musicToggleButton.Value = PersistentState.Instance.data.sound;
            soundsToggleButton.Value = PersistentState.Instance.data.soundSFX;
        }

        private void OnDisable()
        {
            
        }

        private void UpdateOverlayInformation()
        {
            music.text = Text("music");
            sounds.text = Text("sounds");
        }

        public void OnToggleMusicClick()
        {
            musicToggleButton.Value = !PersistentState.Instance.data.sound;
            SoundsController.Instance.SetBackgroundActive(!PersistentState.Instance.data.sound);
        }

        public void OnToggleSoundsClick()
        {
            soundsToggleButton.Value = !PersistentState.Instance.data.soundSFX;
            SoundsController.Instance.SetSFXActive(!PersistentState.Instance.data.soundSFX);
        }

        public void OnCloseButton()
        {
            SetHidddenAnimation(true).Start();
            var soundButton = UIController.Instance.data.activePanel.gameObject.GetComponentInChildren<SoundSettingsButton>();
            if(soundButton != null)
                soundButton.UpdateSoundIcon();
        }
    }
}
