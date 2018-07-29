using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;
using Assets.Scripts.Monetization;

namespace Assets.Scripts.UI.Panels
{
    public class Purchase : BasePanel
    {
        [SerializeField]
        private Text tittle, messange, button = null;

        public System.Action OnPurchase = () => Debug.LogFormat("On Purchase button");

        private void OnEnable()
        {
            SetLabels(UpdatePurchaseInformation);
        }

        private void UpdatePurchaseInformation()
        {
            tittle.text = Text("purchaseTitle");
            switch (PersistentState.Instance.temp.recentPurchase)
            {
                case Purchases.UNLOCK_ALL:
                    messange.text = Text("unlockAllPurchase");
                    button.text = Text("close");
                    //OnPurchase = OnCloseButton;
                    break;
                case Purchases.SKIP:
                    messange.text = Text("skipLevelsPurchase");
                    button.text = Text("clnewLevelsose");
                    OnPurchase = UIController.Instance.PanelController.mainMenuPanel.GetComponent<MainMenu>().OnLevelsButtonClick;
                    break;
                case Purchases.UNLOCK_ENDLESS:
                    messange.text = Text("unlockEndlessPurchase");
                    button.text = Text("endless");
                    OnPurchase = UIController.Instance.PanelController.mainMenuPanel.GetComponent<MainMenu>().OnEndlessButtonClick;
                    break;
                case Purchases.UNLOCK_CONFIGURABLE:
                    messange.text = Text("unlockConfigurablePurchase");
                    button.text = Text("configurableMode");
                    OnPurchase = UIController.Instance.PanelController.mainMenuPanel.GetComponent<MainMenu>().OnConfigurableButtonClick;
                    break;
                case Purchases.UNLOCK_ALL_THEMES:
                    messange.text = Text("unlockThemesPurchase");
                    button.text = Text("newThemes");
                    OnPurchase = UIController.Instance.PanelController.mainMenuPanel.GetComponent<MainMenu>().OnThemesButtonClick;
                    break;
                case Purchases.DISABLE_ADS:
                    messange.text = Text("removeAdsPurchase");
                    button.text = Text("close");
                    //OnPurchase = OnCloseButton;
                    break;
                case Purchases.DONATE:
                    messange.text = Text("supportDevsPurchase");
                    button.text = Text("close");
                    //OnPurchase = OnCloseButton;
                    break;
                default:
                    button.text = Text("close");
                    //OnPurchase = OnCloseButton;
                    break;
            }

        }

        public void OnCloseButton()
        {
            SetHidddenAnimation(true).Start();
        }

        public void OnPurchaseButton()
        {
            OnCloseButton();
            OnPurchase();
        }
    }
}

