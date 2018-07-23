using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Localization;

namespace Assets.Scripts.UI.Elements
{
    public class Slider : MonoBehaviour
    {
        private const float ROUND_TO_INT_DELTA = 0.001f;

        [System.Serializable]
        public class Data
        {
            public float min, max, value, step;
        }
        [SerializeField]
        private string name = null, measure = null;

        [SerializeField]
        private float min, max, _value, step;

        [SerializeField]
        private Text headerText = null, valueText = null;

        [SerializeField]
        private Slider lowerBind = null, upperBind = null;

        private const float VALUE_CHANGE_DELAY = 0.1f;

        private void OnEnable()
        {
            headerText.text = name !=null && name != ""? LocalizationManager.Instance[name]: "Property";
            if (lowerBind != null)
                Min = lowerBind.Value;
            if (upperBind != null)
                Max = upperBind.Value;
            UpdateValue();
        }

        public void OnLeftButtonClik()
        {
            float val = _value - step;
            UpdateBindValues(val);
            Value = val;
        }

        public void OnRightButtonClik()
        {
            float val = _value + step;
            UpdateBindValues(val);
            Value = val;
        }

        public float Min
        {
            get { return min; }
            set { min = value; UpdateValue(); }
        }

        public float Max
        {
            get { return max; }
            set { max = value; UpdateValue(); }
        }

        public float Value
        {
            get { return _value; }
            set { _value = value; UpdateValue(); }
        }

        public float Step
        {
            get { return step; }
            set { step = value; }
        }

        public void SetData(Data data)
        {
            Min = data.min;
            Max = data.max;
            Value = data.value;
            Step = data.step;

        }

        private void UpdateValue()
        {
            if (_value < min)
            {
                _value = min;
            }
            else if (_value > max)
            {
                _value = max;
            }
            string newText = null;
            if(measure !=null && measure != "")
            {
                string measureFinal = LocalizationManager.Instance[measure].ToLower();
                newText = string.Format("{0:0.##} {1}", measureFinal == "%" ? (_value * 100) : _value, measureFinal);
            }
            else
            {
                newText = string.Format("{0:0.##}", _value);
            }
            valueText.text = newText;
        }

        private bool UpdateBindValues(float val)
        {
            bool bindValueUpdated = false;
            if (lowerBind != null)
            {
                if(val >= Min && val <= Max)
                {
                    lowerBind.Max = val;
                    if (lowerBind.Value > val)
                    {
                        lowerBind.Value = val;
                        bindValueUpdated = true;
                    }
                }
            }
            if (upperBind != null)
            {
                if (val >= Min && val <= Max)
                {
                    upperBind.Min = val;
                    if (upperBind.Value < val)
                    {
                        upperBind.Value = val;
                        bindValueUpdated = true;
                    }
                }
                
            }
            return bindValueUpdated;
        }

        public void OnLeftPointerDown()
        {
            StartCoroutine(DecreaseValueCourotine(VALUE_CHANGE_DELAY));
        }

        public void OnLeftPointerUp()
        {
            StopAllCoroutines();
        }
        public void OnRightPointerDown()
        {
            StartCoroutine(IncreaseValueCourotine(VALUE_CHANGE_DELAY));

        }

        public void OnRightPointerUp()
        {
            StopAllCoroutines();

        }

        private IEnumerator DecreaseValueCourotine(float seconds)
        {
            while (true)
            {
                OnLeftButtonClik();
                yield return new WaitForSeconds(seconds);
            }
        }

        private IEnumerator IncreaseValueCourotine(float seconds)
        {
            while (true)
            {
                OnRightButtonClik();
                yield return new WaitForSeconds(seconds);
            }
        }
    }
}

    