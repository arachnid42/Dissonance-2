using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;


namespace Assets.Scripts.UI.Panels
{
    public class Endless : BasePanel
    {
        [SerializeField]
        private Text title = null, title2 = null, score = null, time = null,
            bestScore = null, scoreCount = null, bestTime = null, timeCount = null, back = null;

        private void OnEnable()
        {
            SetLabels(UpdateLabels);
            UIController.Instance.data.activePanel = this;
        }

        private void UpdateLabels()
        {
            title.text = Text("endless");
            title2.text = Text("chooseRecord");
            score.text = Text("score");
            time.text = Text("time");
            bestScore.text = Text("bestScore");
            bestTime.text = Text("bestTime");
            back.text = Text("back");
            scoreCount.text = PersistentState.Instance.data.endlessScoreRecord.ToString();
            timeCount.text = PersistentState.Instance.data.endlessTimeRecord.ToString();
        }

        public void OnScoreButtonClick()
        {
            DifficultyLevels.Instance.LevelName = "Endless";
            DifficultyLevels.Instance.CurrentDifficulty.target.scoreBased = true;
            StartGame();
        }

        public void OnTimeButtonClick()
        {
            DifficultyLevels.Instance.LevelName = "Endless";
            DifficultyLevels.Instance.CurrentDifficulty.target.scoreBased = false;
            StartGame();
        }

        private void StartGame()
        {
            BasePanel fade = UIController.Instance.PanelController.FadePanel;
            GameObject background = UIController.Instance.PanelController.backgroundPanel;
            var startPlay = UIController.Instance.data.activePanel.SwitchToAnimation(fade);
            startPlay.After(fade.SetHiddenEnumerator(true, after: () => {
                Field.Instance.Master.Restart();
                UIController.Instance.data.isInMainMenu = false;
            }, background: background));
            startPlay.Start();
        }

    }
}

