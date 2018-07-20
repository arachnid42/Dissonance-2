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
            numberSlider.Min = 0;
            numberSlider.Max = 100;
            numberSlider.Step = 1;
            numberSlider.Value = data.number;

            startScoreSlider.Min = 0;
            startScoreSlider.Max = 100;
            startScoreSlider.Step = 1;
            startScoreSlider.Value = data.startScore;

            scoreIntervalSlider.Min = 1;
            scoreIntervalSlider.Max = 100;
            scoreIntervalSlider.Step = 1;
            scoreIntervalSlider.Value = data.scoreInterval;

            probabilitySlider.Min = 0;
            probabilitySlider.Max = 1f;
            probabilitySlider.Step = 0.1f;
            probabilitySlider.Value = data.probability;
        }
    }
}