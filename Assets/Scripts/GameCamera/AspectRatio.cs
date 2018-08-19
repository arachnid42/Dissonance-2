using UnityEngine;

namespace Assets.Scripts.GameCamera
{
    public class AspectRatio: MonoBehaviour
    {
        public const float A_9_16 = 0.5625f;
        public const float A_9_18 = 0.5f;

        [SerializeField]
        private bool adjust = false;

        private void Awake()
        {
            if (!adjust)
                return;
            ResizeCameraRect();
        }

        public void ResizeCameraRect()
        {
            float defaultAspectRatio = A_9_16;
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
