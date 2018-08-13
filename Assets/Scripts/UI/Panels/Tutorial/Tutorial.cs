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
        [SerializeField]
        private Text gameMode = null;
        [SerializeField]
        public Image background;
        [SerializeField]
        private Sprite generalBackground, lifeBackground, freezeBackground, explosionBackground;
        [SerializeField]
        private TutorialScreen fadeScreen;


        private TutorialController controller;
        private BaseTutorial general, heart, freeze, explosion;
        private bool IsBackgroundSetted = false;
        private void OnEnable()
        {
            UIController.Instance.data.activePanel = this;
            controller = GetComponent<TutorialController>();
            gameMode.text = Text("color");
            ManageTutorials();
        }

        private void OnDisable()
        {
            controller.Screens.Clear();
            IsBackgroundSetted = false;

    }

    private void ManageTutorials()
        {
            Difficulty currentDifficulty = DifficultyLevels.Instance.CurrentDifficulty;
            var tutorialData = PersistentState.Instance.data.turotiral;
            generalTutorial.SetActive(false);
            freezeTutorial.SetActive(false);
            heartTutorial.SetActive(false);
            explosionTutorial.SetActive(false);
            if (!tutorialData.basic)
            {
                InsertTutorial(generalTutorial.GetComponent<TutorialController>().Screens);
                general = generalTutorial.GetComponent<BaseTutorial>();
                generalTutorial.SetActive(true);
                SetBackground(generalBackground);
            }
            if (!tutorialData.lifeBonus && currentDifficulty.ShouldShowLifeBonusTutorial())
            {
                SetBackground(lifeBackground);
                InsertTutorial(heartTutorial.GetComponent<TutorialController>().Screens, GetScreenInsertionIndex());
                heart = heartTutorial.GetComponent<BaseTutorial>();
                heartTutorial.SetActive(true);
            }
            if (!tutorialData.freezeBonus && currentDifficulty.ShouldShowFreezeBonusTutorial())
            {
                SetBackground(freezeBackground);
                InsertTutorial(freezeTutorial.GetComponent<TutorialController>().Screens, GetScreenInsertionIndex());
                freeze = freezeTutorial.GetComponent<BaseTutorial>();
                freezeTutorial.SetActive(true);
            }
            if (!tutorialData.explosionBonus && currentDifficulty.ShouldShowExplosionBonusTutorial())
            {
                SetBackground(explosionBackground);
                InsertTutorial(explosionTutorial.GetComponent<TutorialController>().Screens, GetScreenInsertionIndex());
                explosion = explosionTutorial.GetComponent<BaseTutorial>();
                explosionTutorial.SetActive(true);
            }
            controller.ResetTutorial();
        }

        private void SetBackground(Sprite backgroundSprite)
        {
            if (!IsBackgroundSetted)
            {
                background.sprite = backgroundSprite;
                IsBackgroundSetted = true;
            }
        }

        private int GetScreenInsertionIndex()
        {
            var tutorialData = PersistentState.Instance.data.turotiral;
            int index = 0;
            if (!tutorialData.basic)
            {
                index = controller.Screens.Count - 2;
                generalTutorial.SetActive(true);
            }
            return index;
        }

        public void OnTutorialActionClick()
        {
            var current = controller.Current;
            var next = controller.Next;
            if (next)
            {
                current.SetHidddenAnimation(true, 0.2f, after:()=> RunScreensNext(current, next)).Start(true);
                next.SetHidddenAnimation(false, 0.3f).Start(true);
            }
            else
            {
                TurnOffFinishedTutorials();
                OnFinishTutorial();
            }
        }

        private void RunScreensNext(BasePanel current, BasePanel next)
        {
            if (general)
                general.Next(current, next);
            if (heart)
                heart.Next(current, next);
            if (explosion)
                explosion.Next(current, next);
            if (freeze)
                freeze.Next(current, next);
        }

        private void TurnOffFinishedTutorials()
        {
            if(generalTutorial.activeSelf)
                PersistentState.Instance.data.turotiral.basic = true;
            if (heartTutorial.activeSelf)
                PersistentState.Instance.data.turotiral.lifeBonus = true;
            if (freezeTutorial.activeSelf)
                PersistentState.Instance.data.turotiral.freezeBonus = true;
            if (explosionTutorial.activeSelf)
                PersistentState.Instance.data.turotiral.explosionBonus = true;

        }

        public void OnFinishTutorial()
        {
            UIController.Instance.PanelController.mainMenuPanel.GetComponent<MainMenu>().StartGame();
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

        private void InsertTutorial(List<GameObject> list, int index = 0)
        {
            if(index == 0)
            {
                foreach(GameObject screen in list)
                {
                    controller.Screens.Add(screen);
                }
            }
            else
            {
                for(int i = list.Count-1; i>=0; i--)
                {
                    controller.Screens.Insert(index, list[i]);

                }
            }
        }
    }
}

