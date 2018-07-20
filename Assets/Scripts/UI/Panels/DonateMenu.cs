using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Panels
{
    public class DonateMenu : BasePanel
    {

        [SerializeField]
        private Text donate = null, 
            back = null;

        private void OnEnable()
        {
            SetLabels(UpdateLabels);
            UIController.Instance.data.activePanel = this;
        }

        private void UpdateLabels()
        {
            donate.text = Text("donate");
            back.text = Text("back");
        }


    }
}

