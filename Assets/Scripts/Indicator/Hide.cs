using System;
using System.Collections;

using UnityEngine;

namespace Assets.Scripts.Indicator
{
    public class Hide : MonoBehaviour
    {
        [SerializeField]
        private float time = 1;
        [SerializeField]
        private bool hidden = false;
        [SerializeField]
        private AnimationCurve curve = null;
        private Coroutine hideTargetsCoroutine = null;
        [Serializable]
        public class Target
        {
            public GameObject target = null;
            public Vector3 visibleScale = Vector3.one, hiddenScale = Vector3.zero;
        }
        [SerializeField]
        private Target[] targets = null;

        public bool Hidden
        {
            get
            {
                return hidden;
            }
            set
            {
                if (hidden == value)
                    return;
                if (hideTargetsCoroutine != null)
                    StopCoroutine(hideTargetsCoroutine);
                hidden = value;
                hideTargetsCoroutine = StartCoroutine(HideTargetsCoroutine(hidden));
            }
        }

        public bool Started
        {
            get
            {
                return hideTargetsCoroutine!=null;
            }
        }


        private void Start()
        {
            hideTargetsCoroutine = StartCoroutine(HideTargetsCoroutine(hidden, 1));
        }


        private IEnumerator HideTargetsCoroutine(bool hidden, float stage = 0)
        {
            while (stage <= 1)
            {
                stage += Time.unscaledDeltaTime / time;
                foreach(var target in targets)
                {
                    if (hidden)
                        target.target.transform.localScale = Vector3.Lerp(target.visibleScale, target.hiddenScale, curve.Evaluate(stage));
                    else
                        target.target.transform.localScale = Vector3.Lerp(target.hiddenScale, target.visibleScale, curve.Evaluate(stage));
                }
                yield return null;
            }
            hideTargetsCoroutine = null;
        }

    }
}
