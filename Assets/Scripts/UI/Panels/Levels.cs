using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Assets.Scripts.Game;
using Assets.Scripts.UI.Elements;
using Assets.Scripts.Analytics;

namespace Assets.Scripts.UI.Panels
{
    public class Levels : BasePanel
    {
        [SerializeField]
        private Text levels = null, back = null;

        [SerializeField]
        private GameObject LevelButtonPrefab;

        [SerializeField]
        private Transform LevelsContent;

        [SerializeField]
        private ScrollRect scrollRect;

        private const float ROW_LENGTH = 4f;
        private const float SCROLL_MIDDLE = 0.5f;
        private float precision;
        private List<GameObject> levelsButtons = new List<GameObject>();

        private void OnEnable()
        {
            Events.PanelOpened(Events.Panels.LEVELS);
            UIController.Instance.data.activePanel = this;
            SetLabels(UpdateLabels);
            StartCoroutine(UpdateLevels());
            float totalLevels = (float)DifficultyLevels.Instance.LevelCount;
            float scrollPosition = 1 - (float)(PersistentState.Instance.data.lastLevelIndex + 1) / totalLevels;
            precision = ROW_LENGTH / totalLevels;
            scrollPosition = scrollPosition < SCROLL_MIDDLE ? scrollPosition - precision : scrollPosition + precision;
            StartCoroutine(SetScrollRectPosition(scrollPosition));
        }

        private IEnumerator SetScrollRectPosition(float position)
        {
            yield return new WaitForEndOfFrame();
            scrollRect.verticalNormalizedPosition = Mathf.Clamp(position,0,1);

        }

        private void UpdateLabels()
        {
            levels.text = Text("levels");
            back.text = Text("back");
        }

        private UnityAction LevelButtonCallback(int index)
        {
            return () =>
            {
                DifficultyLevels.Instance.LevelIndex = index;
                PersistentState.Instance.data.lastLevelIndex = index;
                UIController.Instance.PanelController.mainMenuPanel.GetComponent<MainMenu>().OnPlayButtonClick();
            };
        }

        private IEnumerator UpdateLevels()
        {
            while (PersistentState.Instance == null)
                yield return null;
            foreach (var button in levelsButtons)
                Destroy(button);
            levelsButtons.Clear();
            for (int i = 0; i < DifficultyLevels.Instance.LevelCount; i++)
                {
                GameObject newButton = GameObject.Instantiate(LevelButtonPrefab);
                newButton.transform.SetParent(LevelsContent, false);
                LevelButton levelButton = newButton.GetComponent<LevelButton>();
                int levelIndex = i;
                levelButton.Setup((i+1).ToString(), PersistentState.Instance.data.lastLevelIndex == i, i>=PersistentState.Instance.data.levelsUnlocked, LevelButtonCallback(i));
                levelsButtons.Add(newButton);
            }
        }


    }
}

