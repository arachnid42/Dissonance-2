using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;

namespace Assets.Scripts.UI.Panels.Tutorial
{
    public class LifeTutorial : BaseTutorial
    {
        [SerializeField]
        private Tutorial tutorial;
        [SerializeField]
        private Text text1 = null;
        [SerializeField]
        private Sprite lifeBackground;
        private TutorialController tutorialController;

        private void Start()
        {
            tutorialController = GetComponent<TutorialController>();
        }

        private void OnEnable()
        {
            SetLabels(UpdateLabels);
        }

        private void UpdateLabels()
        {
            text1.text = Text("life.bonus.tutorial.1");
        }
        public override void Next(BasePanel current, BasePanel next)
        {
            if (next.name == "LBTPanel1")
            {
                tutorial.background.sprite = lifeBackground;
            }
        }
    }
}

