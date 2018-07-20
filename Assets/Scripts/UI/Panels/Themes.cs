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

        private UnityAction ThemeButtonCallback(int index)
        {
            return () =>
            {
                ColorsPresets.Instance.CurrentPreset = ColorsPresets.Instance[index];
                ColorsPresetsManager.Instance.ApplyCurrentColorPreset();
                StartCoroutine(UpdateThemesSelected());
            };
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
                levelButton.Setup(ColorsPresets.Instance[i].free, ColorsPresets.Instance.CurrentPreset.name== ColorsPresets.Instance[i].name, ColorsPresets.Instance[i].screenshot, ThemeButtonCallback(i));
                themesButtons.Add(newButton);
            }
        }

        private IEnumerator UpdateThemesSelected()
        {
            while (ColorsPresets.Instance == null)
                yield return null;
            for(int i=0; i<themesButtons.Count; i++)
                themesButtons[i].GetComponent<ThemeButton>().SetSelected(ColorsPresets.Instance.CurrentPreset.name == ColorsPresets.Instance[i].name);

        }


    }
}

