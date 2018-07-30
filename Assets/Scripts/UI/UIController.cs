using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Game;
using Assets.Scripts.UI.Panels;
using Assets.Scripts.Monetization;

namespace Assets.Scripts.UI
{
    public class UIController : MonoBehaviour
    {
        public GameObject SoundButtonImage;
        public Sprite SoundImageOn;
        public Sprite SoundImageOff;

        private PanelController panelController;
        private bool sound;

        delegate void SoundButtonClickDelegate();
        SoundButtonClickDelegate SoundButtonClick;


        private static UIController instance;
        public static UIController Instance
        {
            get { return instance; }
        }

        public PanelController PanelController
        {
            get; private set;
        }

        public class Data
        {
            public bool isAnimationPlays = false;
            public bool isInMainMenu = true;
            public bool isPlaySound = true;
            public BasePanel activePanel;
        }

        public Data data = new Data();

        private void Awake()
        {
            Debug.Log("UIC Awake");
            if (instance == null)
            {
                //DontDestroyOnLoad(gameObject);
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            PanelController = GetComponent<PanelController>();
        }
        // Use this for initialization
        private IEnumerator Start()
        {
            while (Field.Instance == null || Field.Instance.Master.Callbacks == null || Field.Instance.Master.Listeners == null)
            {
                yield return null;
            }
            Field.Instance.Master.Listeners.OnPause += ShowPausePanels;
            Field.Instance.Master.Listeners.OnGameOver += ShowGameOverPanels;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }

        public void OnMainMenuButtonClick()
        {
            data.activePanel.SwitchToAnimation(PanelController.MainMenuPanel).Start();
        }

        private void ShowPausePanels()
        {
            PanelController.PausePanel.SetHidddenAnimation(false).Start();
        }

        private void ShowGameOverPanels(bool win)
        {
            float delay = 2f;
            var gameOverAnim = new BasePanel.Animation(Delay(delay));

            if (win)
            {
                gameOverAnim.After(PanelController.GameOverWinPanel.SetHiddenEnumerator(false, background: PanelController.backgroundPanel));
            }
            else
            {
                gameOverAnim.After(PanelController.GameOverPanel.SetHiddenEnumerator(false, background: PanelController.backgroundPanel));
            }

            if (PersistentState.Instance.ShouldAskRating())
            {
                gameOverAnim.After(PanelController.RatePanel.SetHiddenEnumerator(false));
            }
            else
            {
                gameOverAnim.After(TryToShowAds());
            }

            gameOverAnim.Start();
        }

        private IEnumerator TryToShowAds()
        {
            while (Advertising.Instance == null)
                yield return null;
            Advertising.Instance.TryToShowAds();
        }

        private IEnumerator Delay(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }
    }
}

