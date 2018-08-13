using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;
using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.UI.Panels.Tutorial
{
    public class ExplosionTutorial : BaseTutorial
    {
        [SerializeField]
        private Tutorial tutorial;
        [SerializeField]
        private Swipeable swipeable;
        [SerializeField]
        private Text text1, text2 = null;
        [SerializeField]
        private Sprite background1, background2;
        private TutorialController tutorialController;

        private void Start()
        {
            tutorialController = GetComponent<TutorialController>();
            swipeable.OnSwipeLeft += tutorial.OnTutorialActionClick;
        }

        private void OnEnable()
        {
            SetLabels(UpdateLabels);
        }

        private void UpdateLabels()
        {
            text1.text = Text("explosion.bonus.tutorial.1");
            text2.text = Text("explosion.bonus.tutorial.2");
        }

        public override void Next(BasePanel current, BasePanel next)
        {
            if (next.name == "EBTPanel1")
            {
                tutorial.background.sprite = background1;
            }
            if (current.name == "EBTPanel1")
            {
                tutorial.background.sprite = background2;
            }
        }
    }
}

