using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Purchasing;

namespace Assets.Scripts.UI.Logger
{
    public class Logger : MonoBehaviour
    {
        [SerializeField]
        public Text text;

        private void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
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
    }
}

