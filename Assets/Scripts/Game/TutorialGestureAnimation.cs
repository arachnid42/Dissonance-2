using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Game
{
    public class TutorialGestureAnimation: MonoBehaviour
    {
        public enum SwipeAnimationType
        {
            TOP = 0,
            LEFT = 1,
            RIGHT = 2,
            BOTTOM = 4
        }
        public GameObject circle = null;
        [SerializeField]
        private float tapAnimationTime = 0.5f, swipeAnimationTime = 0.5f, swipeAnimationScaleTime = 0.5f;
        [SerializeField]
        private AnimationCurve tapAnimationScaleCurve = null, swipeAnimationCurve = null;
        private Coroutine currentAnimation = null;
        [SerializeField]
        private GameObject swipeTopAncor, swipeDownAncor, swipeLeftAncor, swipeRightAncor;
        [SerializeField]
        private float verticalSwipeEndPOffset = 0.35f, horizontalSwipePOffset = 0.35f;

        private void DrawAncorGizmos(GameObject ancor)
        {
            if (ancor == null)
                return;
        }

        private void DrawSwipeLineGizmos(GameObject a, GameObject b, float swipeEndPOffset)
        {
            if (a == null || b == null)
                return;
            Vector3 start = a.transform.position, end = b.transform.position;
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(start, 5);
            Gizmos.DrawWireSphere(end, 5);
            Gizmos.DrawWireSphere(Vector3.Lerp(start, end, swipeEndPOffset), 5);
            Gizmos.DrawWireSphere(Vector3.Lerp(end, start, swipeEndPOffset), 5);
            Gizmos.DrawLine(start, end);
        }

        private void OnDrawGizmos()
        {
            DrawSwipeLineGizmos(swipeTopAncor, swipeDownAncor, verticalSwipeEndPOffset);
            DrawSwipeLineGizmos(swipeLeftAncor, swipeRightAncor, horizontalSwipePOffset);
        }


        public void PlayTapAnimation()
        {
            StopAnimation();
            currentAnimation = StartCoroutine(TapAnimation());
            circle.SetActive(true);
        }

        public void PlaySwipeAnimation(SwipeAnimationType type)
        {
            StopAnimation();
            currentAnimation = StartCoroutine(SwipeAnimation(type));
            circle.SetActive(true);
        }

        public void StopAnimation()
        {
            if (currentAnimation != null)
                StopCoroutine(currentAnimation);
            circle.SetActive(false);
        }

        private IEnumerator ScaleAnimation(GameObject target, Vector3 end, float time, AnimationCurve curve)
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

        private IEnumerator MoveAnimation(GameObject target, Vector3 start, Vector3 end, float time, AnimationCurve curve)
        {
            float stage = 0;
            do
            {
                stage += Time.unscaledDeltaTime / time;
                target.transform.position = Vector3.Lerp(start, end, curve.Evaluate(stage));
                yield return null;
            } while (stage < 1);
        }


        private IEnumerator SwipeAnimation(Vector3 start, Vector3 end, bool repeatable = true)
        {
            do
            {
                circle.transform.position = start;
                circle.transform.localScale = Vector3.zero;
                yield return ScaleAnimation(circle, Vector3.one, swipeAnimationScaleTime / 2, tapAnimationScaleCurve);
                yield return MoveAnimation(circle, start, end, swipeAnimationTime, swipeAnimationCurve);
                yield return ScaleAnimation(circle, Vector3.zero, swipeAnimationScaleTime / 2, tapAnimationScaleCurve);
            } while (repeatable);
        }


        public IEnumerator SwipeAnimation(SwipeAnimationType type, bool repeatable = true)
        {
            Vector3
                start = Vector3.zero,
                end = Vector3.zero,
                top = swipeTopAncor.transform.position,
                bottom = swipeDownAncor.transform.position,
                left = swipeLeftAncor.transform.position,
                right = swipeRightAncor.transform.position;

            switch (type)
            {
                case SwipeAnimationType.BOTTOM:
                    start = top;
                    end = Vector3.Lerp(top, bottom, verticalSwipeEndPOffset);
                    break;
                case SwipeAnimationType.TOP:
                    start = bottom;
                    end = Vector3.Lerp(bottom, top, verticalSwipeEndPOffset);
                    break;
                case SwipeAnimationType.LEFT:
                    start = right;
                    end = Vector3.Lerp(right, left, horizontalSwipePOffset);
                    break;
                case SwipeAnimationType.RIGHT:
                    start = left;
                    end = Vector3.Lerp(left, right, horizontalSwipePOffset);
                    break;
            }
            yield return SwipeAnimation(start, end, repeatable);
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
        //    PlaySwipeAnimation(SwipeAnimationType.BOTTOM);
        //}

    }
}
