using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using mixpanel;

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
            public string colorPresetName = null;
            public int lastLevelIndex = 0;
            public int levelsUnlocked = 1;

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
        [SerializeField]
        private Refs refs = new Refs();
        [SerializeField]
        private Temp temp = new Temp();


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
                refs.configurableMode.UpdateData(data.configurableModeData);
            else
                data.configurableModeData = refs.configurableMode.GetData();

            yield return WaitForColorsPresetsAndManagerInitialization();

            if(data.colorPresetName == null)
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
            var values = new Value();

            values["rating"] = rating;
            values["comment"] = comment;

            string name = rating < config.goodRatingMin ? "BAD_RATING" : "GOOD_RATING";
            Mixpanel.Track(name, values);
            data.rating = new Data.Rating()
            {
                rating = rating,
                version = Application.version
            };
            Save();
        }

        public bool ShouldAskRating()
        {
            return data.rating.version != Application.version && data.timesPlayed - data.rating.timesPlayed >= config.askRatingTimesPlayedInterval && data.rating.rating < config.goodRatingMin;
        }

    }


}
