using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;
using Assets.Scripts.UI.Mapping;

namespace Assets.Scripts.UI.Panels
{
    public class Configurable : BasePanel
    {
        [SerializeField]
        private Text title = null, play = null, configure = null, back = null;

        private void OnEnable()
        {
            SetLabels(UpdateLabels);
            UIController.Instance.data.activePanel = this;
        }

        private void UpdateLabels()
        {
            title.text = Text("configurableMode");
            play.text = Text("play");
            configure.text = Text("configure");
            back.text = Text("back");

        }

        public void OnCofigurablePlayClick()
        {
            DifficultyLevels.Instance.LevelName = "Configurable";
            StartGame();
        }

        public void OnConfigureButtonClick()
        {
            SwitchToAnimation(UIController.Instance.PanelController.ConfigurableMenuPanel, after:()=>GetComponent<ConfigurableMapper>().MapFromConfigurableData()).Start();
        }


        private void StartGame()
        {
            BasePanel fade = UIController.Instance.PanelController.FadePanel;
            BasePanel startup = UIController.Instance.PanelController.LevelStartUpPanel;
            BasePanel lastPanel;
            GameObject background = UIController.Instance.PanelController.backgroundPanel;

            var startPlay = UIController.Instance.data.activePanel.SwitchToAnimation(fade);
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
            startPlay.Start();
        }


    }
}

