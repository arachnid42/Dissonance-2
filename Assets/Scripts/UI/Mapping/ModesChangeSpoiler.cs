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
            maxAtOnceSlider.Min = 1;
            maxAtOnceSlider.Max = 3;
            maxAtOnceSlider.Step = 1;
            maxAtOnceSlider.Value = data.maxAtOnce;

            scoreCooldownSlider.Min = 1;
            scoreCooldownSlider.Max = 100;
            scoreCooldownSlider.Step = 10;
            scoreCooldownSlider.Value = data.scoreCooldown;

            timeCooldownSlider.Min = 1;
            timeCooldownSlider.Max = 100;
            timeCooldownSlider.Step = 10;
            timeCooldownSlider.Value = data.timeCooldown;

            reactionTimeSlider.Min = 0.5f;
            reactionTimeSlider.Max = 1f;
            reactionTimeSlider.Step = 0.1f;
            reactionTimeSlider.Value = data.reactionTime;

            startScoreSlider.Min = 1;
            startScoreSlider.Max = 100;
            startScoreSlider.Step = 10;
            startScoreSlider.Value = data.startScore;
        }
    }
}