using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Assets.Scripts.Game;
using Assets.Scripts.UI.Elements;


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
        private Color passedColor, currentColor;

        private List<GameObject> levelsButtons = new List<GameObject>();

        private void OnEnable()
        {
            SetLabels(UpdateLabels);
            StartCoroutine(UpdateLevels());
            UIController.Instance.data.activePanel = this;
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
            Color color = passedColor;
            for (int i = PersistentState.Instance.data.levelsUnlocked-1; i >= 0 ; i--)
            {
                color = PersistentState.Instance.data.lastLevelIndex == i ? currentColor : passedColor;
                GameObject newButton = GameObject.Instantiate(LevelButtonPrefab);
                newButton.transform.SetParent(LevelsContent, false);
                LevelButton levelButton = newButton.GetComponent<LevelButton>();
                int levelIndex = i;
                levelButton.Setup((i+1).ToString(), color, LevelButtonCallback(i));
                levelsButtons.Add(newButton);
            }
        }


    }
}

