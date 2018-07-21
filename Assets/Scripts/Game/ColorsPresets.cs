using UnityEngine;

namespace Assets.Scripts.Game
{
    public class ColorsPresets: MonoBehaviour
    {

        private static ColorsPresets instance = null;
        public static ColorsPresets Instance
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

        [SerializeField]
        private string presetName = "Default";
        private ColorsPreset currentPreset = null;
        private bool isReady = false;

        public bool IsReady
        {
            get { return isReady; }
        }

        public ColorsPreset CurrentPreset
        {
            get { return currentPreset; }
            set { currentPreset = value; }
        }

        public string PresetName
        {
            get { return currentPreset.name; }
            set { currentPreset = GetPresetByName(value); }
        }

        public ColorsPreset this[int index]
        {
            get { return transform.GetChild(index).GetComponent<ColorsPreset>(); }
        }

        public int PresetCount
        {
            get { return transform.childCount; }
        }

        private ColorsPreset GetPresetByName(string name)
        {
            return transform.Find(name).GetComponent<ColorsPreset>();
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                currentPreset = GetPresetByName(presetName);
                isReady = true;
            }else if(instance != this)
            {
                Destroy(gameObject);
            }

        }
    }
}
