using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Elements
{
    public class RateStars : MonoBehaviour
    {
        private List<StarButton> stars = new List<StarButton>();
        [SerializeField]
        [Range(0, 5)]
        private int rate;

        public delegate void RateHandler(int rate);
        public RateHandler OnRateChanges;

        protected void RateChanges(int rate)
        {
            if (OnRateChanges != null)
            {
                OnRateChanges(rate);
            }
        }

        private void OnEnable()
        {
            Rating = rate;
        }

        public void AddStarToGroup(StarButton star)
        {
            stars.Add(star);
        }

        public void OnStarGroup(int id)
        {
            SetRating(id);
            RateChanges(id);
        }

        public void SetRating(int newRate)
        {
            rate = newRate;
            foreach (StarButton star in stars)
            {
                if (star.Id <= rate)
                    star.Value = true;
                else
                    star.Value = false;
            }
        }

        public int Rating
        {
            get { return rate; }
            set { SetRating(value); }
        }
    }
}

    