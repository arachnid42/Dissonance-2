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
            timeCount.text = FormatTimeScore(PersistentState.Instance.data.endlessTimeRecord);
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

        public string FormatTimeScore(int scoreSeconds)
        {
            int hours = Mathf.FloorToInt(scoreSeconds / 3600.0f);
            int minutes = Mathf.FloorToInt((scoreSeconds % 3600) / 60);
            int seconds = Mathf.FloorToInt((scoreSeconds % 3600) % 60);
            string sseconds = (seconds < 10 ? "0" : "") + seconds.ToString();
            string sminutes = (minutes < 10 ? "0" : "") + minutes.ToString();

            if (hours > 0)
            {
                string shours = (hours < 10 ? "0" : "") + hours.ToString();
                return string.Format("{0}:{1}:{2}", shours, sminutes, sseconds);
            }
            if (minutes > 0)
            {
                return string.Format("{0}:{1}", sminutes, sseconds);
            }
            
            return string.Format("{0}", seconds.ToString());
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

