using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Localization;

namespace Assets.Scripts.UI.Elements
{
    public class Toggle : MonoBehaviour
    {
        [SerializeField]
        private string name = null;
        [SerializeField]
        private bool isOn, allowSwitchOff;

        [SerializeField]
        private Text titleText = null;
        [SerializeField]
        private Image backgroundImage, foregroundImage, contentImage;
        [SerializeField]
        private Sprite image = null;
        [SerializeField]
        private Color backgroundColor, foregroundColor, contentColor;
        [SerializeField]
        private ToggleGroup toggleGroup = null;

        private Color backgroundOffColor;

        private void Start()
        {
            backgroundImage.color = backgroundColor;
            foregroundImage.color = foregroundColor;
            backgroundOffColor = new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, 0);
            if(toggleGroup != null)
            {
                toggleGroup.AddToggleToGroup(this);
            }
            Value = isOn;
        }

        private void OnEnable()
        {
            if(titleText != null)
                titleText.text = name !=null && name != ""? LocalizationManager.Instance[name]: "Title";
            if (contentImage != null)
            {
                contentImage.sprite = image;
                contentImage.color = contentColor;
            }
            Value = isOn;
        }

        public void OnToggleClick()
        {
            if(toggleGroup != null )
            {
                toggleGroup.OnToggleGroup(gameObject.name);

                //if (!isOn)
                //{
                //    Value = !isOn;
                //    toggleGroup.OnToggleGroup(gameObject.name);
                //}
            }
            else
            {
                Value = !isOn;
            }
        }

        public bool Value
        {
            get { return isOn; }
            set {
                //Debug.LogFormat("Toggle {0}, {1}",gameObject.name,value);
                if (allowSwitchOff)
                {
                    isOn = value;
                }
                else
                {
                    isOn = true;
                }
                UpdateValue(); }
        }

        public string Name
        {
            get { return name; }
        }

        private void UpdateValue()
        {
            if (isOn)
            {
                backgroundImage.color = backgroundColor;
            }
            else
            {
                backgroundImage.color = backgroundOffColor;
            }
        }
    }
}

    