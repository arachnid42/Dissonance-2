using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Assets.Scripts.UI.Elements
{
    public class LevelButton : MonoBehaviour
    {

        public Button button;
        public Text levelNumber;

        public void Setup(string number, Color color, UnityAction action)
        {
            levelNumber.text = number;
            button.GetComponent<Image>().color = color;
            button.onClick.AddListener(action);
        }
    }
}

