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

        public Button button;
        public Text levelNumber;

        private Color normal, highlighted;
        
        public void Setup(string number, bool isHighlighted, UnityAction action)
        {
            levelNumber.text = number;
            button.onClick.AddListener(action);
            StartCoroutine(InitialUIThemeApplyCourotine(isHighlighted));
        }

        private IEnumerator InitialUIThemeApplyCourotine(bool isHighlighted)
        {
            while (ColorsPresets.Instance == null || !ColorsPresets.Instance.IsReady)
                yield return null;
            var preset = ColorsPresets.Instance.CurrentPreset;
            normal = preset.uiColorPreset.buttonsColor.color1;
            highlighted = preset.uiColorPreset.buttonsColor.color2;
            button.GetComponent<Image>().color = isHighlighted ? highlighted : normal;

        }
    }
}

