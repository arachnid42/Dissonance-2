using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Analytics;


namespace Assets.Scripts.UI.Panels
{
    public class Donate : BasePanel
    {
        [SerializeField]
        private Text donate = null, 
            donateMessange = null, 
            continueButton = null, 
            back = null;

        private void OnEnable()
        {
            Events.PanelOpened(Events.Panels.DONATION_MESSAGE);
            SetLabels(UpdateLabels);
            UIController.Instance.data.activePanel = this;
        }

        private void UpdateLabels()
        {
            donate.text = Text("donate");
            donateMessange.text = Text("donateMessange");
            continueButton.text = Text("continueButton");
            back.text = Text("back");
        }

        public void OnDonateMenuButtonClick()
        {
            SwitchToAnimation(UIController.Instance.PanelController.DonateMenuPanel).Start();
        }
    }
}

