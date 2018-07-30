using System.Collections;
using System.Collections.Generic;
using UnityEngine.Advertisements;
using Assets.Scripts.Game;
using UnityEngine;


namespace Assets.Scripts.Monetization
{
    public class Advertising : MonoBehaviour
    {
        public static Advertising Instance
        {
            get; private set;
        }

        [SerializeField]
        private string androidGameId = "2655137";
        [SerializeField]
        private int adsTimesPlayedInterval = 2;
        [SerializeField]
        private float adsInitTimeLimit = 10.0f;

        private Coroutine showAdsCoroutine = null;

        private void Start()
        {
            Advertisement.Initialize(androidGameId);
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void TryToShowAds()
        {
            if (showAdsCoroutine != null)
            {
                StopCoroutine(showAdsCoroutine);
            }
            StartCoroutine(ShowAdsCoroutine());
        }

        private IEnumerator ShowAdsCoroutine()
        {
            while (!PersistentState.Ready)
                yield return null;

            var data = PersistentState.Instance.data;

            if (data.adsDisabled || data.timesPlayed - data.adsDisplayed < adsTimesPlayedInterval)
                yield break;

            Debug.Log("Waiting for ads initialization");
            float initTime = 0;
            while (!Advertisement.IsReady())
            {
                yield return null;
                initTime += Time.unscaledDeltaTime;
                if (initTime >= adsInitTimeLimit)
                {
                    Debug.Log("Ads disabled because init time reached limit");
                    yield break;
                }
            }
               
            Advertisement.Show();
            Debug.Log("Waiting for ads displayed");
            data.adsDisplayed = data.timesPlayed;
            showAdsCoroutine = null;
        }

    }

}

