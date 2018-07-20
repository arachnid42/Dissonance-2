using System.Collections;
using System.Collections.Generic;
using UnityEngine.Advertisements;
using Assets.Scripts.Game;
using UnityEngine;


namespace Assets.Scripts.Monetization
{
    public class Advertising : MonoBehaviour
    {
        [SerializeField]
        private string androidGameId = "2655137";
        [SerializeField]
        private int adsTimesPlayedInterval = 2;

        private IEnumerator Start()
        {
            while (Field.Instance == null || Field.Instance.Master == null || Field.Instance.Master.Listeners == null)
            {
                yield return null;
            }
            Field.Instance.Master.Listeners.OnGameOver += TryToShowAds;
            Advertisement.Initialize(androidGameId);
        }


        private void TryToShowAds(bool win)
        {
            if (ShouldShowAds())
            {
                ShowAds();
            }
        }

        private void ShowAds()
        {
            var data = PersistentState.Instance.data;
            data.adsDisplayed = data.timesPlayed;
            StartCoroutine(ShowAdsEnumerator());
        }


        private bool ShouldShowAds()
        {
            var data = PersistentState.Instance.data;
            return !data.adsDisabled && data.timesPlayed - data.adsDisplayed >= adsTimesPlayedInterval;
        }

        private IEnumerator ShowAdsEnumerator()
        {
            while (!Advertisement.IsReady())
                yield return null;
            Advertisement.Show();
        }

    }

}

