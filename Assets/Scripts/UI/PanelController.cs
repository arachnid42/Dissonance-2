using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.UI.Panels;

namespace Assets.Scripts.UI
{
    public class PanelController : MonoBehaviour
    {
        
        public GameObject mainMenuCanvas, backgroundPanel, mainMenuPanel, endlessPanel,
            levelsMenuPanel, configurablePanel, configurableMenuPanel, donatePanel, donateMenuPanel, themesPanel,
            levelStartUpPanel, gameOverWinPanel, gameOverPanel, pausePanel, fadePanel, ratePanel, purchasePanel, overlayPanel, closeOverlayPanel, tutorialPanel, startupPanel;

        public BasePanel BackgroundPanel
        {
            get { return backgroundPanel.GetComponent<BasePanel>(); }
        }

        public BasePanel MainMenuPanel
        {
            get { return mainMenuPanel.GetComponent<BasePanel>(); }
        }

        public BasePanel EndlessPanel
        {
            get { return endlessPanel.GetComponent<BasePanel>(); }
        }

        public BasePanel LevelsMenuPanel
        {
            get { return levelsMenuPanel.GetComponent<BasePanel>(); }
        }

        public BasePanel ConfigurablePanel
        {
            get { return configurablePanel.GetComponent<BasePanel>(); }
        }

        public BasePanel ConfigurableMenuPanel
        {
            get { return configurableMenuPanel.GetComponent<BasePanel>(); }
        }

        public BasePanel DonatePanel
        {
            get { return donatePanel.GetComponent<BasePanel>(); }
        }

        public BasePanel DonateMenuPanel
        {
            get { return donateMenuPanel.GetComponent<BasePanel>(); }
        }

        public BasePanel ThemesPanel
        {
            get { return themesPanel.GetComponent<BasePanel>(); }
        }

        public BasePanel LevelStartUpPanel
        {
            get { return levelStartUpPanel.GetComponent<BasePanel>(); }
        }

        public BasePanel GameOverWinPanel
        {
            get { return gameOverWinPanel.GetComponent<BasePanel>(); }
        }

        public BasePanel GameOverPanel
        {
            get { return gameOverPanel.GetComponent<BasePanel>(); }
        }

        public BasePanel PausePanel
        {
            get { return pausePanel.GetComponent<BasePanel>(); }
        }

        public BasePanel FadePanel
        {
            get { return fadePanel.GetComponent<BasePanel>(); }
        }

        public BasePanel RatePanel
        {
            get { return ratePanel.GetComponent<BasePanel>(); }
        }

        public BasePanel PurchasePanel
        {
            get { return purchasePanel.GetComponent<BasePanel>(); }
        }

        public BasePanel OverlayPanel
        {
            get { return overlayPanel.GetComponent<BasePanel>(); }
        }

        public BasePanel CloseOverlayPanel
        {
            get { return closeOverlayPanel.GetComponent<BasePanel>(); }
        }

        public BasePanel TutorialPanel
        {
            get { return tutorialPanel.GetComponent<BasePanel>(); }
        }

        public BasePanel StartupPanel
        {
            get { return startupPanel.GetComponent<BasePanel>(); }
        }

    }
}