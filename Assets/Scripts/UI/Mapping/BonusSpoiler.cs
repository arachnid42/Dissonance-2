using UnityEngine;
using UnityEditor;
using Assets.Scripts.Game;
using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.UI.Mapping
{
    public class BonusSpoiler : Spoiler<Difficulty.Bonus>
    {
        [SerializeField]
        private Slider numberSlider, startScoreSlider, 
            scoreIntervalSlider, probabilitySlider;
        [SerializeField]
        private int max;
        [SerializeField]
        private Slider.Data numberSliderData = new Slider.Data(), startScoreSliderData = new Slider.Data(),
            scoreIntervalSliderData = new Slider.Data(), probabilitySliderData = new Slider.Data();
        public override Difficulty.Bonus GetData()
        {
            Difficulty.Bonus data = new Difficulty.Bonus(max);
            data.number = Mathf.FloorToInt(numberSlider.Value);
            data.startScore = Mathf.FloorToInt(startScoreSlider.Value);
            data.scoreInterval = Mathf.FloorToInt(scoreIntervalSlider.Value);
            data.probability = probabilitySlider.Value;
            return data;
        }

        public override void SetData(Difficulty.Bonus data)
        {
            numberSliderData.value = data.number;
            numberSlider.SetData(numberSliderData);

            startScoreSliderData.value = data.startScore;
            startScoreSlider.SetData(startScoreSliderData);

            scoreIntervalSliderData.value = data.scoreInterval;
            scoreIntervalSlider.SetData(scoreIntervalSliderData);

            probabilitySliderData.value = data.probability;
            probabilitySlider.SetData(probabilitySliderData);
        }
    }
}