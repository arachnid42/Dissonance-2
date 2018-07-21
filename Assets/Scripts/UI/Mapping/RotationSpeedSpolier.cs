using UnityEngine;
using UnityEditor;
using Assets.Scripts.Game;
using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.UI.Mapping
{
    public class RotationSpeedSpolier : Spoiler<Difficulty.Rotation>
    {
        [SerializeField]
        private Slider minSlider, maxSlider;
        [SerializeField]
        private Slider.Data minSliderData = new Slider.Data(), maxSliderData = new Slider.Data();
        public override Difficulty.Rotation GetData()
        {
            Difficulty.Rotation data = new Difficulty.Rotation();
            data.min = minSlider.Value;
            data.max = maxSlider.Value;
            return data;
        }

        public override void SetData(Difficulty.Rotation data)
        {
            minSliderData.value = data.min;
            minSliderData.max = data.max;
            minSlider.SetData(minSliderData);

            maxSliderData.value = data.max;
            maxSliderData.min = data.min;
            maxSlider.SetData(maxSliderData);
        }
    }
}