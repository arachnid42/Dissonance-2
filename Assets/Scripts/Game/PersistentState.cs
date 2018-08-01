using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;

namespace Assets.Scripts.Game
{
    public class PersistentState : MonoBehaviour
    {
        [Serializable]
        public class Refs
        {
            public Difficulty configurableMode = null;
        }

        [Serializable]
        public class Temp
        {
            public Difficulty.Data configurableModeOriginalData = null;
            public string recentPurchase = null;
        }

        [Serializable]
        public class Config
        {
            public bool devMode = false;
            public int askRatingTimesPlayedInterval = 15;
            public int goodRatingMin = 4;
        }

        [Serializable]
        public class Data
        {
            [Serializable]
            public class Rating
            {
                public string version = null;
                public float rating = 0;
                public long timesPlayed = 0;
            }
            [Serializable]
            public class Tutorial
            {
                public bool basic = false;
                public bool lifeBonus = false;
                public bool freezeBonus = false;
                public bool explosionBonus = false;
            }

            public bool sound = false;
            public string colorPresetName = null;
            public int lastLevelIndex = 0;
            public int levelsUnlocked = 1;

            public bool themesUnlocked = false;
            public bool customModeUnlocked = false;
            public bool endlessModeUnlocked = false;
            public bool adsDisabled = false;

            //game over at which ads was displayed
            public long adsDisplayed = 0;
            //total number of times played
            public long timesPlayed = 0;

            public int endlessScoreRecord = 0;
            public int endlessTimeRecord = 0;

            public Rating rating = new Rating();
            public Tutorial turotiral = new Tutorial();
            public Difficulty.Data configurableModeData = null;

        }

        public static bool Ready
        {
            get { return Instance != null && Instance.isReady; }
        }

        public static PersistentState Instance
        {
            get; private set;
        }

        public string DataPath
        {
            get { return Path.Combine(Application.persistentDataPath, "persistentState.dat"); }
        }


        public Data data = new Data();
        public Config config = new Config();
        public Temp temp = new Temp();
        [SerializeField]
        private Refs refs = new Refs();

        private bool isReady = false;

       
        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Load();
            }
            else if(Instance != this)
            {
                Destroy(gameObject);
            }
        }


        private void OnApplicationQuit()
        {
            Save();
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                Save();
            }
        }

        public void Save()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(DataPath);
            data.configurableModeData = refs.configurableMode.GetData();
            bf.Serialize(file, data);
            file.Close();
        }

        public void ApplyConfigurableModeData()
        {
            refs.configurableMode.UpdateData(data.configurableModeData);
            data.configurableModeData = refs.configurableMode.GetData();
        }

        public void ResetConfigurableModeData()
        {
            refs.configurableMode.UpdateData(temp.configurableModeOriginalData);
            data.configurableModeData = refs.configurableMode.GetData();

        }

        private IEnumerator FixDataCoroutine()
        {
            yield return WaitForDifficultyLevelsInitialization();

            temp.configurableModeOriginalData = refs.configurableMode.GetData();
            data.levelsUnlocked = config.devMode ? DifficultyLevels.Instance.LevelCount : Mathf.Clamp(data.levelsUnlocked, 1, DifficultyLevels.Instance.LevelCount);
            if (data.configurableModeData != null)
            {
                //data.configurableModeData.bonusCatchSlowdown = new Difficulty.BonusCatchSlowdown();
                //data.configurableModeData.slowdown = new Difficulty.DefaultSlowdown();
                data.configurableModeData.freeze = new Difficulty.FreezeBonus().Update(data.configurableModeData.freeze);
                data.configurableModeData.explosion = new Difficulty.ExplosionBonus().Update(data.configurableModeData.explosion);
                data.configurableModeData.heart = new Difficulty.HeartBonus().Update(data.configurableModeData.heart);
                refs.configurableMode.UpdateData(data.configurableModeData);
            }
            else
            {
                data.configurableModeData = refs.configurableMode.GetData();
            }
                

            yield return WaitForColorsPresetsAndManagerInitialization();

            if(string.IsNullOrEmpty(data.colorPresetName))
            {
                data.colorPresetName = ColorsPresets.Instance.PresetName;
            }

            if(ColorsPresets.Instance.CurrentPreset.name != data.colorPresetName)
            {
                ColorsPresets.Instance.PresetName = data.colorPresetName;
            }
            ColorsPresetsManager.Instance.ApplyCurrentColorPreset();

            isReady = true;
        }

        private IEnumerator WaitForDifficultyLevelsInitialization()
        {
            while (DifficultyLevels.Instance == null)
                yield return null;
        }

        private IEnumerator WaitForColorsPresetsAndManagerInitialization()
        {
            while (!ColorsPresets.Ready || !ColorsPresetsManager.Ready)
                yield return null;
        }

        private IEnumerator SetColorsPresetsCoroutine(string name)
        {
            yield return WaitForColorsPresetsAndManagerInitialization();
            if (ColorsPresets.Instance.PresetName != name)
            {
                data.colorPresetName = name;
                ColorsPresets.Instance.PresetName = name;
                ColorsPresetsManager.Instance.ApplyCurrentColorPreset();
            }
        }

        public void SetColorsPreset(string name)
        {
            StartCoroutine(SetColorsPresetsCoroutine(name));
        }

        private void FixData()
        {
            StartCoroutine(FixDataCoroutine());
        }

        private void Load()
        {
            isReady = false;
            if (File.Exists(DataPath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(DataPath, FileMode.Open);
                try
                {
                    data = (Data)bf.Deserialize(file);
                }
                catch (System.Runtime.Serialization.SerializationException)
                {
                    data = new Data();
                }
                FixData();
            }
            else
            {
                data = new Data();
                FixData();
            }
        }

        public void SendRating(float rating, string comment)
        {
            data.rating = new Data.Rating()
            {
                rating = rating,
                version = Application.version
            };
            Save();
        }

        public void DeclineRating()
        {
            data.rating.timesPlayed = data.timesPlayed;
            Save();
        }

        public bool ShouldAskRating()
        {
            return ShouldShowRatingButton() && data.timesPlayed - data.rating.timesPlayed >= config.askRatingTimesPlayedInterval;
        }

        public bool ShouldShowRatingButton()
        {
            return data.rating.version != Application.version && data.rating.rating < config.goodRatingMin;
        }

    }


}
