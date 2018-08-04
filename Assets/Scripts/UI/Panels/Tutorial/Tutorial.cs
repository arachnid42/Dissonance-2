using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;

namespace Assets.Scripts.UI.Panels.Tutorial
{
    public class Tutorial : BasePanel
    {
        [SerializeField]
        private Text pause = null, resume = null, mainMenu = null;

        private void OnEnable()
        {
            //SetLabels(UpdateLabels);
            UIController.Instance.data.activePanel = this;
        }

        private void UpdateLabels()
        {
            pause.text = Text("pause");
            resume.text = Text("resume");
            mainMenu.text = Text("mainMenu");

        }

        public void OnFinishTutorial()
        {
            BasePanel startup = UIController.Instance.PanelController.LevelStartUpPanel;
            GameObject background = UIController.Instance.PanelController.backgroundPanel;

            var startPlay = SwitchToAnimation(startup);
            startPlay.After(startup.SetHiddenEnumerator(true, after: () => {
                Field.Instance.Master.Restart();
                UIController.Instance.data.isInMainMenu = false;
            }, background: background));
            startPlay.Start();
        }

        public void OnMainMenuButtonClick()
        {
            SwitchToAnimation(UIController.Instance.PanelController.MainMenuPanel, 
                after:()=> Field.Instance.Master.Stop() , 
                background:UIController.Instance.PanelController.backgroundPanel).Start();
        }


    }
}

