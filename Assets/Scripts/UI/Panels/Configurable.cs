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
            UIController.Instance.PanelController.mainMenuPanel.GetComponent<MainMenu>().StartGame();
        }

        public void OnConfigureButtonClick()
        {
            SwitchToAnimation(UIController.Instance.PanelController.ConfigurableMenuPanel, after:()=>GetComponent<ConfigurableMapper>().MapFromConfigurableData()).Start();
        }
        
    }
}

