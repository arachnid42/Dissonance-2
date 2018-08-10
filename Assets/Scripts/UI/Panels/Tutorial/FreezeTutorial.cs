using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;
using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.UI.Panels.Tutorial
{
    public class FreezeTutorial : BasePanel
    {
        [SerializeField]
        private Tutorial tutorial;
        [SerializeField]
        private Swipeable swipeable;
        [SerializeField]
        private Text text1, text2 = null;
        [SerializeField]
        private Image background;
        [SerializeField]
        private Sprite background1, background2;
        private TutorialController tutorialController;

        private void Start()
        {
            tutorialController = GetComponent<TutorialController>();
            swipeable.OnSwipeRight += OnTutorialActionClick;
        }

        private void OnEnable()
        {
            SetLabels(UpdateLabels);
            background.sprite = background1;
        }

        private void OnDisable()
        {
            // Do not remove
        }

        private void UpdateLabels()
        {
            text1.text = Text("freeze.bonus.tutorial.1");
            text2.text = Text("freeze.bonus.tutorial.2");
        }

        public void OnTutorialActionClick()
        {
            var current = tutorialController.Current;
            var next = tutorialController.Next;
            
            if (next)
            {
                if(current.name == "FBTPanel1")
                {
                    background.sprite = background2;
                }
                current.SwitchToAnimation(next).Start();
            }
            else
            {
                PersistentState.Instance.data.turotiral.freezeBonus = true;
                tutorial.OnFinishTutorial();
            }
        }
    }
}

