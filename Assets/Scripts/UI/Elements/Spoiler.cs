using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Localization;

namespace Assets.Scripts.UI.Elements
{
    public abstract class Spoiler<T> : MonoBehaviour
    {
        [SerializeField]
        private string name;
        [SerializeField]
        private bool isExpanded;

        [SerializeField]
        private Text headerText = null;
        [SerializeField]
        private Button toggleButton;
        [SerializeField]
        private Sprite spoilerClosedImage, spoilerOpenedImage;
        [SerializeField]
        private RectTransform content;
        [SerializeField]
        private Image buttonImage;
        [SerializeField]
        private Color closedColor, openedColor;

        private RectTransform rt;
        private float bottom;

        public abstract T GetData();
        public abstract void SetData(T data);


        private void Start()
        {
            //rt = GetComponent<RectTransform>();
            //bottom = rt.sizeDelta.y;
        }

        private void OnEnable()
        {
            rt = GetComponent<RectTransform>();
            headerText.text = name != ""? LocalizationManager.Instance[name]: "Property";
            bottom = rt.sizeDelta.y;
            ToggleSpoiler();
        }

        private void OnDisable()
        {
            isExpanded = false;
            ToggleSpoiler();
        }

        public void OnToggleButtonClik()
        {
            isExpanded = !isExpanded;
            ToggleSpoiler();
        }

        private void ToggleSpoiler()
        {
            if (isExpanded)
            {
                buttonImage.sprite = spoilerOpenedImage;
                buttonImage.color = openedColor;
                rt.sizeDelta = new Vector2(rt.sizeDelta.x, bottom + content.sizeDelta.y);
            }
            else
            {
                buttonImage.sprite = spoilerClosedImage;
                buttonImage.color = closedColor;
                rt.sizeDelta = new Vector2(rt.sizeDelta.x, bottom);
            }
            //Debug.LogFormat("offsetMin.x: {0}, offsetMin.y: {1}", rt.offsetMin.x, rt.offsetMin.y);
            //Debug.LogFormat("offsetMax.x: {0}, offsetMax.y: {1}", rt.offsetMax.x, rt.offsetMax.y);
            //Debug.LogFormat("sizeDelta.x: {0}, sizeDelta.y: {1}", rt.sizeDelta.x, rt.sizeDelta.y);
        }

    }
}

