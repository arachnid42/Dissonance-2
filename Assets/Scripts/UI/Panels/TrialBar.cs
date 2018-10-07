using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Panels
{
    public class TrialBar : BasePanel
    {

        [SerializeField]
        private Text text = null;

        private void OnEnable()
        {
            SetLabels(UpdateLabels);
        }

        private void OnDisable()
        {
            
        }

        private void UpdateLabels()
        {
            text.text = Text("trialAccess");
        }

    }
}

