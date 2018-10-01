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
        private Text text, functional = null;

        private Animation closeAnimation, fromEndlessToMainMenuAnimation, fromConfigurableToMeinMenuAnimation, fromEndlessToDonateAnimation, fromConfigurableToDonateAnimation;

        private const string ENDLESS_PANEL = "EndlessPanel";
        private const string CONFIGURABLE_PANEL = "ConfigurablePanel";

        private void OnEnable()
        {
            SetLabels(UpdateOverlayInformation);
            var panelController = UIController.Instance.PanelController;
            Debug.LogFormat("PC {0}", panelController);
            closeAnimation = SetHidddenAnimation(true);
            fromEndlessToMainMenuAnimation = SetHidddenAnimation(true).After(panelController.EndlessPanel.SetHiddenEnumerator(true)).After(panelController.MainMenuPanel.SetHiddenEnumerator(false));
            fromEndlessToDonateAnimation = SetHidddenAnimation(true).After(panelController.EndlessPanel.SetHiddenEnumerator(true)).After(panelController.DonatePanel.SetHiddenEnumerator(false));
            fromConfigurableToMeinMenuAnimation = SetHidddenAnimation(true).After(panelController.ConfigurablePanel.SetHiddenEnumerator(true)).After(panelController.MainMenuPanel.SetHiddenEnumerator(false));
            fromConfigurableToDonateAnimation = SetHidddenAnimation(true).After(panelController.ConfigurablePanel.SetHiddenEnumerator(true)).After(panelController.DonatePanel.SetHiddenEnumerator(false));
        }

        private void OnDisable()
        {

        }

        private void UpdateOverlayInformation()
        {
            text.text = Text("winOrBuyToUnlock");
            functional.text = Text("unlock");
        }

        public void OnCloseButton()
        {
            switch (UIController.Instance.data.activePanel.name)
            {
                case ENDLESS_PANEL:
                    fromEndlessToMainMenuAnimation.Start();
                    break;
                case CONFIGURABLE_PANEL:
                    fromConfigurableToMeinMenuAnimation.Start();
                    break;
                default:
                    closeAnimation.Start();
                    break;
            }
        }

        public void OnCancelButtonClick()
        {

        }

        public void OnConfirmButtonClick()
        {

        }

        public void OnFunctionalButtonClick()
        {
            switch (UIController.Instance.data.activePanel.name)
            {
                case ENDLESS_PANEL:
                    fromEndlessToDonateAnimation.Start();
                    break;
                case CONFIGURABLE_PANEL:
                    fromConfigurableToDonateAnimation.Start();
                    break;
                default:
                    closeAnimation.Start();
                    break;
            }
        }

        public void OnTryButtonClick()
        {
            switch (UIController.Instance.data.activePanel.name)
            {
                case ENDLESS_PANEL:
                    Advertising.Instance.TryToShowAds(TrialEndlessCallback);
                    break;
                case CONFIGURABLE_PANEL:
                    fromConfigurableToDonateAnimation.Start();
                    break;
                default:
                    closeAnimation.Start();
                    break;
            }
        }

        private void TrialEndlessCallback(ShowResult result)
        {
            switch (result)
            {
                case ShowResult.Finished:
                    Debug.Log("The ad was successfully shown.");
                    //
                    // YOUR CODE TO REWARD THE GAMER
                    // Give coins etc.
                    closeAnimation.Start();
                    break;
                case ShowResult.Skipped:
                    Debug.Log("The ad was skipped before reaching the end.");
                    break;
                case ShowResult.Failed:
                    Debug.LogError("The ad failed to be shown.");
                    break;
            }
        }
    }
}

