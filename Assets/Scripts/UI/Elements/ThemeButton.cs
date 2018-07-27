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

        private const float BADGE_UNSELECTED_X = 73;
        private const float BADGE_UNSELECTED_Y = 216.8f;
        private const float BADGE_SELECTED_X = 68;
        private const float BADGE_SELECTED_Y = 211.8f;

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
                badgeImage.gameObject.SetActive(false);
                //badgeImage.GetComponent<Image>().color = freeColor;
                //badgeText.text = LocalizationManager.Instance["free"];
            }
            else
            {
                badgeImage.gameObject.SetActive(true);
                badgeImage.GetComponent<Image>().color = buyColor;
                badgeText.text = LocalizationManager.Instance["buy"];
            }
        }

        public void SetSelected(bool isSelected)
        {
            button.interactable = !isSelected;
            RectTransform badge = badgeImage.GetComponent<RectTransform>();
            Debug.LogFormat("offsetMin: {0} offsetMax: {1}",badge.offsetMin,badge.offsetMax);
            //Debug.LogFormat("rect: {0} position: {1}", badge.rect,badge.position);
            if (isSelected)
            {
                backgroundImage.GetComponent<RectTransform>().offsetMin = new Vector2(OFFSET_SELECTED, OFFSET_SELECTED);
                backgroundImage.GetComponent<RectTransform>().offsetMax = new Vector2(-OFFSET_SELECTED, -OFFSET_SELECTED);
                badgeImage.GetComponent<RectTransform>().offsetMin = new Vector2(BADGE_SELECTED_X, BADGE_SELECTED_Y);

            }
            else
            {
                backgroundImage.GetComponent<RectTransform>().offsetMin = new Vector2(OFFSET_UNSELECTED, OFFSET_UNSELECTED);
                backgroundImage.GetComponent<RectTransform>().offsetMax = new Vector2(-OFFSET_UNSELECTED, -OFFSET_UNSELECTED);
                badgeImage.GetComponent<RectTransform>().offsetMin = new Vector2(BADGE_UNSELECTED_X, BADGE_UNSELECTED_Y);

            }
        }
    }
}

