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
        private GameObject SoundButtonImage;

        [SerializeField]
        private Sprite SoundImageOn = null, SoundImageOff = null;

        private string appID;
        
        private void OnEnable()
        {
            StartCoroutine(AlterUI(InitPanel));
            appID = string.Format("com.{0}.{1}", Application.companyName, Application.productName);
            Debug.LogFormat("App ID: {0}", appID);
        }
        private void InitPanel()
        {
            Debug.LogFormat("{0} : {1}", PersistentState.Instance, UIController.Instance);
            SetLabels(UpdateLabels);
            UIController.Instance.data.activePanel = this;
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
            BasePanel fade = UIController.Instance.PanelController.FadePanel;
            BasePanel startup = UIController.Instance.PanelController.LevelStartUpPanel;
            GameObject background = UIController.Instance.PanelController.backgroundPanel;
            DifficultyLevels.Instance.LevelIndex = PersistentState.Instance.data.lastLevelIndex;
            var startPlay = UIController.Instance.data.activePanel.SwitchToAnimation(fade);
            startPlay.After(fade.SetHiddenEnumerator(true));
            startPlay.After(startup.SetHiddenEnumerator(false));
            startPlay.After(startup.SetHiddenEnumerator(true, after:()=> {
                Field.Instance.Master.Restart();
                UIController.Instance.data.isInMainMenu = false;
            }, background:background));
            startPlay.Start();
        }

        public void OnLevelsButtonClick()
        {
            PersistentState.Instance.data.levelsUnlocked = DifficultyLevels.Instance.LevelCount;
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
                OnDonateButtonClick();
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
                OnDonateButtonClick();
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

        public void OnSoundButtonClick()
        {
            UIController.Instance.data.isPlaySound = !UIController.Instance.data.isPlaySound;
            if (UIController.Instance.data.isPlaySound)
            {
                SoundButtonImage.GetComponent<Image>().sprite = SoundImageOn;
            }
            else
            {
                SoundButtonImage.GetComponent<Image>().sprite = SoundImageOff;
            }
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
            Application.Quit();
        }

        public void OpenAppUrlInMarket()
        {
#if UNITY_ANDROID
            Application.OpenURL(string.Format("market://details?id={0}", appID));
#endif
        }

    }
}

