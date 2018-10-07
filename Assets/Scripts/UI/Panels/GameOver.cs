using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;
using Assets.Scripts.Indicator;
using Assets.Scripts.PlayServices;

namespace Assets.Scripts.UI.Panels
{
    public class GameOver : BasePanel
    {
        [SerializeField]
        private Text oops = null, messange = null, goalText = null, score = null, scoreType = null,
            skip = null, replay = null, mainMenu = null, newRecord = null;

        [SerializeField]
        private GameObject skipButton, messangeText, newRecordText;
        
        private bool isEndless, isConfigurable;

        private void OnEnable()
        {
            UIController.Instance.data.activePanel = this;
            isEndless = DifficultyLevels.Instance.CurrentDifficulty.target.endless;
            isConfigurable = DifficultyLevels.Instance.CurrentDifficulty.name == "Configurable";
            SetLabels(UpdateLabels);
            UpdateDynamicObjects();
            UpdateRecordScores();
        }

        public void OnReplayButtonClick()
        {
            if (!UIController.Instance.OnTrialPblockAction())
            {
                ReplayAction();
            }
        }

        public void ReplayAction()
        {
            BasePanel fade = UIController.Instance.PanelController.FadePanel;
            GameObject background = UIController.Instance.PanelController.backgroundPanel;
            var rePlay = UIController.Instance.data.activePanel.SwitchToAnimation(fade);
            rePlay.After(fade.SetHiddenEnumerator(true, after: () => Field.Instance.Master.Restart(), background: background));
            rePlay.Start();
        }

        private void UpdateLabels()
        {
            var gameOver = Field.Instance.Master.State.gameOver;

            int shouldScore, haveScore;

            if (gameOver.target.scoreBased)
            {
                goalText.text = Text("goalScore");
                scoreType.text = Text("points");
                shouldScore = gameOver.target.score;
                haveScore = gameOver.score;
            }
            else
            {
                goalText.text = Text("goalSurvive");
                scoreType.text = Text("seconds");
                shouldScore = gameOver.target.time;
                haveScore = gameOver.time;
            }

            oops.text = Text("oops");
            messange.text = gameOver.mode == GameMode.Color ? Text("wrongColor") : Text("wrongShape");
            scoreType.gameObject.SetActive(true);
            if (isEndless && !gameOver.target.scoreBased)
            {
                score.text = UIController.Instance.PanelController.endlessPanel.GetComponent<Endless>().FormatTimeScore(haveScore);
                if(haveScore>60)
                    scoreType.gameObject.SetActive(false);
            }
            else
                score.text = isEndless ? haveScore.ToString() : string.Format("{0}/{1}", haveScore, shouldScore);
            skip.text = Text("skip");
            replay.text = Text("replay");
            mainMenu.text = Text("mainMenu");
            
        }

        private void UpdateDynamicObjects()
        {
            var data = PersistentState.Instance.data;
            if (isEndless)
            {
                skipButton.SetActive(false);
                var gameOver = Field.Instance.Master.State.gameOver;
                if (gameOver.score > data.endlessScoreRecord && gameOver.target.scoreBased || 
                    gameOver.time > data.endlessTimeRecord && !gameOver.target.scoreBased)
                {
                    oops.text = Text("congrats");
                    messangeText.SetActive(false);
                    newRecordText.SetActive(true);
                    newRecord.text = Text("newRecord");
                }
                else
                {
                    oops.text = Text("oops");
                    messangeText.SetActive(true);
                    newRecordText.SetActive(false);
                }
            }
            else
            {
                if (isConfigurable || data.levelsUnlocked == DifficultyLevels.Instance.LevelCount)
                    skipButton.SetActive(false);
                else
                    skipButton.SetActive(true);
                oops.text = Text("oops");
                messangeText.SetActive(true);
                newRecordText.SetActive(false);
            }
        }

        private void UpdateRecordScores()
        {
            if (isEndless)
            {
                var gameOver = Field.Instance.Master.State.gameOver;
                var data = PersistentState.Instance.data;
                Debug.LogFormat("Score/EndlessRecord: {0}/{1}", gameOver.score, data.endlessScoreRecord);
                Debug.LogFormat("Time/EndlessRecord: {0}/{1}", gameOver.time, data.endlessTimeRecord);
                if (gameOver.score > data.endlessScoreRecord && gameOver.target.scoreBased)
                {
                    data.endlessScoreRecord = gameOver.score;
                    if (GPServices.Ready)
                    {
                        GPServices.Instance.UpdateEndlessScoreLeaderBoard(gameOver.score);
                    }
                }
                if (gameOver.time > data.endlessTimeRecord && !gameOver.target.scoreBased)
                {
                    data.endlessTimeRecord = gameOver.time;
                    if (GPServices.Ready)
                    {
                        GPServices.Instance.UpdateEndlessTimeLeaderBoard(gameOver.time);
                    }
                }
            }
        }

        

    }
}

