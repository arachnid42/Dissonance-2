using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Indicator
{
    public class Timer : MonoBehaviour
    {

        [SerializeField]
        private TextMesh timerText = null;
        private float seconds = 0;

        public float Seconds
        {
            get { return seconds; }
            set
            {
                if (seconds == value)
                    return;
                seconds = value;
                timerText.text = FormatTimerText(seconds);
            }
        }

        private string FormatTimerText(float newSeconds)
        {
            int minutes = Mathf.FloorToInt(newSeconds / 60.0f);
            int seconds = Mathf.FloorToInt(newSeconds % 60.0f);
            int centiseconds = Mathf.FloorToInt((newSeconds - minutes * 60 - seconds) * 100);
            string sseconds = (seconds < 10 ? "0" : "") + seconds.ToString();
            string scentiseconds = (centiseconds < 10 ? "0" : "") + centiseconds.ToString();
            if (minutes > 0)
            {
                string sminutes = minutes.ToString();
                return string.Format("{0}:{1}:{2}", sminutes, sseconds, scentiseconds);
            }
            return string.Format("{0}:{1}", sseconds, scentiseconds);
        }

    }
}
