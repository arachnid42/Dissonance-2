using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;

namespace Assets.Scripts.Localization
{
    public class LocalizationManager : MonoBehaviour
    {

        public const string NOT_FOUND_TEXT = "UNKNOWN LABEL";

        [Serializable]
        public class Listener
        {
            public TextMesh textMesh;
            public string key;
        }

        public static LocalizationManager Instance
        {
            get; private set;
        }

        [SerializeField]
        private Coroutine loadLocalizationFileCoroutine = null;

        private Dictionary<string, string> items;
        [SerializeField]
        private Listener[] listeners = null;

        [SerializeField]
        private string localization = "en";
        [SerializeField]
        private bool autodetection = true;
        public string Localization
        {
            get { return localization; }
            set
            {
                if (localization == value)
                    return;
                localization = value;
                if (loadLocalizationFileCoroutine != null)
                    StopCoroutine(loadLocalizationFileCoroutine);
                loadLocalizationFileCoroutine = StartCoroutine(LoadLocalizationFileCoroutine(GetPath(localization)));
            }
        }


        [Serializable]
        private class Data
        {
            [Serializable]
            public class Item
            {
                public string key = null, value = null;
            }
            public Item[] items = null;
            
            public Dictionary<string,string> ToDictionary()
            {
                var dict = new Dictionary<string, string>();
                foreach (var item in items)
                    dict[item.key] = item.value;
                return dict;
            }
        }


        private void UpdateListeners()
        {
            foreach (var listener in listeners)
            {
                string value = this[listener.key];
                listener.textMesh.text = value;
            }
        }

        private string GetPath(string locName)
        {
            string folder = Path.Combine(Application.streamingAssetsPath, "Localization");
            return Path.Combine(folder,locName + ".json");
        }

        private IEnumerator LoadLocalizationFileCoroutine(string localizationFilePath)
        {
            var request = UnityWebRequest.Get(localizationFilePath);
            yield return request.SendWebRequest();

            var text = request.downloadHandler.text.Trim();
            items = JsonUtility.FromJson<Data>(text).ToDictionary();

            UpdateListeners();
            loadLocalizationFileCoroutine = null;
        }

        private IEnumerator Start()
        {
            if (Instance == null)
            {
                yield return StartCoroutine(LoadLocalizationFileCoroutine(GetPath(localization)));
                Instance = this;
            } else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Awake()
        {
            if (!autodetection)
                return;
            switch (Application.systemLanguage)
            {
                case SystemLanguage.Russian:
                    localization = "ru";
                    break;
                case SystemLanguage.Ukrainian:
                    localization = "ua";
                    break;
                default:
                    localization = "en";
                    break;
            }
        }

        public String this[string key]
        {
            get
            {
                string value = null;
                if(items.TryGetValue(key, out value))
                {
                    return value;
                }
                else
                {
                    return NOT_FOUND_TEXT;
                }
            }
        }
    }
}
