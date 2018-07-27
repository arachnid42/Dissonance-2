using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Monetization;

namespace Assets.Scripts.UI.Panels
{
    public class DonateMenu : BasePanel
    {

        [SerializeField]
        private Text donate, back = null;

        [SerializeField]
        private Button disableAdsButton;

        private IEnumerator Start()
        {
            yield return WaitForStore();
            Purchases.Instance.OnSuccess += OnPurchaseSuccess;
        }

        private void OnEnable()
        {
            SetLabels(UpdateLabels);
            UIController.Instance.data.activePanel = this;
        }

        private void UpdateLabels()
        {
            donate.text = Text("donate")+" Test";
            back.text = Text("back");
        }

        private void OnPurchaseSuccess(string id)
        {
            if (id == Purchases.DISABLE_ADS)
            {
                disableAdsButton.gameObject.SetActive(false);
            }
        }

        private void OnPurchaseFail(string id)
        {

        }

        public void OnDisableAdsClick()
        {
            StartCoroutine(DisableAds());
        }

        private IEnumerator WaitForStore()
        {
            while (!Purchases.Ready)
                yield return null;
        }

        private IEnumerator DisableAds()
        {
            Debug.Log("Waiting for Purchases instance");
            yield return WaitForStore();
            Debug.Log("Call BuyDisableAds");
            Purchases.Instance.BuyDisableAds();
        }


    }
}

