using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;

namespace Assets.Scripts.UI.Panels
{
    public class LevelStartUp : BasePanel
    {
        [SerializeField]
        private Text header, footer, number, tutorialWelcome, tutorialMessage = null;
        [SerializeField]
        private GameObject goalPanel, tutorialPanel;

        private void OnEnable()
        {
            StartCoroutine(AlterField(() => Field.Instance.gameObject.SetActive(true)));
            ReactOnTutorial();
        }

        private void ReactOnTutorial()
        {
            if (ShouldShowTutorial())
            {
                tutorialPanel.SetActive(true);
                goalPanel.SetActive(false);
                SetupTutorialPanel();
            }
            else
            {
                tutorialPanel.SetActive(false);
                goalPanel.SetActive(true);
                SetupGoalScreen();
            }
        }

        private void SetupTutorialPanel()
        {
            tutorialWelcome.text = Text("tutorialWelcome");
            tutorialMessage.text = Text("tutorialMessage");
        }

        private void SetupGoalScreen()
        {
            var difficulty = DifficultyLevels.Instance.CurrentDifficulty;
            gameObject.GetComponent<CanvasGroup>().alpha = 1;
            if (difficulty.target.endless)
            {
                header.text = Text("tryToBeat");
                footer.text = difficulty.target.scoreBased ? Text("points") : Text("seconds");
                number.text = (difficulty.target.scoreBased ? PersistentState.Instance.data.endlessScoreRecord : PersistentState.Instance.data.endlessTimeRecord).ToString();
            }
            else if (difficulty.target.scoreBased)
            {
                header.text = Text("tryToScore");
                footer.text = Text("points");
                number.text = difficulty.target.score.ToString();
            }
            else
            {
                header.text = Text("tryToSurvive");
                footer.text = Text("seconds");
                number.text = difficulty.target.time.ToString();
            }
        }

        private bool ShouldShowTutorial()
        {
            var tutorialData = PersistentState.Instance.data.turotiral;
            Difficulty currentDifficulty = DifficultyLevels.Instance.CurrentDifficulty;
            return (
                !tutorialData.basic ||
                !tutorialData.explosionBonus && currentDifficulty.ShouldShowExplosionBonusTutorial() ||
                !tutorialData.freezeBonus && currentDifficulty.ShouldShowFreezeBonusTutorial() ||
                !tutorialData.lifeBonus && currentDifficulty.ShouldShowLifeBonusTutorial()
                );
        }

    }
}

