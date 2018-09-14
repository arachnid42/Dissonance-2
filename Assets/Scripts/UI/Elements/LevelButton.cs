using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Assets.Scripts.UI.Theme;
using Assets.Scripts.Game;

namespace Assets.Scripts.UI.Elements
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField]
        private Button button;
        [SerializeField]
        private Text levelNumber;
        [SerializeField]
        private Image lockImage;

        private Color normal, highlighted;
        
        public void Setup(string number, bool isHighlighted, bool isLocked, UnityAction action)
        {
            if (isLocked)
            {
                levelNumber.gameObject.SetActive(false);
                lockImage.gameObject.SetActive(true);
                button.onClick.RemoveAllListeners();
            }
            else
            {
                lockImage.gameObject.SetActive(false);
                levelNumber.gameObject.SetActive(true);
                levelNumber.text = number;
                button.onClick.AddListener(action);
            }
            StartCoroutine(InitialUIThemeApplyCourotine(isHighlighted, isLocked));
        }

        private IEnumerator InitialUIThemeApplyCourotine(bool isHighlighted, bool isLocked)
        {
            while (ColorsPresets.Instance == null || !ColorsPresets.Instance.IsReady)
                yield return null;
            var preset = ColorsPresets.Instance.CurrentPreset;
            if (isLocked)
            {
                button.GetComponent<Image>().color = preset.uiColorPreset.buttonsColor.color5;
                lockImage.color = preset.uiColorPreset.textColor.secondary1;
            }
            else
            {
                normal = preset.uiColorPreset.buttonsColor.color1;
                highlighted = preset.uiColorPreset.buttonsColor.color2;
                button.GetComponent<Image>().color = isHighlighted ? highlighted : normal;
                levelNumber.color = preset.uiColorPreset.textColor.secondary1;
            }
        }
    }
}

