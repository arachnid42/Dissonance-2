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
        [SerializeField]
        private Slider.Data minSliderData = new Slider.Data(), maxSliderData = new Slider.Data(), changeSliderData = new Slider.Data();
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
            minSliderData.value = data.min;
            minSliderData.max = data.max;
            minSlider.SetData(minSliderData);

            maxSliderData.value = data.max;
            maxSliderData.min = data.min;
            maxSlider.SetData(maxSliderData);

            changeSliderData.value = data.changePerScore;
            changeSlider.SetData(changeSliderData);
        }
    }
}