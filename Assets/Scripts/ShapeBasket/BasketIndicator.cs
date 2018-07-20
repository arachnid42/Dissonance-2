using UnityEngine;
using System.Collections;
namespace Assets.Scripts.ShapeBasket
{
    public class BasketIndicator : MonoBehaviour
    {
        public Vector3 disabledScale;
        public Vector3 enabledScale;

        private Vector3 start;
        private Vector3 end;
        private float xStage = 0, yStage = 0;
        private Coroutine changeState;
        [SerializeField]
        private bool isEnabled;
        private float yStageTime = 0.1f;
        private float xStageTime = 0.15f;


        private IEnumerator ChangeState()
        {
            do
            {
                float xTime = Time.unscaledDeltaTime / xStageTime;
                float yTime = Time.unscaledDeltaTime / yStageTime;
                var newScale = transform.localScale;
                if (!isEnabled)
                {
                    if (xStage < 1)
                    {
                        xStage += xTime;

                    }
                    else if (yStage < 1)
                    {
                        yStage += yTime;
                    }
                }
                else
                {
                    if (yStage < 1)
                    {
                        yStage += yTime;
                    }
                    else if (xStage < 1)
                    {
                        xStage += xTime;
                    }
                }
                newScale.x = Mathf.Lerp(start.x, end.x, xStage);
                newScale.y = Mathf.Lerp(start.y, end.y, yStage);
                transform.localScale = newScale;
                yield return null;
            } while (xStage < 1 || yStage < 1);
        }

        public void SetEnabled(bool value, bool immediately = false)
        {
            if (value == isEnabled)
                return;
            if (changeState != null)
            {
                StopCoroutine(changeState);
            }

            if (immediately)
            {
                transform.localScale = value ? enabledScale : disabledScale;
                xStage = 0;
                yStage = 0;
            }
            else
            {
                xStage = xStage > 0 ? 1 - xStage : 0;
                yStage = yStage > 0 ? 1 - yStage : 0;
                start = transform.localScale;
                end = value ? enabledScale : disabledScale;
                isEnabled = value;
                changeState = StartCoroutine(ChangeState());
            }
               
            isEnabled = value;
        }

        private void Start()
        {
            transform.localScale = isEnabled ? enabledScale : disabledScale;
        }

        private void OnDestroy()
        {
            if (changeState != null)
                StopCoroutine(changeState);
        }
    }
}
