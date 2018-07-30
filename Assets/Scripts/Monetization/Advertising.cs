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
        private bool ignoreAdsDisabled = false;
        [SerializeField]
        private string gameId = "2655137";
        [SerializeField]
        private int adsTimesPlayedInterval = 2;
        [SerializeField]
        private float adsInitTimeLimit = 10.0f;
        private float coroutineStepTimeInterval = 0.1f;
        private Coroutine showAdsCoroutine = null;

        private void Start()
        {
            Advertisement.Initialize(gameId);
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

            if ((data.adsDisabled && !ignoreAdsDisabled) || data.timesPlayed - data.adsDisplayed < adsTimesPlayedInterval)
            {
                Debug.Log("Ads disabled. Ignoring request");
                yield break;
            }
                

            Debug.Log("Waiting for ads initialization");
            float initTime = 0;
            Debug.Log("Adsvertisment.IsInitialized:" + Advertisement.isInitialized);
            Debug.LogFormat("Placement state:{0}", Advertisement.GetPlacementState());
            while (!Advertisement.IsReady())
            {
                
                yield return new WaitForSecondsRealtime(coroutineStepTimeInterval); ;
                if (initTime >= adsInitTimeLimit)
                {
                    Debug.Log("Ads disabled because init time reached limit");
                    yield break;
                }
                if(Advertisement.GetPlacementState() == PlacementState.NoFill)
                {
                    Debug.Log("Placement has no more ads to show");
                    yield break;
                }
                initTime += coroutineStepTimeInterval;
            }
               
            Advertisement.Show();
            Debug.Log("Waiting for ads displayed");
            data.adsDisplayed = data.timesPlayed;
            showAdsCoroutine = null;
        }

    }

}

