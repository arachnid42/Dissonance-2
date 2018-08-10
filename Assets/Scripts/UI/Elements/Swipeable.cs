using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Elements
{
    public class Swipeable : MonoBehaviour
    {
        private Vector3 touchStartPosition;
        private float touchStartedTime;
        [SerializeField]
        private Vector2 swipeResistance = new Vector2(0.3f, 0.3f);
        [SerializeField]
        private float swipeTime = 0.3f;

        public System.Action OnSwipeLeft = () => { Debug.Log("OnSwipeLeft"); };
        public System.Action OnSwipeRight = () => { Debug.Log("OnSwipeRight"); };
        public System.Action OnSwipeDown = () => { Debug.Log("OnSwipeDown"); };
        public System.Action OnSwipeUp = () => { Debug.Log("OnSwipeUp"); };


        private void Update()
        {
            DetectSwipe();
        }

        private Touch CurrentTouch
        {
            get
            {
                if (Input.touchCount > 0)
                {
                    return Input.GetTouch(Input.touchCount - 1);
                }
                else
                {
                    Touch touch = new Touch()
                    {
                        position = Input.mousePosition
                    };
                    if (Input.GetMouseButtonUp(0))
                        touch.phase = TouchPhase.Ended;
                    else if (Input.GetMouseButtonDown(0))
                        touch.phase = TouchPhase.Began;
                    else if (Input.GetMouseButton(0))
                        touch.phase = TouchPhase.Moved;

                    return touch;
                }
            }
        }

        private bool DetectSwipe()
        {
            //Debug.Log("Trying to detect swipe");
            //Debug.Log(CurrentTouch.phase);
            var touch = CurrentTouch;
            if (touch.phase == TouchPhase.Began)
            {
                //Debug.Log("SWIPE BEGAN DETECTED");
                touchStartPosition = touch.position;
                touchStartedTime = Time.unscaledTime;
                return false;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (Time.unscaledTime - touchStartedTime > swipeTime)
                    return false;
                Vector3 delta = Camera.main.ScreenToViewportPoint(touch.position) - Camera.main.ScreenToViewportPoint(touchStartPosition);
                Vector2 absDelta = new Vector2(Mathf.Abs(delta.x), Mathf.Abs(delta.y));
                if (absDelta.x >= absDelta.y && absDelta.x > swipeResistance.x)
                {
                    if (delta.x < 0)
                    {
                        OnSwipeLeft();
                    }
                    else
                    {
                        OnSwipeRight();
                    }
                    return true;
                }
                else if (absDelta.x < absDelta.y && absDelta.y > swipeResistance.y)
                {
                    if (delta.y < 0)
                    {
                        OnSwipeDown();
                    }
                    else
                    {
                        OnSwipeUp();
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
