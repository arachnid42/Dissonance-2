using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Localization;

namespace Assets.Scripts.UI.Elements
{
    public abstract class Spoiler<T> : MonoBehaviour
    {
        [SerializeField]
        private string name = null;
        [SerializeField]
        private bool isExpanded;

        [SerializeField]
        private Text headerText = null;
        [SerializeField]
        private Sprite spoilerClosedImage, spoilerOpenedImage;
        [SerializeField]
        private RectTransform content;
        [SerializeField]
        private Image buttonImage;
        [SerializeField]
        private Color closedColor, openedColor;
        [SerializeField]
        private float time = 0.25f;
        [SerializeField]
        private AnimationCurve curve;

        private RectTransform rt;
        private float bottom;
        private Coroutine toggleCoroutine;
        private float stage;

        public abstract T GetData();
        public abstract void SetData(T data);


        private void Start()
        {
            float tan45 = Mathf.Tan(Mathf.Deg2Rad * 45);
            curve = new AnimationCurve();
            curve.AddKey(new Keyframe(0, 0, tan45, 0));
            curve.AddKey(new Keyframe(1, 1, 0, tan45));
        }

        private void OnEnable()
        {
            rt = GetComponent<RectTransform>();
            headerText.text = name != ""? LocalizationManager.Instance[name]: "Property";
            bottom = rt.sizeDelta.y;
            ToggleSpoiler();
        }

        private void OnDisable()
        {
            ResetSpoiler();
        }

        public void OnToggleButtonClik()
        {
            isExpanded = !isExpanded;
            ToggleSpoiler();

        }

        private void ToggleSpoiler()
        {
            var start = rt.sizeDelta;
            var end = new Vector2(rt.sizeDelta.x, bottom + content.sizeDelta.y);
            if (isExpanded)
            {
                buttonImage.sprite = spoilerOpenedImage;
                buttonImage.color = openedColor;
            }
            else
            {
                buttonImage.sprite = spoilerClosedImage;
                buttonImage.color = closedColor;
                end = new Vector2(rt.sizeDelta.x, bottom);
            }
            if (toggleCoroutine != null)
                StopCoroutine(toggleCoroutine);
            toggleCoroutine = StartCoroutine(TogleCoroutine(start,end));
        }

        private IEnumerator TogleCoroutine(Vector2 start, Vector2 end)
        {
            while (stage <= 1)
            {
                stage += Time.unscaledDeltaTime / time;
                rt.sizeDelta = Vector2.Lerp(start, end, curve.Evaluate(stage));
                yield return null;
            }
            stage = 0;
        }
        
        private void ResetSpoiler()
        {
            isExpanded = false;
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, bottom);
        }
    }
}

