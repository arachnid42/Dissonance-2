using UnityEngine;


namespace Assets.Scripts.Game
{
    class PlayerInput : MonoBehaviour
    {

        private Vector3 touchStartPosition;
        private float touchStartedTime;
        [SerializeField]
        private Vector2 swipeResistance = new Vector2(0.3f, 0.3f);
        [SerializeField]
        private float swipeTime = 0.3f;
        [SerializeField]
        private Master master = null;
        [SerializeField]
        private float raycastDistance = 100f;
        [SerializeField]
        private LayerMask bonusLayer;
        [SerializeField]
        private LayerMask uiLayer;
        [SerializeField]
        private string bonusTag = "Bonus";
        [SerializeField]
        private string tileTag = "Tile";
        [SerializeField]
        private bool debug = false;


        private bool IsTouched
        {
            get
            {
                return Input.touchCount > 0 || Input.GetMouseButton(0) || Input.GetMouseButtonUp(0) || Input.GetMouseButtonDown(0);
            }
        }

        private Touch CurrentTouch
        {
            get
            {
                if(Input.touchCount > 0)
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

        private Vector2 TouchPosition
        {
            get
            {
                return Input.touchCount > 0 ? (Vector3) CurrentTouch.position: Input.mousePosition;
            }
        }    


        private bool DetectInteractionWithObjects()
        {
            var touch = CurrentTouch;
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
            {


                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;
                Physics.Raycast(ray, out hit, raycastDistance, bonusLayer | uiLayer);


                if (touch.phase == TouchPhase.Began && hit.collider != null && hit.collider.tag == bonusTag)
                {
                    if (master.State.tutorial != null && !master.State.tutorial.controls.bonusPick)
                        return true;
                    var controller = hit.collider.attachedRigidbody.GetComponent<Shape.Controller>();
                    controller.Bonus.DetectTouch();
                    return true;
                }
                else if (hit.collider != null && hit.collider.tag == tileTag)
                {
                    if (master.State.tutorial !=null && !master.State.tutorial.controls.backet)
                        return true;
                    master.State.Mapping.TileSwitcher.Tile = hit.collider.gameObject;
                    return true;
                }


            }
            return false;
        }


        private void OnSwipeDown()
        {
            if (master.State.tutorial != null && !master.State.tutorial.controls.pause)
                return;
            master.Actions.Pause();
        }

        private void OnSwipeUp()
        {
            if (master.State.tutorial != null && !master.State.tutorial.controls.pause)
                return;
            master.Actions.Pause();
        }

        private void OnSwipeLeft()
        {
            if (master.State.tutorial != null && !master.State.tutorial.controls.bonusUse)
                return;
            master.Actions.UseExplosionBonus();
        }

        private void OnSwipeRight()
        {
            if (master.State.tutorial != null && !master.State.tutorial.controls.bonusUse)
                return;
            master.Actions.UseFreezeBonus();
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
            else if(touch.phase == TouchPhase.Ended)
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
                else if(absDelta.x < absDelta.y && absDelta.y > swipeResistance.y)
                {
                    if(delta.y < 0)
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


        private bool DetectRestart()
        {
            if (!debug)
            {
                return false;
            }
            if (master.State.Started)
                return false;
            if (Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0)
            {
                master.Restart();
                return true;
            }
            return false;
        }

        public void Update()
        {
            if (DetectRestart())
            {
                return;
            }

            if (master.State.Started && !master.State.paused)
            {
                if (!IsTouched)
                {
                    //Debug.Log("Not touched");
                    return;
                }
                if (DetectInteractionWithObjects())
                {
                    //Debug.Log("Interaction detected");
                    return;
                }
                else if (DetectSwipe())
                {
                    // Debug.Log("Swipe detected");
                    return;
                }
            }
                
        }
    }
}
