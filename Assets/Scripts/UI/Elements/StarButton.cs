using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Elements
{
    public class StarButton : MonoBehaviour
    {
        [SerializeField]
        private Image starImage;
        [SerializeField]
        private Sprite unselectedStarImage, selectedStarImage;
        [SerializeField]
        private RateStars rateStars;
        [SerializeField]
        private int id;

        private bool isOn;

        private void Start()
        {
            rateStars.AddStarToGroup(this);
        }

        public void OnStarClick()
        {
            rateStars.OnStarGroup(id);
        }

        public int Id
        {
            get { return id; }
        }

        public bool Value
        {
            get { return isOn; }
            set { isOn = value; UpdateButton(); }
        }

        private void UpdateButton()
        {
            if (isOn)
            {
                starImage.sprite = selectedStarImage;
            }
            else
            {
                starImage.sprite = unselectedStarImage;
            }
        }
    }
}

