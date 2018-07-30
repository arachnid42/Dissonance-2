using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;
using Assets.Scripts.Monetization;

namespace Assets.Scripts.UI.Panels
{
    public class DonateMenu : BasePanel
    {

        [SerializeField]
        private Text donate, unlockAll, skipLevels, unlockEndless, unlockConfigurable, unlockThemes, disableAds, supportDevs, back = null;

        [SerializeField]
        private Button unlockAllButton, skipLevelsButton, unlockEndlessButton, unlockConfigurableButton, unlockThemesButton, disableAdsButton;

        private PersistentState.Data data;

        private IEnumerator Start()
        {
            yield return WaitForStore();
            Purchases.Instance.OnSuccess += OnPurchaseSuccess;
        }

        private void OnEnable()
        {
            UIController.Instance.data.activePanel = this;
            SetLabels(UpdateLabels);
            data = PersistentState.Instance.data;
            Debug.Log(data);
            UpdateButtons();
        }

        private void UpdateLabels()
        {
            donate.text = Text("donate");
            unlockAll.text = Text("unlockAll");
            skipLevels.text = Text("skipLevels");
            unlockEndless.text = Text("unlockEndless");
            unlockConfigurable.text = Text("unlockConfigurable");
            unlockThemes.text = Text("unlockThemes");
            disableAds.text = Text("removeAds");
            supportDevs.text = Text("supportDevs");
            back.text = Text("back");
        }

        private void UpdateButtons()
        {
            ToggleUnlockAllButton();
            ToggleSkipLevelsButton();
            ToggleUnlockEndlessButton();
            ToggleUnlockConfigurableButton();
            ToggleUnlockThemesButton();
            ToggleDisableAdsButton();
        }

        private void OnPurchaseSuccess(string id)
        {
            UpdateButtons();
            UIController.Instance.PanelController.PurchasePanel.SetHidddenAnimation(false).Start();
        }

        private void OnPurchaseFail(string id)
        {

        }

        public void OnUnlockAllClick()
        {
            StartCoroutine(Buy(Purchases.UNLOCK_ALL));
        }

        public void OnSkipLevelsClick()
        {
            StartCoroutine(Buy(Purchases.SKIP));
        }

        public void OnUnlockEndlessClick()
        {
            StartCoroutine(Buy(Purchases.UNLOCK_ENDLESS));
        }

        public void OnUnlockConfigurableClick()
        {
            StartCoroutine(Buy(Purchases.UNLOCK_CONFIGURABLE));
        }

        public void OnUnlockThemesClick()
        {
            StartCoroutine(Buy(Purchases.UNLOCK_ALL_THEMES));
        }

        public void OnDisableAdsClick()
        {
            StartCoroutine(Buy(Purchases.DISABLE_ADS));
        }

        public void OnSupportDevsClick()
        {
            StartCoroutine(Buy(Purchases.DONATE));
        }

        private IEnumerator WaitForStore()
        {
            while (!Purchases.Ready)
                yield return null;
        }

        private IEnumerator Buy(string id)
        {
            yield return WaitForStore();
            PersistentState.Instance.temp.recentPurchase = id;
            switch (id)
            {
                case Purchases.UNLOCK_ALL:
                    Purchases.Instance.BuyUnlockAll();
                    break;
                case Purchases.SKIP:
                    Purchases.Instance.BuySkipLevels();
                    break;
                case Purchases.UNLOCK_ENDLESS:
                    Purchases.Instance.BuyUnlockEndless();
                    break;
                case Purchases.UNLOCK_CONFIGURABLE:
                    Purchases.Instance.BuyUnlockConfigurable();
                    break;
                case Purchases.UNLOCK_ALL_THEMES:
                    Purchases.Instance.BuyUnlockAllThemes();
                    break;
                case Purchases.DISABLE_ADS:
                    Purchases.Instance.BuyDisableAds();
                    break;
                case Purchases.DONATE:
                    Purchases.Instance.BuyDonate();
                    break;
            }
        }

        private void ToggleUnlockAllButton()
        {
            if (data.themesUnlocked && data.customModeUnlocked && data.endlessModeUnlocked && data.adsDisabled)
                unlockAllButton.gameObject.SetActive(false);
            else
                unlockAllButton.gameObject.SetActive(true);
        }

        private void ToggleSkipLevelsButton()
        {
            if (data.levelsUnlocked == DifficultyLevels.Instance.LevelCount)
                skipLevelsButton.gameObject.SetActive(false);
            else
                skipLevelsButton.gameObject.SetActive(true);
        }

        private void ToggleUnlockEndlessButton()
        {
            if (data.endlessModeUnlocked)
                unlockEndlessButton.gameObject.SetActive(false);
            else
                unlockEndlessButton.gameObject.SetActive(true);
        }

        private void ToggleUnlockConfigurableButton()
        {
            if (data.customModeUnlocked)
                unlockConfigurableButton.gameObject.SetActive(false);
            else
                unlockConfigurableButton.gameObject.SetActive(true);
        }

        private void ToggleUnlockThemesButton()
        {
            if (data.themesUnlocked)
                unlockThemesButton.gameObject.SetActive(false);
            else
                unlockThemesButton.gameObject.SetActive(true);
        }

        private void ToggleDisableAdsButton()
        {
            if (data.adsDisabled)
                disableAdsButton.gameObject.SetActive(false);
            else
                disableAdsButton.gameObject.SetActive(true);
        }
    }
}

