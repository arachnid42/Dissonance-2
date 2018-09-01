using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;

namespace Assets.Scripts.UI.Panels
{
    public class MainMenu : BasePanel
    {
        [SerializeField]
        private Text levels = null, endless = null, configurable = null, donate = null, themes = null;
        [SerializeField]
        private float startUpDelay = 2f;

        private string appID;

        private void OnEnable()
        {
            StartCoroutine(AlterUI(InitPanel));
            appID = string.Format("com.{0}.{1}", Application.companyName, Application.productName);

            
        }
        private void InitPanel()
        {
            Debug.LogFormat("{0} : {1}", PersistentState.Instance, UIController.Instance);
            SetLabels(UpdateLabels);
            UIController.Instance.data.activePanel = this;
            //UIController.Instance.PanelController.StartupPanel.SetHidddenAnimation(true).Start();
            //var showMenuAnim = new Animation(Delay(startUpDelay));
            //showMenuAnim.After(UIController.Instance.PanelController.StartupPanel.SetHiddenEnumerator(true));
            //showMenuAnim.Start();

        }

        private void UpdateLabels()
        {
            levels.text = Text("levels");
            endless.text = Text("endless");
            configurable.text = Text("configurableMode");
            donate.text = PersistentState.Instance.data.adsDisabled ? Text("donate") : Text("donateRemoveAds");
            themes.text = Text("themes");
        }

        public void OnPlayButtonClick()
        {
            DifficultyLevels.Instance.LevelIndex = PersistentState.Instance.data.lastLevelIndex;
            StartGame();
        }

        public void OnLevelsButtonClick()
        {
            //PersistentState.Instance.data.levelsUnlocked = DifficultyLevels.Instance.LevelCount;
            UIController.Instance.data.activePanel.SwitchToAnimation(UIController.Instance.PanelController.LevelsMenuPanel).Start();
        }

        public void OnEndlessButtonClick()
        {
            if (PersistentState.Instance.data.endlessModeUnlocked)
            {
                UIController.Instance.data.activePanel.SwitchToAnimation(UIController.Instance.PanelController.EndlessPanel).Start();
            }
            else
            {
                UIController.Instance.data.activePanel.SwitchToAnimation(UIController.Instance.PanelController.EndlessPanel).
                    After(UIController.Instance.PanelController.OverlayPanel.SetHiddenEnumerator(false)).Start();
            }
        }

        public void OnConfigurableButtonClick()
        {
            if (PersistentState.Instance.data.customModeUnlocked)
            {
                UIController.Instance.data.activePanel.SwitchToAnimation(UIController.Instance.PanelController.ConfigurablePanel).Start();
            }
            else
            {
                UIController.Instance.data.activePanel.SwitchToAnimation(UIController.Instance.PanelController.ConfigurablePanel).
                    After(UIController.Instance.PanelController.OverlayPanel.SetHiddenEnumerator(false)).Start();
            }
        }

        public void OnDonateButtonClick()
        {
            UIController.Instance.data.activePanel.SwitchToAnimation(UIController.Instance.PanelController.DonatePanel).Start();
        }

        public void OnThemesButtonClick()
        {
            UIController.Instance.data.activePanel.SwitchToAnimation(UIController.Instance.PanelController.ThemesPanel).Start();
        }

        public void OnAchievementsClick()
        {
            Debug.Log("OnAchievementsClick");
        }

        public void OnShareButtonClick()
        {
#if UNITY_ANDROID
            // Get the required Intent and UnityPlayer classes.
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

            // Construct the intent.
            AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent");
            intent.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            intent.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), string.Format("https://play.google.com/store/apps/details?id={0}", appID));
            intent.Call<AndroidJavaObject>("setType", "text/uri-list");

            // Display the chooser.
            AndroidJavaObject currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intent, "Share");
            currentActivity.Call("startActivity", chooser);
#endif
        }

        public void OnRateButtonClick()
        {
            if (PersistentState.Instance.data.rating.rating <= PersistentState.Instance.config.goodRatingMin)
            {
                UIController.Instance.PanelController.RatePanel.SetHidddenAnimation(false).Start();
            }
            else
            {
                OpenAppUrlInMarket();
            }
        }

        public void OnExitButtonClick()
        {
            UIController.Instance.PanelController.CloseOverlayPanel.SetHidddenAnimation(false).Start();
        }

        public void OpenAppUrlInMarket()
        {
#if UNITY_ANDROID
            Application.OpenURL(string.Format("market://details?id={0}", appID));
#endif
        }

        public void StartGame()
        {
            BasePanel fade = UIController.Instance.PanelController.FadePanel;
            BasePanel startup = UIController.Instance.PanelController.LevelStartUpPanel;
            BasePanel tutorial = UIController.Instance.PanelController.TutorialPanel;
            GameObject background = UIController.Instance.PanelController.backgroundPanel;
            BasePanel lastPanel;

            var startPlay = UIController.Instance.data.activePanel.SwitchToAnimation(fade);
            startPlay.After(fade.SetHiddenEnumerator(true));
            if (ShouldShowTutorial())
            {
                Debug.Log("Show tutorial");
                startPlay.After(tutorial.SetHiddenEnumerator(false));
            }
            else
            {
                if (DifficultyLevels.Instance.CurrentDifficulty.target.endless)
                {
                    lastPanel = fade;
                }
                else
                {
                    startPlay.After(fade.SetHiddenEnumerator(true));
                    startPlay.After(startup.SetHiddenEnumerator(false));
                    lastPanel = startup;
                }
                startPlay.After(lastPanel.SetHiddenEnumerator(true, after: () => {
                    Field.Instance.Master.Restart();
                    UIController.Instance.data.isInMainMenu = false;
                }, background: background));
            }
            startPlay.Start();
        }

        private IEnumerator Delay(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }

        private bool ShouldShowTutorial()
        {
            var tutorialData = PersistentState.Instance.data.turotiral;
            Difficulty currentDifficulty = DifficultyLevels.Instance.CurrentDifficulty;
            return (
                !tutorialData.basic || 
                !tutorialData.explosionBonus && currentDifficulty.ShouldShowExplosionBonusTutorial() ||
                !tutorialData.freezeBonus && currentDifficulty.ShouldShowFreezeBonusTutorial() ||
                !tutorialData.lifeBonus && currentDifficulty.ShouldShowLifeBonusTutorial()
                );
        }

    }
}

