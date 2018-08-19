using UnityEngine;
using Assets.Scripts.Game;
namespace Assets.Scripts.GameCamera
{
    public class AspectRatio: MonoBehaviour
    {
        public const float A_9_16 = 0.5625f;
        public const float A_9_18 = 0.5f;
        public static float Distance(float a, float b)
        {
            return Mathf.Abs(a - b);
        }
        [SerializeField]
        private Field field = null;

        private void Awake()
        {
            ResizeCameraRect();
        }

        public void ResizeCameraRect()
        {
            float currentAspect = Camera.main.aspect;
            float defaultAspectRatio = A_9_16;

            if (Distance(currentAspect, A_9_16) > Distance(currentAspect, A_9_18))
            {
                defaultAspectRatio = A_9_18;
                field.Set918Active(true);
            }
            else
            {
                field.Set918Active(false);
            }

            float wTh = (float)Screen.width / (float)Screen.height;
            float hTw = 1 / wTh;
            float widthScale = hTw * defaultAspectRatio;
            float heightScale = wTh / defaultAspectRatio;

            var rect = Camera.main.rect;
            if (widthScale < 1)
                rect.x = (1 - widthScale) / 2;
            rect.width = widthScale;

            if (heightScale < 1)
                rect.y = (1 - heightScale) / 2;
            rect.height = heightScale;

            Camera.main.rect = rect;
        }
    }
}
