using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Assets.Scripts.Game;
using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.UI.Panels
{
    public class Themes : BasePanel
    {
        [SerializeField]
        private Text title = null, back = null;

        [SerializeField]
        private GameObject ThemeButtonPrefab;

        [SerializeField]
        private Transform ThemesContent;

        private List<GameObject> themesButtons = new List<GameObject>();

        private void OnEnable()
        {
            SetLabels(UpdateLabels);
            UIController.Instance.data.activePanel = this;
            StartCoroutine(UpdateThemes());
        }

        private void UpdateLabels()
        {
            title.text = Text("themes");
            back.text = Text("back");

        }

        private UnityAction ThemeButtonCallback(int index, bool free)
        {
            if (free)
                return () =>
                {
                    PersistentState.Instance.SetColorsPreset(ColorsPresets.Instance[index].name);
                    UpdateThemesSelected(index);
                    Debug.LogFormat("ColorPreset: {0}, color1: {1}", ColorsPresets.Instance.CurrentPreset.name, ColorsPresets.Instance.CurrentPreset.uiColorPreset.buttonsColor.color1);
                };
            else
                return UIController.Instance.PanelController.mainMenuPanel.GetComponent<MainMenu>().OnDonateButtonClick;
        }

        private IEnumerator UpdateThemes()
        {
            while (ColorsPresets.Instance == null)
                yield return null;
            foreach (var button in themesButtons)
                Destroy(button);
            themesButtons.Clear();
            var presetsCount = ColorsPresets.Instance.PresetCount;
            for (int i = 0; i < presetsCount; i++)
            {
                GameObject newButton = GameObject.Instantiate(ThemeButtonPrefab);
                newButton.transform.SetParent(ThemesContent, false);
                ThemeButton levelButton = newButton.GetComponent<ThemeButton>();
                int themeIndex = i;
                bool free = ColorsPresets.Instance[i].free || !ColorsPresets.Instance[i].free  && PersistentState.Instance.data.themesUnlocked;
                levelButton.Setup(free, ColorsPresets.Instance.CurrentPreset.name== ColorsPresets.Instance[i].name, ColorsPresets.Instance[i].screenshot, ThemeButtonCallback(i, free));
                themesButtons.Add(newButton);
            }
        }

        private void UpdateThemesSelected(int index)
        {
            for(int i=0; i<themesButtons.Count; i++)
                themesButtons[i].GetComponent<ThemeButton>().SetSelected(i==index);
        }


    }
}

