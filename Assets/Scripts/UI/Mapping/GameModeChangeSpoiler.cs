using UnityEngine;
using UnityEditor;
using Assets.Scripts.UI.Elements;
using Assets.Scripts.Game;

namespace Assets.Scripts.UI.Mapping
{
    public class GameModeChangeSpoiler : Spoiler<Difficulty.Mode>
    {
        [SerializeField]
        private Slider scoreIntervalSlider, timeIntervalSlider, probabilitySlider;
        [SerializeField]
        private ToggleGroup toggleGroup;

        public override Difficulty.Mode GetData()
        {
            Difficulty.Mode data = new Difficulty.Mode();
            data.scoreInterval = Mathf.FloorToInt(scoreIntervalSlider.Value);
            data.timeInterval = timeIntervalSlider.Value;
            data.probability = probabilitySlider.Value;
            data.scoreBased = GetScoredBasedValue();
            return data;
        }

        public override void SetData(Difficulty.Mode data)
        {
            scoreIntervalSlider.Min = 0;
            scoreIntervalSlider.Max = 100;
            scoreIntervalSlider.Step = 10f;
            scoreIntervalSlider.Value = data.scoreInterval;

            timeIntervalSlider.Min = 0;
            timeIntervalSlider.Max = 100;
            timeIntervalSlider.Step = 10;
            timeIntervalSlider.Value = data.timeInterval;

            probabilitySlider.Min = 0;
            probabilitySlider.Max = 1f;
            probabilitySlider.Step = 0.1f;
            probabilitySlider.Value = data.probability;

            if (data.scoreBased)
            {
                timeIntervalSlider.gameObject.SetActive(false);
                scoreIntervalSlider.gameObject.SetActive(true);
                toggleGroup.OnToggleGroup("ScoreToggle");
            }
            else
            {
                timeIntervalSlider.gameObject.SetActive(true);
                scoreIntervalSlider.gameObject.SetActive(false);
                toggleGroup.OnToggleGroup("TimeToggle");
            }
            toggleGroup.Toggle = HandleToggle;
        }

        private void HandleToggle()
        {
            if (GetScoredBasedValue())
            {
                timeIntervalSlider.gameObject.SetActive(false);
                scoreIntervalSlider.gameObject.SetActive(true);
            }
            else
            {
                timeIntervalSlider.gameObject.SetActive(true);
                scoreIntervalSlider.gameObject.SetActive(false);
            }
        }

        private bool GetScoredBasedValue()
        {
            bool val = false;
            foreach (Toggle toggle in toggleGroup.GetActiveToggles())
            {
                if(toggle.gameObject.name == "ScoreToggle")
                {
                    val = true;
                }else if(toggle.gameObject.name == "TimeToggle")
                {
                    val = false;
                }
            }
            return val;
        }
    }
}
