using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Elements
{
    public class ToggleGroup : MonoBehaviour
    {
        [SerializeField]
        private bool allowSwitchOff;

        
        private List<Toggle> toggles = new List<Toggle>();

        public delegate void ToggleHandler();
        public ToggleHandler Toggle;

        protected void OnToggle()
        {
            if (Toggle != null)
            {
                Toggle();
            }
        }

        private void OnEnable()
        {
            
        }

        public void AddToggleToGroup(Toggle toggle)
        {
            toggles.Add(toggle);
        }

        public void OnToggleGroup(string name)
        {
            //Debug.LogFormat("Toggle Group amount {0}, active: {1}", toggles.Count, name);
            foreach(Toggle toggle in toggles)
            {
                //if (allowSwitchOff)
                //{
                //    toggle.Value = false;
                //}else 
                if (toggle.gameObject.name == name)
                {
                    if (allowSwitchOff)
                        toggle.Value = !toggle.Value;
                    else
                        toggle.Value = true;
                    //Debug.LogFormat("Turn ON {0}", toggle.gameObject.name);
                }
                else
                {
                    if(!allowSwitchOff)
                        toggle.Value = false;
                    //Debug.LogFormat("Switch OFF {0}", toggle.gameObject.name);
                }
            }
            OnToggle();
        }

        public void SetToggleValue(string name, bool val)
        {
            foreach (Toggle toggle in toggles)
            {
                if (toggle.gameObject.name == name)
                {
                    //Debug.LogFormat("Set Toggle {0}, Value: {1}", name, val);
                    toggle.Value = val;

                }
            }
        }

        public void ResetValues()
        {
            foreach (Toggle toggle in toggles)
            {
                    toggle.Value = false;
            }
        }

        public List<Toggle> GetToggles()
        {
            return toggles;
        }

        public List<Toggle> GetActiveToggles()
        {
            List<Toggle> activeToggles = new List<Toggle>();
            foreach (Toggle toggle in toggles)
            {
                if (toggle.Value)
                    activeToggles.Add(toggle);
            }
            //Debug.LogFormat("Return active toggles: {0}",activeToggles.Count);
            return activeToggles;
        }

        public bool AllowSwitchOff
        {
            get { return allowSwitchOff; }
        }
    }
}

    