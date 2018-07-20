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

        public override Difficulty.RandomRotation GetData()
        {
            Difficulty.RandomRotation data = new Difficulty.RandomRotation();
            data.timeInterval = intervalSlider.Value;
            data.probability = probabilitySlider.Value;
            return data;
        }

        public override void SetData(Difficulty.RandomRotation data)
        {
            intervalSlider.Min = 0.1f;
            intervalSlider.Max = 10;
            intervalSlider.Step = 0.1f;
            intervalSlider.Value = data.timeInterval;

            probabilitySlider.Min = 0;
            probabilitySlider.Max = 1f;
            probabilitySlider.Step = 0.1f;
            probabilitySlider.Value = data.probability;
        }
    }
}