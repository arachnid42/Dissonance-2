using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Game
{
    public class TutorialGestureAnimation: MonoBehaviour
    {
        [SerializeField]
        private GameObject circle = null;
        [SerializeField]
        private float tapAnimationTime = 0.5f;
        [SerializeField]
        private AnimationCurve tapAnimationScaleCurve = null;
        private Coroutine currentAnimation = null;

        public void PlayTapAnimation()
        {
            StopAnimation();
            currentAnimation = StartCoroutine(TapAnimation());
            circle.SetActive(true);
        }

        public void StopAnimation()
        {
            if (currentAnimation != null)
                StopCoroutine(currentAnimation);
            circle.SetActive(false);
        }

        private IEnumerator ScaleAnimation(GameObject target, Vector3 end, float time, AnimationCurve curve = null)
        {
            float stage = 0;

            Vector3 start = target.transform.localScale;
            do
            {
                stage += Time.unscaledDeltaTime / time;
                target.transform.localScale = Vector3.Lerp(start, end, curve.Evaluate(stage));
                yield return null;
            } while (stage < 1);
        }

        private IEnumerator TapAnimation()
        {

            while (true)
            {
                yield return ScaleAnimation(circle, Vector3.one * 0.1f, tapAnimationTime/2, tapAnimationScaleCurve);
                yield return ScaleAnimation(circle, Vector3.one, tapAnimationTime/2, tapAnimationScaleCurve);
            }
        }

        //private void Start()
        //{
        //    PlayTapAnimation();
        //}

    }
}
