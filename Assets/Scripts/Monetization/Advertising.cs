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
            StartCoroutine(ShowAdsCoroutine());
        }

        private IEnumerator ShowAdsCoroutine()
        {
            while (!PersistentState.Ready)
                yield return null;

            var data = PersistentState.Instance.data;

            if (data.adsDisabled || data.timesPlayed - data.adsDisplayed < adsTimesPlayedInterval)
                yield break;

            while (!Advertisement.IsReady())
                yield return null;
            Advertisement.Show();
            data.adsDisplayed = data.timesPlayed;
        }

    }

}

