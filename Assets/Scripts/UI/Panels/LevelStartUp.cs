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
        private Text header = null, footer = null, number = null;

        private void OnEnable()
        {
            StartCoroutine(AlterField(() => Field.Instance.gameObject.SetActive(true)));

            var difficulty = DifficultyLevels.Instance.CurrentDifficulty;
            gameObject.GetComponent<CanvasGroup>().alpha = 1;
            if (difficulty.target.endless)
            {
                header.text = Text("tryToBeat");
                footer.text = difficulty.target.scoreBased?Text("points"):Text("seconds");
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

    }
}

