using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;
using Assets.Scripts.Monetization;
using Assets.Scripts.Analytics;

namespace Assets.Scripts.UI.Panels
{
    public class Purchase : BasePanel
    {
        [SerializeField]
        private Text tittle, messange, button = null;

        private BasePanel target = null;

        private void OnEnable()
        {
            SetLabels(UpdatePurchaseInformation);
        }

        private void OnDisable()
        {

        }

        private void UpdatePurchaseInformation()
        {
            tittle.text = Text("purchaseTitle");
            button.text = Text("mainMenu");
            target = UIController.Instance.PanelController.MainMenuPanel;
            Events.PurchaseCompleted(PersistentState.Instance.temp.recentPurchase);
            switch (PersistentState.Instance.temp.recentPurchase)
            {
                case Purchases.UNLOCK_ALL:
                    messange.text = Text("unlockAllPurchase");
                    break;
                case Purchases.SKIP:
                    messange.text = Text("skipLevelsPurchase");
                    button.text = Text("newLevels");
                    target = UIController.Instance.PanelController.LevelsMenuPanel;
                    break;
                case Purchases.UNLOCK_ENDLESS:
                    messange.text = Text("unlockEndlessPurchase");
                    button.text = Text("endless");
                    target = UIController.Instance.PanelController.EndlessPanel;
                    break;
                case Purchases.UNLOCK_CONFIGURABLE:
                    messange.text = Text("unlockConfigurablePurchase");
                    button.text = Text("configurableMode");
                    target = UIController.Instance.PanelController.ConfigurablePanel;
                    break;
                case Purchases.UNLOCK_ALL_THEMES:
                    messange.text = Text("unlockThemesPurchase");
                    button.text = Text("newThemes");
                    target = UIController.Instance.PanelController.ThemesPanel;
                    break;
                case Purchases.DISABLE_ADS:
                    messange.text = Text("removeAdsPurchase");
                    break;
                case Purchases.DONATE:
                    messange.text = Text("supportDevsPurchase");
                    break;
            }

        }

        public void OnCloseButton()
        {
            SetHidddenAnimation(true).Start();
        }

        public void OnPurchaseButton()
        {
            SetHidddenAnimation(true).After(UIController.Instance.data.activePanel.SetHiddenEnumerator(true)).After(target.SetHiddenEnumerator(false)).Start();
        }
    }
}

