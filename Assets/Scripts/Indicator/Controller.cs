using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Indicator
{
    public class Controller : MonoBehaviour
    {
        [SerializeField]
        private Bonuses bonuses = null;
        [SerializeField]
        private Counter scoreCounter = null;
        [SerializeField]
        private Hide hide = null;
        [SerializeField]
        private Mode mode = null;
        [SerializeField]
        private Timer timer = null;
        [SerializeField]
        private HideElement hideScoreConter;
        [SerializeField]
        private HideElement hideTimer;

        public bool ScoreHidden
        {
            get
            {
                return hideScoreConter.Hidden;
            }
            set
            {
                hideScoreConter.Hidden = value;
            }
        }

        public bool TimerHidden
        {
            get
            {
                return hideTimer.Hidden;
            }
            set
            {
                hideTimer.Hidden = value;
            }
        }

        public Timer Timer
        {
            get
            {
                return timer;
            }
        }
        
        public Mode Mode
        {
            get
            {
                return mode;
            }
        }

        public Counter ScoreCounter
        {
            get
            {
                return scoreCounter;
            }
        }

        public Bonuses Bonuses
        {
            get
            {
                return bonuses;
            }
        }

        public Hide Hide
        {
            get
            {
                return hide;
            }
        }
    }
}


