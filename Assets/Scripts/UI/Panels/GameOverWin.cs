using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;


namespace Assets.Scripts.UI.Panels
{
    public class GameOverWin : BasePanel
    {
        [SerializeField]
        private Text win = null, congrats = null, levelsUnlocked = null,
            next = null, rate = null, share = null, donate = null, mainMenu = null;

        [SerializeField]
        private GameObject winText, congratsText, levelsUnlockedText;

        private bool hasNextLevel, isConfigurable;

        private void OnEnable()
        {
            UIController.Instance.data.activePanel = this;
            isConfigurable = DifficultyLevels.Instance.CurrentDifficulty.name == "Configurable";
            hasNextLevel = NextLevel();
            SetLabels(UpdateLabels);
        }

        private bool NextLevel()
        {
            int lastIndex = PersistentState.Instance.data.lastLevelIndex;
            int levelsCount = DifficultyLevels.Instance.LevelCount;
            int levelsUnlocked = PersistentState.Instance.data.levelsUnlocked;

            lastIndex = Mathf.Clamp(lastIndex + 1, 0, levelsCount);

            PersistentState.Instance.data.levelsUnlocked = Mathf.Clamp(lastIndex+1, levelsUnlocked, levelsCount);
            PersistentState.Instance.data.lastLevelIndex = lastIndex;

            PersistentState.Instance.Save();

            return PersistentState.Instance.data.lastLevelIndex < levelsCount;
        }

        private void UpdateLabels()
        {
            winText.SetActive(true);
            congratsText.SetActive(false);
            levelsUnlockedText.SetActive(false);
            win.text = Text("winText");

            if (isConfigurable)
            {
                next.text = Text("replay");
            }
            else
            {
                if (hasNextLevel)
                {
                    next.text = string.Format("{0} {1}", Text("next"), PersistentState.Instance.data.lastLevelIndex + 1);
                }
                else
                {
                    winText.SetActive(false);
                    congratsText.SetActive(true);
                    levelsUnlockedText.SetActive(true);
                    congrats.text = Text("congrats");
                    levelsUnlocked.text = Text("levelsUnlocked");
                    next.text = Text("playEndless");
                    PersistentState.Instance.data.endlessModeUnlocked = true;
                    PersistentState.Instance.data.customModeUnlocked = true;
                    PersistentState.Instance.data.lastLevelIndex--;
                    PersistentState.Instance.Save();
                }
            }
            
            rate.text = Text("rate");
            share.text = Text("share");
            donate.text = PersistentState.Instance.data.adsDisabled ? Text("donate") : Text("donateRemoveAds");
            mainMenu.text = Text("mainMenu");
            
        }

        public void OnNextPlayButtonClick()
        {
            if (isConfigurable)
            {
                UIController.Instance.PanelController.configurablePanel.GetComponent<Configurable>().OnCofigurablePlayClick();
                return;
            }
            if (hasNextLevel)
                UIController.Instance.PanelController.mainMenuPanel.GetComponent<MainMenu>().OnPlayButtonClick();
            else
                UIController.Instance.PanelController.mainMenuPanel.GetComponent<MainMenu>().OnEndlessButtonClick();
        }

    }
}

