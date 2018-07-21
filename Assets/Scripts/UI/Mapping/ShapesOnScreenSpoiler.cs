using UnityEngine;
using UnityEditor;
using Assets.Scripts.Game;
using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.UI.Mapping
{
    public class ShapesOnScreenSpoiler : Spoiler<Difficulty.ShapesOnScreen>
    {
        [SerializeField]
        private Slider minSlider, maxSlider, intervalSlider;
        [SerializeField]
        private Slider.Data minSliderData = new Slider.Data(), maxSliderData = new Slider.Data(), intervalSliderData = new Slider.Data();
        public override Difficulty.ShapesOnScreen GetData()
        {
            Difficulty.ShapesOnScreen data = new Difficulty.ShapesOnScreen();
            data.min = Mathf.FloorToInt(minSlider.Value);
            data.max = Mathf.FloorToInt(maxSlider.Value);
            data.increaseScoreInterval = Mathf.FloorToInt(intervalSlider.Value);
            return data;
        }

        public override void SetData(Difficulty.ShapesOnScreen data)
        {
            minSliderData.value = data.min;
            minSliderData.max = data.max;
            minSlider.SetData(minSliderData);

            maxSliderData.value = data.max;
            maxSliderData.min = data.min;
            maxSlider.SetData(maxSliderData);

            intervalSliderData.value = data.increaseScoreInterval;
            intervalSlider.SetData(intervalSliderData);
        }
    }
}