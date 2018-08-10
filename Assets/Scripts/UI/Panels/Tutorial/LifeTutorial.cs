using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;

namespace Assets.Scripts.UI.Panels.Tutorial
{
    public class LifeTutorial : BasePanel
    {
        [SerializeField]
        private Tutorial tutorial;
        [SerializeField]
        private Text text1 = null;
        private TutorialController tutorialController;

        private void Start()
        {
            tutorialController = GetComponent<TutorialController>();
        }

        private void OnEnable()
        {
            SetLabels(UpdateLabels);
        }

        private void OnDisable()
        {
            // Do not remove
        }

        private void UpdateLabels()
        {
            text1.text = Text("life.bonus.tutorial.1");
        }

        public void OnTutorialActionClick()
        {
            var current = tutorialController.Current;
            var next = tutorialController.Next;
            
            if (next)
            {
                current.SwitchToAnimation(next).Start();
            }
            else
            {
                PersistentState.Instance.data.turotiral.lifeBonus = true;
                tutorial.OnFinishTutorial();
            }
        }
    }
}

