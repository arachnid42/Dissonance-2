using UnityEngine;
using UnityEditor;
using Assets.Scripts.Game;
using Assets.Scripts.UI.Elements;
using Assets.Scripts.Localization;

namespace Assets.Scripts.UI.Mapping
{
    public class TargetMapper : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Text endlessText;
        [SerializeField]
        private Slider scoreTargetSlider, timeTargetSlider;
        [SerializeField]
        private ToggleGroup toggleGroup;
        [SerializeField]
        private Slider.Data scoreTargetSliderData = new Slider.Data(), timeTargetSliderData = new Slider.Data();
        private bool isScoreBased, isEndless;

        public Difficulty.Target GetData()
        {
            Difficulty.Target data = new Difficulty.Target();
            GetToggleValues();
            data.scoreBased = isScoreBased;
            data.endless = isEndless;
            data.score = Mathf.FloorToInt(scoreTargetSlider.Value);
            data.time = Mathf.FloorToInt(timeTargetSlider.Value);
            return data;
        }

        public void SetData(Difficulty.Target data)
        {
            SetToggleGroupValues(data);

            scoreTargetSliderData.value = data.score;
            scoreTargetSlider.SetData(scoreTargetSliderData);

            timeTargetSliderData.value = data.time;
            timeTargetSlider.SetData(timeTargetSliderData);
            HandleToggle();
            toggleGroup.Toggle = HandleToggle;
        }

        private void HandleToggle()
        {
            GetToggleValues();
            
            if (isEndless)
            {
                timeTargetSlider.gameObject.SetActive(false);
                scoreTargetSlider.gameObject.SetActive(false);
                endlessText.text = LocalizationManager.Instance["endlessTarget"];
                endlessText.gameObject.SetActive(true);
            }
            else
            {
                endlessText.gameObject.SetActive(false);
                if (isScoreBased)
                {
                    timeTargetSlider.gameObject.SetActive(false);
                    scoreTargetSlider.gameObject.SetActive(true);
                }
                else
                {
                    timeTargetSlider.gameObject.SetActive(true);
                    scoreTargetSlider.gameObject.SetActive(false);
                }
            }
        }

        private void GetToggleValues()
        {
            foreach (Toggle toggle in toggleGroup.GetActiveToggles())
            {
                //if (toggle.gameObject.name == "EndlessToggle")
                //{
                //    isEndless = true;
                //    isScoreBased = false;
                //}
                //else
                //    isEndless = false;
                isEndless = false;
                if (toggle.gameObject.name == "ScoreToggle")
                    isScoreBased = true;
                if (toggle.gameObject.name == "TimeToggle")
                    isScoreBased = false;
            }
        }

        private void SetToggleGroupValues(Difficulty.Target data)
        {
            //toggleGroup.ResetValues();
            //toggleGroup.SetToggleValue("ScoreToggle", data.scoreBased);
            //toggleGroup.SetToggleValue("TimeToggle", !data.scoreBased);
            //if (data.endless)
            //{
            //    toggleGroup.OnToggleGroup("EndlessToggle");
            //}
            //else
            //{
            //    toggleGroup.SetToggleValue("ScoreToggle", data.scoreBased);
            //    toggleGroup.SetToggleValue("TimeToggle", !data.scoreBased);
            //}

            if(data.scoreBased)
                toggleGroup.OnToggleGroup("ScoreToggle");
            else
                toggleGroup.OnToggleGroup("TimeToggle");

        }
    }
}
