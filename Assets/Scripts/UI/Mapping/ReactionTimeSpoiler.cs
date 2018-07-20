using UnityEngine;
using UnityEditor;
using Assets.Scripts.Game;
using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.UI.Mapping
{
    public class ReactionTimeSpoiler : Spoiler<Difficulty.ReactionTime>
    {
        [SerializeField]
        private Slider minSlider, maxSlider, changeSlider;

        public override Difficulty.ReactionTime GetData()
        {
            Difficulty.ReactionTime data = new Difficulty.ReactionTime();
            data.min = minSlider.Value;
            data.max = maxSlider.Value;
            data.changePerScore = changeSlider.Value;
            return data;
        }

        public override void SetData(Difficulty.ReactionTime data)
        {
            minSlider.Min = 1;
            minSlider.Max = data.max;
            minSlider.Step = 1;
            minSlider.Value = data.min;

            maxSlider.Min = data.min;
            maxSlider.Max = 5;
            maxSlider.Step = 1;
            maxSlider.Value = data.max;

            changeSlider.Min = -1;
            changeSlider.Max = 0;
            changeSlider.Step = 0.1f;
            changeSlider.Value = data.changePerScore;
        }
    }
}