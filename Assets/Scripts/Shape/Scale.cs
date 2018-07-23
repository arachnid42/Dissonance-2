using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Shape
{

    public class Scale : MonoBehaviour
    {
        private Coroutine scaleCoroutine = null;
        private Destruction destruction = null;

        public void SetScale(Vector3 scale)
        {
            transform.localScale = scale;
        }

        public void SetSmoothScale(Vector3 scale, float time)
        {
            if (scaleCoroutine != null)
                StopCoroutine(scaleCoroutine);
            scaleCoroutine = StartCoroutine(ScaleCoroutine(scale, time));
        }

        private void Awake()
        {
            destruction = GetComponent<Destruction>();
        }

        private IEnumerator ScaleCoroutine(Vector3 end, float time = 1)
        {
            Vector3 start = transform.localScale;
            for(float stage = 0; stage <=1 &&!destruction.Started; stage += Time.deltaTime / time)
            {
                transform.localScale = Vector3.Slerp(start, end, stage);
                yield return null;
            }
            scaleCoroutine = null;
        }

    }
}
