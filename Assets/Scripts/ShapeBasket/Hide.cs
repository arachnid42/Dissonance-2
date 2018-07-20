using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.ShapeBasket
{
    public class Hide : MonoBehaviour
    {
        [SerializeField]
        private float time = 0.25f;
        [SerializeField]
        private Vector3 hiddenPosition = Vector3.zero, visiblePosition = Vector3.zero;
        [SerializeField]
        private AnimationCurve curve = null;
        [SerializeField]
        private bool hidden = false;

        private Coroutine hideCoroutine;
        private float stage;

        private IEnumerator HideCoroutine(Vector3 start,Vector3 end)
        {

            //Debug.LogFormat("Starting coroutine stage:{0}", stage);
            while(stage <= 1)
            {
                stage += Time.unscaledDeltaTime/time;
                transform.localPosition = Vector3.Lerp(start, end, curve.Evaluate(stage));
                //Debug.LogFormat("Hidding stage:{0}", stage);
                yield return null;
            }
            stage = 0;
        }

        private void Awake()
        {
            transform.localPosition = hidden ? hiddenPosition : visiblePosition;
        }

        public bool Hidden
        {
            get { return hidden; }
            set
            {
                //Debug.Log("Trying to set hidden");
                if (value == hidden)
                    return;
                //Debug.Log("Setting hidden");
                if (hideCoroutine != null)
                    StopCoroutine(hideCoroutine);
                hidden = value;
                stage = stage > 0 ? 1 - stage : 0;
                var start = transform.localPosition;
                var end = hidden ? hiddenPosition : visiblePosition;
                hideCoroutine = StartCoroutine(HideCoroutine(start,end));
            }
        }

    }
}
