using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;

namespace Assets.Scripts.UI.Panels.Tutorial
{
    public class TutorialController : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> screens;

        private int index;


        private void OnEnable()
        {
            ResetTutorial();
        }

        public BasePanel Current
        {
            get; private set;
        }

        public BasePanel Next
        {
            get
            {
                Debug.LogFormat("index: {0} Count: {1}", index, screens.Count);

                if (index + 1 < screens.Count && !UIController.Instance.data.isAnimationPlays)
                {
                    Current = screens[++index].GetComponent<BasePanel>();
                    return Current;
                }
                return null;
            }
        }

        public void ResetTutorial()
        {
            index = 0;
            Current = screens[index].GetComponent<BasePanel>();
            for(int i=0; i<screens.Count; i++)
            {
                if (i == 0)
                {
                    screens[i].gameObject.SetActive(true);
                    screens[i].gameObject.GetComponent<CanvasGroup>().alpha = 1;
                }
                else
                    screens[i].gameObject.SetActive(false);
            }
        } 
    }
}

