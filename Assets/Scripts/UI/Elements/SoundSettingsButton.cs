using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;

namespace Assets.Scripts.UI.Elements
{
    public class SoundSettingsButton : MonoBehaviour
    {
        [SerializeField]
        private Image soundImage;
        [SerializeField]
        private Sprite soundOnImage, soundOffImage;

        private void OnEnable()
        {
            UpdateSoundIcon();
        }

        public void UpdateSoundIcon()
        {
            if (PersistentState.Instance.data.sound || PersistentState.Instance.data.soundSFX)
                soundImage.sprite = soundOnImage;
            else
                soundImage.sprite = soundOffImage;
        }
    }
}

