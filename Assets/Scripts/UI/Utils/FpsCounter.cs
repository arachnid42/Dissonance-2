using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Assets.Scripts.UI.Utils
{
    public class FpsCounter : MonoBehaviour
    {
        [SerializeField]
        private float frequency = 0.5f;
        [SerializeField]
        public Text text;

        private void Start()
        {
            StartCoroutine(FPS());
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        public int FramesPerSec { get; protected set; }

        private IEnumerator FPS()
        {
            for (; ; )
            {
                // Capture frame-per-second
                int lastFrameCount = Time.frameCount;
                float lastTime = Time.realtimeSinceStartup;
                yield return new WaitForSeconds(frequency);
                float timeSpan = Time.realtimeSinceStartup - lastTime;
                int frameCount = Time.frameCount - lastFrameCount;

                // Display it
                FramesPerSec = Mathf.RoundToInt(frameCount / timeSpan);
                text.text = "FPS "+FramesPerSec.ToString();
            }
        }

    }
}

