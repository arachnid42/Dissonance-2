using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;
using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.UI.Panels
{
    public class Rate : BasePanel
    {
        [SerializeField]
        private Text tittle = null, placeholder = null, rate = null;
        [SerializeField]
        private GameObject feadbackPanel;
        [SerializeField]
        private InputField inputField;
        [SerializeField]
        private Button submitButton;
        [SerializeField]
        private Animator submitTextAnimator;
        [SerializeField]
        private Color activeButtonColor, inactiveButtonColor;
        [SerializeField]
        private RateStars rateStars;

        private int rateVal;
        private string feadback;
        private bool isReadyToSubmit;

        private void Start()
        {
            rateStars.OnRateChanges = HandleRateChanges;
            IsReadyToSubmit = false;
            rateVal = 0;
        }

        private void OnEnable()
        {
            //rateVal = Mathf.FloorToInt(PersistentState.Instance.data.rating.rating);
            SetLabels(UpdateLabels);
            rateStars.Rating = rateVal;
            HandleRateChanges(rateVal);
            inputField.text = "";
        }

        private void OnDisable()
        {
            rateStars.Rating = 0;
        }

        private void UpdateLabels()
        {
            tittle.text = Text("rateTittle");
            placeholder.text = Text("ratePlaceholder");
            rate.text = Text("rate");

        }

        private void HandleRateChanges(int rate)
        {
            if(rate == 0)
            {
                feadbackPanel.SetActive(false);
                IsReadyToSubmit = false;
                return;
            }else if (rate < 5)
                feadbackPanel.SetActive(true);
            else
                feadbackPanel.SetActive(false);
            IsReadyToSubmit = true;

        }

        private bool IsReadyToSubmit
        {
            get { return isReadyToSubmit; }
            set { isReadyToSubmit = value; UpdateSubmitButton(); }
        }

        private void UpdateSubmitButton()
        {
            if (isReadyToSubmit)
            {
                submitButton.interactable = true;
                submitTextAnimator.enabled = true;
                submitButton.GetComponent<Image>().color = activeButtonColor;
            }
            else
            {
                submitButton.interactable = false;
                submitTextAnimator.enabled = false;
                submitButton.GetComponent<Image>().color = inactiveButtonColor;
            }
        }

        public void OnTextChange(string text)
        {
            feadback = inputField.text;
        }

        public void OnCloseButton()
        {
            SetHidddenAnimation(true).Start();
        }

        public void OnSubmitRateClick()
        {
            PersistentState persistentState = PersistentState.Instance;
            persistentState.data.rating.rating = rateStars.Rating;
            persistentState.SendRating(rateStars.Rating, feadback);
            OnCloseButton();
            MainMenu mainMenu = UIController.Instance.PanelController.mainMenuPanel.GetComponent<MainMenu>();
            mainMenu.OpenAppUrlInMarket();
        }


    }
}

