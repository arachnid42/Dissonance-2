using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;

namespace Assets.Scripts.UI.Panels
{
    public class Pause : BasePanel
    {
        [SerializeField]
        private Text pause = null, resume = null, mainMenu = null;

        private void OnEnable()
        {
            SetLabels(UpdateLabels);
            UIController.Instance.data.activePanel = this;
        }

        private void UpdateLabels()
        {
            pause.text = Text("pause");
            resume.text = Text("resume");
            mainMenu.text = Text("mainMenu");

        }

        public void OnResumeButton()
        {
            SetHidddenAnimation(true, after:()=> Field.Instance.Master.Actions.Pause()).Start();
        }

        public void OnMainMenuButtonClick()
        {
            SwitchToAnimation(UIController.Instance.PanelController.MainMenuPanel, 
                after:()=> Field.Instance.Master.Stop() , 
                background:UIController.Instance.PanelController.backgroundPanel).Start();
        }


    }
}

