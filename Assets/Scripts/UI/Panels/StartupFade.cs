using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Panels
{
    public class StartupFade : BasePanel
    {
        [SerializeField]
        private CanvasGroup logoImageCanvas;
        [SerializeField]
        private float time = 0.25f;
        [SerializeField]
        private AnimationCurve curve;

        private float stage;

        private void OnEnable()
        {
            //StartCoroutine(ShowLogoCoroutine(0,1f));
        }

        private void OnDisable()
        {

        }

        private IEnumerator ShowLogoCoroutine(float start, float end)
        {
            while (stage <= 1)
            {
                stage += Time.unscaledDeltaTime / time;
                logoImageCanvas.alpha = Mathf.Lerp(start, end, curve.Evaluate(stage));
                //Debug.LogFormat("Stage: {0} Alpha: {1}",stage, logoImageCanvas.alpha);

                //rt.sizeDelta = Vector2.Lerp(start, end, curve.Evaluate(stage));
                yield return null;
            }
            stage = 0;
        }


    }
}

