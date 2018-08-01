using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;
using Assets.Scripts.Monetization;

namespace Assets.Scripts.UI.Panels
{
    public class CloseOverlay : BasePanel
    {

        [SerializeField]
        private Text text, cancel, confirm = null;

        private Animation closeAnimation;

        private void OnEnable()
        {
            SetLabels(UpdateOverlayInformation);
            closeAnimation = Field.Instance.Master.State.started && (!UIController.Instance.data.activePanel || UIController.Instance.data.activePanel && UIController.Instance.data.activePanel.name!="PausePanel")? SetHidddenAnimation(true, after: () => Field.Instance.Master.Actions.Pause()) : SetHidddenAnimation(true);
        }

        private void OnDisable()
        {
            
        }

        private void UpdateOverlayInformation()
        {
            text.text = Text("exitText");
            cancel.text = Text("no");
            confirm.text = Text("yes");
        }

        public void OnCloseButton()
        {
            closeAnimation.Start();
        }

        public void OnCancelButtonClick()
        {
            OnCloseButton();
        }

        public void OnConfirmButtonClick()
        {
            OnCloseButton();
            Application.Quit();
        }

    }
}

