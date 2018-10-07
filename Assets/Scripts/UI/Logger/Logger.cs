using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using Assets.Scripts.Game;

namespace Assets.Scripts.UI.Logger
{
    public class Logger : MonoBehaviour
    {
        [SerializeField]
        public Text text;
        [SerializeField]
        private Toggle iapToggle;

        private void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
            StartCoroutine(InitPurchasetoggleCoroutine());
        }
        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }
        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            text.text = text.text + "\n" + logString;
        }

        public void OnClearButtonClick()
        {
            text.text = "";
        }

        public void ClearTransactionLog()
        {
            UnityPurchasing.ClearTransactionLog();
        }

        public void OnCloseclick()
        {
            gameObject.SetActive(false);
        }

        private IEnumerator InitPurchasetoggleCoroutine()
        {
            while (PersistentState.Instance == null)
                yield return null;
            var data = PersistentState.Instance.data;
            iapToggle.isOn = data.adsDisabled || data.customModeUnlocked || data.endlessModeUnlocked || data.themesUnlocked;
        }

        public void DisablePurchases(bool value)
        {
            PersistentState.Instance.data.adsDisabled = value;
            PersistentState.Instance.data.customModeUnlocked = value;
            PersistentState.Instance.data.endlessModeUnlocked = value;
            PersistentState.Instance.data.themesUnlocked = value;
            PersistentState.Instance.Save();
        }
    }
}

