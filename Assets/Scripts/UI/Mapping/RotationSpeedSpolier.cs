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

        public override Difficulty.Rotation GetData()
        {
            Difficulty.Rotation data = new Difficulty.Rotation();
            data.min = minSlider.Value;
            data.max = maxSlider.Value;
            return data;
        }

        public override void SetData(Difficulty.Rotation data)
        {
            minSlider.Min = 0;
            minSlider.Max = data.max;
            minSlider.Step = 5;
            minSlider.Value = data.min;

            maxSlider.Min = data.min;
            maxSlider.Max = 360;
            maxSlider.Step = 5;
            maxSlider.Value = data.max;
        }
    }
}