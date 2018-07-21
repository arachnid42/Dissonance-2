using UnityEngine;
using UnityEditor;
using Assets.Scripts.Game;
using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.UI.Mapping
{
    public class RandomRotationSpoiler : Spoiler<Difficulty.RandomRotation>
    {
        [SerializeField]
        private Slider intervalSlider, probabilitySlider;
        [SerializeField]
        private Slider.Data intervalSliderData = new Slider.Data(), probabilitySliderData = new Slider.Data();
        public override Difficulty.RandomRotation GetData()
        {
            Difficulty.RandomRotation data = new Difficulty.RandomRotation();
            data.timeInterval = intervalSlider.Value;
            data.probability = probabilitySlider.Value;
            return data;
        }

        public override void SetData(Difficulty.RandomRotation data)
        {
            intervalSliderData.value = data.timeInterval;
            intervalSlider.SetData(intervalSliderData);

            probabilitySliderData.value = data.probability;
            probabilitySlider.SetData(probabilitySliderData);
        }
    }
}