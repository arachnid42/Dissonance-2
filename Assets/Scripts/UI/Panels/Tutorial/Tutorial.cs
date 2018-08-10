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
        private GameObject generalTutorial, freezeTutorial, heartTutorial, explosionTutorial = null;

        private void OnEnable()
        {
            UIController.Instance.data.activePanel = this;
            ManageTutorials();
        }

        private void ManageTutorials()
        {
            var tutorialData = PersistentState.Instance.data.turotiral;
            if (tutorialData.basic)
            {
                generalTutorial.SetActive(true);
                freezeTutorial.SetActive(false);
                heartTutorial.SetActive(false);
                explosionTutorial.SetActive(false);

            }
            else if(tutorialData.lifeBonus)
            {
                generalTutorial.SetActive(false);
                freezeTutorial.SetActive(false);
                heartTutorial.SetActive(true);
                explosionTutorial.SetActive(false);
            }
            else if (tutorialData.freezeBonus)
            {
                generalTutorial.SetActive(false);
                freezeTutorial.SetActive(true);
                heartTutorial.SetActive(false);
                explosionTutorial.SetActive(false);
            }
            else if (tutorialData.explosionBonus)
            {
                generalTutorial.SetActive(false);
                freezeTutorial.SetActive(false);
                heartTutorial.SetActive(false);
                explosionTutorial.SetActive(true);
            }
            else
            {
                BackToGame();
            }
        }

        public void OnFinishTutorial()
        {
            BasePanel fade = UIController.Instance.PanelController.FadePanel;
            SwitchToAnimation(fade);
            UIController.Instance.PanelController.mainMenuPanel.GetComponent<MainMenu>().OnPlayButtonClick();
        }

        private void BackToGame()
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
    }
}

