using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Game;
using Assets.Scripts.UI.Panels;

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
            //Field.Instance.gameObject.SetActive(false);
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
            if (win)
            {
                var gameOverAnim = new BasePanel.Animation(Delay(delay));
                gameOverAnim.After(PanelController.GameOverWinPanel.SetHiddenEnumerator(false, background: PanelController.backgroundPanel));
                gameOverAnim.Start();
            }
            else
            {
                var gameOverAnim = new BasePanel.Animation(Delay(delay));
                gameOverAnim.After(PanelController.GameOverPanel.SetHiddenEnumerator(false, background: PanelController.backgroundPanel));
                gameOverAnim.Start();
            }
        }

        private IEnumerator Delay(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }
    }
}

