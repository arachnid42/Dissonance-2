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
        private Text tittle = null, feedback = null, rate = null;
        [SerializeField]
        private GameObject buttonsPanel;
        [SerializeField]
        private Button submitButton, feedbackButton;
        [SerializeField]
        private Animator submitTextAnimator;
        [SerializeField]
        private RateStars rateStars;

        private int rateVal;
        private string EMAIL = "arachnid42.games.feedback@gmail.com";
        private string EMAIL_SUBJECT = "Dissonance 2 Feadback";

        private void Start()
        {
            rateStars.OnRateChanges = HandleRateChanges;
        }

        private void OnEnable()
        {
            rateVal = 0;
            SetLabels(UpdateLabels);
            rateStars.Rating = rateVal;
            HandleRateChanges(rateVal);
        }

        private void OnDisable()
        {
            rateStars.Rating = 0;
        }

        private void UpdateLabels()
        {
            tittle.text = Text("rateTittle");
            feedback.text = Text("feedbackButton");
            rate.text = Text("rate");

        }

        private void HandleRateChanges(int rate)
        {
            if(rate == 0)
            {
                buttonsPanel.SetActive(false);
            }
            else
            {
                buttonsPanel.SetActive(true);
                if (rate < PersistentState.Instance.config.goodRatingMin)
                {
                    feedbackButton.gameObject.SetActive(true);
                    submitButton.gameObject.SetActive(false);
                }
                else
                {
                    feedbackButton.gameObject.SetActive(false);
                    submitButton.gameObject.SetActive(true);
                }
            }

        }

        public void OnCloseButton()
        {
            SetHidddenAnimation(true).Start();
        }

        public void OnRateButtonClick()
        {
            PersistentState persistentState = PersistentState.Instance;
            persistentState.data.rating.rating = rateStars.Rating;
            persistentState.SendRating(rateStars.Rating, "Good rating");
            OnCloseButton();
            MainMenu mainMenu = UIController.Instance.PanelController.mainMenuPanel.GetComponent<MainMenu>();
            mainMenu.OpenAppUrlInMarket();
        }

        public void OnFeedbackButtonClick()
        {
            PersistentState persistentState = PersistentState.Instance;
            persistentState.data.rating.rating = rateStars.Rating;
            persistentState.SendRating(rateStars.Rating, "Bad rating");
            OnCloseButton();
            SendFeedbackMail();
        }

        private void SendFeedbackMail()
        {
            Application.OpenURL(string.Format("mailto:{0}?subject={1}[{2}]",EMAIL,EMAIL_SUBJECT,rateStars.Rating.ToString()));
        }
    }
}

