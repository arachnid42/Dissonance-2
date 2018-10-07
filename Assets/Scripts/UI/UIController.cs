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

        [SerializeField]
        private float loadingDuration = 2f;
        [SerializeField]
        private BasePanel logoFadePanel;

        private PanelController panelController;
        private bool IsInitialized = false;

        private static UIController instance;
        public static UIController Instance
        {
            get { return instance; }
        }

        public PanelController PanelController
        {
            get; private set;
        }
        [System.Serializable]
        public class Data
        {
            public bool isAnimationPlays = false;
            public bool isInMainMenu = true;
            public bool isPlaySound = true;
            public BasePanel activePanel;
            private bool isTrial = false;
            public bool IsTrial
            {
                get { return isTrial; }
                set {
                    Instance.PanelController.trialBarPanel.SetActive(value);
                    isTrial = value;
                }
            }
        }

        public bool OnTrialPblockAction()
        {
            if (Instance.data.IsTrial)
            {
                Instance.PanelController.OverlayPanel.SetHidddenAnimation(false).Start();
            }
            return Instance.data.IsTrial;
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

        private void OnEnable()
        {
            StartCoroutine(InitComponents());
            StartCoroutine(InitUI());
        }

        private IEnumerator InitUI()
        {
            float counter = 0f;
            Debug.Log("Showing Start Up panel");
            var showAnimation = new BasePanel.Animation(Delay(1));
            showAnimation.After(logoFadePanel.SetHiddenEnumerator(false)).Start();
            Debug.Log("Waiting for components loading ");
            while (counter < loadingDuration)
            {
                counter += Time.deltaTime;
                yield return null;
            }
            Debug.Log("Showing Main Menu");
            var showMenu = PanelController.MainMenuPanel.SetHidddenAnimation(false);
            showMenu.After(logoFadePanel.SetHiddenEnumerator(true));
            showMenu.After(PanelController.StartupPanel.SetHiddenEnumerator(true));
            showMenu.Start(true);
        }

        private IEnumerator InitComponents()
        {
            while (Field.Instance == null || 
                Field.Instance.Master == null || 
                Field.Instance.Master.Callbacks == null ||
                Field.Instance.Master.Listeners == null ||
                !ColorsPresets.Ready ||
                !ColorsPresetsManager.Ready ||
                !PersistentState.Ready)
            {
                yield return null;
            }
            IsInitialized = true;
            Debug.Log("INITIALIZED!!!");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                if (Field.Instance.Master.State.started)
                    if (Field.Instance.Master.State.paused)
                        PanelController.CloseOverlayPanel.SetHidddenAnimation(false).Start();
                    else
                        PanelController.CloseOverlayPanel.SetHidddenAnimation(false, after: () => Field.Instance.Master.Actions.Pause()).Start();
                else
                    PanelController.CloseOverlayPanel.SetHidddenAnimation(false).Start();

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

