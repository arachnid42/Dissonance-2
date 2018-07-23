using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Shape
{
    public class Halo : MonoBehaviour
    {

        public float pulseInterval = 0.5f;
        public float minIntensity = 25;
        public float maxIntensity = 28;

        private float stage = 0;
        private bool increase = true;

        // Update is called once per frame

        public void Update()
        {
            if (stage < 1)
            {
                stage += (Time.deltaTime / pulseInterval)*2;
                float scale = transform.parent.lossyScale.x;
                float start = increase?minIntensity:maxIntensity;
                float end = increase?maxIntensity:minIntensity;
                GetComponentInChildren<Light>().range = Mathf.Lerp(start*scale, end*scale, stage);
            }
            else
            {
                increase = !increase;
                stage = 0;
            }
        }
    }
}

