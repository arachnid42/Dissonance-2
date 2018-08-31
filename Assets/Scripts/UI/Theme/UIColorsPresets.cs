using UnityEngine;
using Assets.Scripts.Game;
using System.Collections;

namespace Assets.Scripts.UI.Theme
{
    public class UIColorsPresets: MonoBehaviour
    {
        public System.Action<ColorsPreset> OnUIColorPresetApply = (preset) => { Debug.LogFormat("OnUIColorPresetApply {0}", preset.name); };
        private static UIColorsPresets instance = null;
        public static UIColorsPresets Instance
        {
            get { return instance; }
        }

        public static bool Ready
        {
            get
            {
                return Instance != null && Instance.IsReady;
            }
        }

        private bool isReady = false;

        public bool IsReady
        {
            get { return isReady; }
        }

        public void ApplyUIColorPreset()
        {
            ColorsPreset preset = ColorsPresets.Instance.CurrentPreset;
            Debug.LogFormat("APPLYING UI COLOR PRESET {0}", preset.name);
            OnUIColorPresetApply(preset);
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                //DontDestroyOnLoad(gameObject);
                isReady = true;
            }else if(instance != this)
            {
                Destroy(gameObject);
            }

        }
    }
}
