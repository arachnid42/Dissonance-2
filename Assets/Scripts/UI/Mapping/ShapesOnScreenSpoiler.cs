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
            minSlider.Min = 1;
            minSlider.Max = data.max;
            minSlider.Step = 1;
            minSlider.Value = data.min;

            maxSlider.Min = data.min;
            maxSlider.Max = 9;
            maxSlider.Step = 1;
            maxSlider.Value = data.max;

            intervalSlider.Min = 1;
            intervalSlider.Max = 100;
            intervalSlider.Step = 1;
            intervalSlider.Value = data.increaseScoreInterval;
        }
    }
}