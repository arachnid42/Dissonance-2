using UnityEngine;
using UnityEditor;
using Assets.Scripts.Game;
using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.UI.Mapping
{
    public class ModesChangeSpoiler : Spoiler<Difficulty.ModeChange>
    {
        [SerializeField]
        private Slider maxAtOnceSlider, scoreCooldownSlider, 
            timeCooldownSlider, reactionTimeSlider, startScoreSlider;
        [SerializeField]
        private Slider.Data maxAtOnceSliderData = new Slider.Data(), scoreCooldownSliderData = new Slider.Data(), timeCooldownSliderData = new Slider.Data(),
            reactionTimeSliderData = new Slider.Data(), startScoreSliderData = new Slider.Data();

        public override Difficulty.ModeChange GetData()
        {
            Difficulty.ModeChange data = new Difficulty.ModeChange();
            data.maxAtOnce = Mathf.FloorToInt(maxAtOnceSlider.Value);
            data.scoreCooldown = Mathf.FloorToInt(scoreCooldownSlider.Value);
            data.timeCooldown = timeCooldownSlider.Value;
            data.reactionTime = reactionTimeSlider.Value;
            data.startScore = Mathf.FloorToInt(startScoreSlider.Value);
            return data;
        }

        public override void SetData(Difficulty.ModeChange data)
        {
            maxAtOnceSliderData.value = data.maxAtOnce;
            maxAtOnceSlider.SetData(maxAtOnceSliderData);

            scoreCooldownSliderData.value = data.scoreCooldown;
            scoreCooldownSlider.SetData(scoreCooldownSliderData);

            timeCooldownSliderData.value = data.timeCooldown;
            timeCooldownSlider.SetData(timeCooldownSliderData);

            reactionTimeSliderData.value = data.reactionTime;
            reactionTimeSlider.SetData(reactionTimeSliderData);

            startScoreSliderData.value = data.startScore;
            startScoreSlider.SetData(startScoreSliderData);
        }
    }
}