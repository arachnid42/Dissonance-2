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
        [SerializeField]
        private Slider.Data scoreIntervalSliderData = new Slider.Data(), timeIntervalSliderData = new Slider.Data(), probabilitySliderData = new Slider.Data();
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
            scoreIntervalSliderData.value = data.scoreInterval;
            scoreIntervalSlider.SetData(scoreIntervalSliderData);

            timeIntervalSliderData.value = data.timeInterval;
            timeIntervalSlider.SetData(timeIntervalSliderData);

            probabilitySliderData.value = data.probability;
            probabilitySlider.SetData(probabilitySliderData);

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
