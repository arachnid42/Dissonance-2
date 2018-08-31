﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;

namespace Assets.Scripts.UI.Theme
{
    public abstract class BaseThemeListener : MonoBehaviour
    {
        protected void Start()
        {
            StartCoroutine(InitialUIThemeApplyCourotine());
            UIColorsPresets.Instance.OnUIColorPresetApply += OnApplyColorTheme;
        }

        public abstract void OnApplyColorTheme(ColorsPreset preset);

        private IEnumerator InitialUIThemeApplyCourotine()
        {
            while (ColorsPresets.Instance == null || !ColorsPresets.Instance.IsReady)
                yield return null;
            OnApplyColorTheme(ColorsPresets.Instance.CurrentPreset);
        }
    }
}

    