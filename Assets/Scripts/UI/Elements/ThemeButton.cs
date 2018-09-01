using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Assets.Scripts.Localization;
using Assets.Scripts.Game;
using Assets.Scripts.UI.Theme;


namespace Assets.Scripts.UI.Elements
{
    public class ThemeButton : MonoBehaviour
    {

        [SerializeField]
        private Button button;

        [SerializeField]
        private Image backgroundImage,badgeImage;

        [SerializeField]
        private Text badgeText;

        [SerializeField]
        private Color freeColor, buyColor;

        private const float OFFSET_SELECTED = 10;
        private const float OFFSET_UNSELECTED = 5;

        private const float BADGE_UNSELECTED_X = 73;
        private const float BADGE_UNSELECTED_Y = 216.8f;

        public void Setup(bool isFree, bool isSelected, ColorsPreset preset, UnityAction action)
        {
            SetupBadge(isFree);
            SetSelected(isSelected, preset);
            button.onClick.AddListener(action);
            backgroundImage.GetComponent<Image>().sprite = preset.screenshot;
            StartCoroutine(InitialUIThemeApplyCourotine());
        }
        private void OnApplyColorTheme(ColorsPreset preset)
        {
            ColorBlock colors = new ColorBlock();
            colors = button.colors;
            colors.normalColor = preset.uiColorPreset.buttonsColor.color1;
            colors.pressedColor = preset.uiColorPreset.buttonsColor.color2;
            colors.disabledColor = preset.uiColorPreset.buttonsColor.color5;
            button.colors = colors;

        }

        private IEnumerator InitialUIThemeApplyCourotine()
        {
            while (ColorsPresets.Instance == null || !ColorsPresets.Instance.IsReady)
                yield return null;
            OnApplyColorTheme(ColorsPresets.Instance.CurrentPreset);
        }

        private void SetupBadge(bool free)
        {
            if (free)
            {
                badgeImage.gameObject.SetActive(false);
            }
            else
            {
                badgeImage.gameObject.SetActive(true);
                badgeImage.GetComponent<Image>().color = buyColor;
                badgeText.text = LocalizationManager.Instance["buy"];
            }
        }

        public void SetSelected(bool isSelected, ColorsPreset preset)
        {
            button.interactable = !isSelected;
            OnApplyColorTheme(preset);
            if (isSelected)
            {
                backgroundImage.GetComponent<RectTransform>().offsetMin = new Vector2(OFFSET_SELECTED, OFFSET_SELECTED);
                backgroundImage.GetComponent<RectTransform>().offsetMax = new Vector2(-OFFSET_SELECTED, -OFFSET_SELECTED);
            }
            else
            {
                backgroundImage.GetComponent<RectTransform>().offsetMin = new Vector2(OFFSET_UNSELECTED, OFFSET_UNSELECTED);
                backgroundImage.GetComponent<RectTransform>().offsetMax = new Vector2(-OFFSET_UNSELECTED, -OFFSET_UNSELECTED);
            }
        }
    }
}

