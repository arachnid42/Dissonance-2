using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Assets.Scripts.Localization;

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

        public void Setup(bool isFree, bool isSelected, Sprite background, UnityAction action)
        {
            SetupBadge(isFree);
            SetSelected(isSelected);
            button.onClick.AddListener(action);
            backgroundImage.GetComponent<Image>().sprite = background;
        }

        private void SetupBadge(bool free)
        {
            if (free)
            {
                badgeImage.GetComponent<Image>().color = freeColor;
                badgeText.text = LocalizationManager.Instance["free"];
            }
            else
            {
                badgeImage.GetComponent<Image>().color = buyColor;
                badgeText.text = LocalizationManager.Instance["buy"];
            }
        }

        public void SetSelected(bool isSelected)
        {
            button.interactable = !isSelected;
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

