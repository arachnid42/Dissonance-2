using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using Assets.Scripts.Monetization;

namespace Assets.Scripts.UI.Panels
{
    public class Overlay : BasePanel
    {
        [SerializeField]
        private Text text, message, watchAds, unlock = null;
        [SerializeField]
        private GameObject messagePanel, trialButton;

        private Animation closeAnimation, toMainMenu, toDonateMenu;

        private const string ENDLESS_PANEL = "EndlessPanel";
        private const string CONFIGURABLE_PANEL = "ConfigurablePanel";
        private const string GAMEOVER_PANEL = "GameOverPanel";
        private const string GAMEOVERWIN_PANEL = "GameOverWinPanel";


        private void OnEnable()
        {
            SetLabels(UpdateOverlayInformation);
            ReactOnTrial();
            InitAnimations();
        }

        private void OnDisable()
        {

        }

        private void InitAnimations()
        {
            var uiController = UIController.Instance;
            closeAnimation = SetHidddenAnimation(true);
            toMainMenu = SetHidddenAnimation(true).After(uiController.data.activePanel.SetHiddenEnumerator(true)).After(uiController.PanelController.MainMenuPanel.SetHiddenEnumerator(false));
            toDonateMenu = SetHidddenAnimation(true).After(uiController.data.activePanel.SetHiddenEnumerator(true)).After(uiController.PanelController.DonatePanel.SetHiddenEnumerator(false));
        }

        private void ReactOnTrial()
        {
            if (!IsNetworkAvailable())
            {
                ShowNetworkUnavailable();
            }else if(IsAdsAvailable()){
                ShowPromo();
            }
            else
            {
                ShowAdsUnavailable();
            }
        }

        private void ShowNetworkUnavailable()
        {
            trialButton.SetActive(false);
            messagePanel.SetActive(true);
            message.text = Text("checkNetwork");
        }

        private void ShowPromo()
        {
            trialButton.SetActive(true);
            messagePanel.SetActive(true);
            message.text = Text("watchAds");
        }

        private void ShowAdsUnavailable()
        {
            trialButton.SetActive(false);
            messagePanel.SetActive(true);
            message.text = Text("adsUnavailable");
        }

        private void ShowAdsSkipped()
        {
            trialButton.SetActive(true);
            messagePanel.SetActive(true);
            message.text = Text("adsSkipped");
        }

        private void ShowAdsFailed()
        {
            trialButton.SetActive(true);
            messagePanel.SetActive(true);
            message.text = Text("adsFailed");
        }

        private bool IsNetworkAvailable()
        {
            return !(Application.internetReachability == NetworkReachability.NotReachable);
        }

        private bool IsAdsAvailable()
        {
            return !(Advertisement.GetPlacementState() == PlacementState.NoFill);
        }

        private void UpdateOverlayInformation()
        {
            text.text = Text("winOrBuyToUnlock");
            watchAds.text = Text("watchAdsButton");
            unlock.text = Text("unlock");
        }

        public void OnCloseButton()
        {
            toMainMenu.Start();
        }

        public void OnFunctionalButtonClick()
        {
            toDonateMenu.Start();
            UIController.Instance.data.IsTrial = false;
        }

        public void OnTryButtonClick()
        {
            Advertising.Instance.TryToShowAds(TrialAccessCallback);
        }

        private void TrialAccessCallback(ShowResult result)
        {
            switch (result)
            {
                case ShowResult.Finished:
                    Debug.Log("The ad was successfully shown.");
                    switch (UIController.Instance.data.activePanel.name)
                    {
                        case ENDLESS_PANEL:
                        case CONFIGURABLE_PANEL:
                            closeAnimation.Start();
                            break;
                        case GAMEOVER_PANEL:
                            closeAnimation.Start();
                            UIController.Instance.data.isAnimationPlays = false;
                            UIController.Instance.PanelController.gameOverPanel.GetComponent<GameOver>().ReplayAction();
                            break;
                        case GAMEOVERWIN_PANEL:
                            closeAnimation.Start();
                            UIController.Instance.data.isAnimationPlays = false;
                            UIController.Instance.PanelController.mainMenuPanel.GetComponent<MainMenu>().StartGame();
                            break;
                        default:
                            closeAnimation.Start();
                            break;
                    }
                    closeAnimation.Start();
                    UIController.Instance.data.IsTrial = true;
                    break;
                case ShowResult.Skipped:
                    Debug.Log("The ad was skipped before reaching the end.");
                    ShowAdsSkipped();
                    break;
                case ShowResult.Failed:
                    Debug.LogError("The ad failed to be shown.");
                    ShowAdsFailed();
                    break;
            }
        }
    }
}

