using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;
using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.UI.Panels.Tutorial
{
    public class GeneralTutorial : BaseTutorial
    {
        [SerializeField]
        private Tutorial tutorial;
        [SerializeField]
        private Swipeable swipeable;
        [SerializeField]
        private Sprite backgroundShape, backgroundColor;
        [SerializeField]
        private Text gameMode, pause, resume, tap1, tap2, text1, text2, text3, text4, text5, text6, text7, text8, text9, text10 = null;
        private TutorialController tutorialController;

        private void Start()
        {
            tutorialController = GetComponent<TutorialController>();
            swipeable.OnSwipeDown += tutorial.OnTutorialActionClick;
            swipeable.OnSwipeUp += tutorial.OnTutorialActionClick;
        }

        private void OnEnable()
        {
            SetLabels(UpdateLabels);
        }

        private void UpdateLabels()
        {
            gameMode.text = Text("shape");
            pause.text = Text("pause");
            resume.text = Text("resume");
            tap1.text = tap2.text = Text("tap");
            //text1.text = Text("general.tutorial.1");
            //text2.text = Text("general.tutorial.2");
            //text3.text = Text("general.tutorial.3");
            //text4.text = Text("general.tutorial.4");
            //text5.text = Text("general.tutorial.5");
            //text6.text = Text("general.tutorial.6");
            //text7.text = Text("general.tutorial.7");
            //text8.text = Text("general.tutorial.8");
            text9.text = Text("general.tutorial.9");
            text10.text = Text("general.tutorial.10");

        }

        public override void Next(BasePanel current, BasePanel next)
        {
            if (current.name == "GTPanelShape")
            {
                tutorial.background.sprite = backgroundShape;
                gameMode.text = Text("shape");
            }
            if (next.name == "GTPanelColor")
            {
                tutorial.background.sprite = backgroundColor;
                gameMode.text = Text("color");
            }
        }
    }
}

