using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Localization;
using UnityEngine.EventSystems;

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
        private GameObject leftButton, rightButton;

        [SerializeField]
        private Text headerText = null, valueText = null;

        [SerializeField]
        private Slider lowerBind = null, upperBind = null;

        private const float VALUE_CHANGE_DELAY = 0.4f;
        private const float VALUE_CHANGE_DURATION = 1f;

        private void Start()
        {
            leftButton = transform.Find("LeftButton").gameObject;
            rightButton = transform.Find("RightButton").gameObject;
            if (leftButton != null && rightButton != null)
            {
                EventTrigger ltrigger = leftButton.AddComponent<EventTrigger>();
                EventTrigger rtrigger = rightButton.AddComponent<EventTrigger>();

                var leftDown = new EventTrigger.Entry();
                leftDown.eventID = EventTriggerType.PointerDown;
                leftDown.callback.AddListener(OnLeftPointerDown);

                var rightDown = new EventTrigger.Entry();
                rightDown.eventID = EventTriggerType.PointerDown;
                rightDown.callback.AddListener(OnRightPointerDown);

                var pointerUp = new EventTrigger.Entry();
                pointerUp.eventID = EventTriggerType.PointerUp;
                pointerUp.callback.AddListener(StopValueChangeCourotine);

                var pointerExit = new EventTrigger.Entry();
                pointerExit.eventID = EventTriggerType.PointerExit;
                pointerExit.callback.AddListener(StopValueChangeCourotine);

                ltrigger.triggers.Add(leftDown);
                ltrigger.triggers.Add(pointerUp);
                ltrigger.triggers.Add(pointerExit);

                rtrigger.triggers.Add(rightDown);
                rtrigger.triggers.Add(pointerUp);
                rtrigger.triggers.Add(pointerExit);

            }
        }

        private void OnEnable()
        {
            headerText.text = name !=null && name != ""? LocalizationManager.Instance[name]: "Property";
            if (lowerBind != null)
                Min = lowerBind.Value;
            if (upperBind != null)
                Max = upperBind.Value;
            UpdateValue();
        }

        private void OnLeftButtonClik()
        {
            float val = _value - step;
            UpdateBindValues(val);
            Value = val;
        }

        private void OnRightButtonClik()
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

        public void OnLeftPointerDown(BaseEventData args)
        {
            StartCoroutine(ChangeValueCourotine(CalculateDelay(), OnLeftButtonClik));
        }

        public void OnRightPointerDown(BaseEventData args)
        {
            StartCoroutine(ChangeValueCourotine(CalculateDelay(), OnRightButtonClik));
        }

        private void StopValueChangeCourotine(BaseEventData args)
        {
            StopAllCoroutines();
        }

        private IEnumerator ChangeValueCourotine(float seconds, System.Action action)
        {
            action();
            yield return new WaitForSeconds(VALUE_CHANGE_DELAY);
            while (true)
            {
                action();
                yield return new WaitForSeconds(seconds);
            }
        }

        private float CalculateDelay()
        {
            float delay = VALUE_CHANGE_DURATION / ((max - min) / step);
            Debug.LogFormat("Delay: {0}", delay);
            return delay;
        }
    }
}

    