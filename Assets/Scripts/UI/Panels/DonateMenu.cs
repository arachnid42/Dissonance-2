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
        private Text donate = null, 
            back = null;

        private void OnEnable()
        {
            SetLabels(UpdateLabels);
            UIController.Instance.data.activePanel = this;
        }

        private void UpdateLabels()
        {
            donate.text = Text("donate");
            back.text = Text("back");
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

