using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;
using Assets.Scripts.UI.Mapping;


namespace Assets.Scripts.UI.Panels
{
    public class ConfigurableMenu : BasePanel
    {
        [SerializeField]
        private Text title = null, startingMode = null, target = null, 
            save = null, revert = null, back = null;

        [SerializeField]
        private ConfigurableMapper configurableMapper;

        private void OnEnable()
        {
            UIController.Instance.data.activePanel = this;
            SetLabels(UpdateLabels);
        }

        private void OnDisable()
        {
            configurableMapper.MapFromConfigurableData();
        }

        private void UpdateLabels()
        {
            title.text = Text("configurableMode");
            startingMode.text = Text("startingMode");
            target.text = Text("target");
            save.text = Text("save");
            revert.text = Text("revert");
            back.text = Text("back");

        }

        
        public void OnSaveButtonClick()
        {
            configurableMapper.MapToConfigurableData();
            PersistentState.Instance.ApplyConfigurableModeData();
            OnBackMenuButtonClick();
        }

        public void OnRevertButtonClick()
        {
            PersistentState.Instance.ResetConfigurableModeData();
            configurableMapper.MapFromConfigurableData();
        }

        public void OnBackMenuButtonClick()
        {
            SwitchToAnimation(UIController.Instance.PanelController.ConfigurablePanel).Start();
        }

        
    }
}

