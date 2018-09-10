using UnityEngine;
using System.Collections;
using Assets.Scripts.Game;
using System;

namespace Assets.Scripts.Sound
{
    public class SoundsController: MonoBehaviour
    {
        public enum SoundSFX
        {
            SHAPE_CATCH = 1,
            TILE_TAP = 2,
            WRONG_SHAPE_CATCH = 3,
            GAME_WON = 4
        }

        public static void PlaySound(SoundSFX id, float delay = 0)
        {
            if (Instance != null)
                Instance.PlaySoundSFX(id,delay);
        }

        public static SoundsController Instance
        {
            get;private set;
        }

        [SerializeField]
        private AudioSource background = null;
        private int shapeCatchNextIndex = 0;
        [SerializeField]
        private AudioSource[] shapeCatch = null;
        [SerializeField]
        private AudioSource tileTap = null;
        [SerializeField]
        private AudioSource wrongShapeCatch = null;
        [SerializeField]
        private AudioSource gameWon = null;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }else if(Instance != this)
            {
                Destroy(gameObject);
            }
        }


        private IEnumerator Start()
        {
            if (!PersistentState.Ready)
                yield return null;
            if (PersistentState.Instance.data.sound)
            {
                background.Play();
            }
        }

        private void SetShapeCatchNextIndex()
        {
            if (shapeCatchNextIndex + 1 >= shapeCatch.Length)
                shapeCatchNextIndex = 0;
            else
                shapeCatchNextIndex++;
        }

        public void SetBackgroundActive(bool active)
        {
            PersistentState.Instance.data.sound = active;
            if (active)
            {
                background.Play();
            }
            else
            {
                background.Stop();
            }
        }

        public void SetSFXActive(bool active)
        {
            PersistentState.Instance.data.soundSFX = false;
        }

        public void PlaySoundSFX(SoundSFX id, float delay = 0)
        {
            if (!PersistentState.Instance.data.soundSFX)
                return;
            AudioSource source = null;
            switch (id)
            {
                case SoundSFX.SHAPE_CATCH:
                    source = shapeCatch[shapeCatchNextIndex];
                    SetShapeCatchNextIndex();
                    break;
                case SoundSFX.WRONG_SHAPE_CATCH:
                    source = wrongShapeCatch;
                    break;
                case SoundSFX.TILE_TAP:
                    source = tileTap;
                    break;
                case SoundSFX.GAME_WON:
                    source = gameWon;
                    break;
            }
            if (source != null)
                source.PlayDelayed(delay);
        }


    }
}
