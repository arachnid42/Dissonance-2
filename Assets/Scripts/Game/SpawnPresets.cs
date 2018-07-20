using UnityEngine;


namespace Assets.Scripts.Game
{
    public class SpawnPresets : MonoBehaviour
    {
        [SerializeField]
        private SpawnsPreset[] presets;

        public SpawnsPreset this[int index]
        {
            get
            {
                return presets[index];
            }
        } 
    }
}
