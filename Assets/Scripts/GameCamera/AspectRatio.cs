using UnityEngine;

namespace Assets.Scripts.GameCamera
{
    public class AspectRatio: MonoBehaviour
    {
        [SerializeField]
        private float defaultAspectRatio = 0.5625f;

        private void Awake()
        {
            ResizeCameraRect();
        }

        public void ResizeCameraRect()
        {
            float wTh = (float)Screen.width / (float)Screen.height;
            float hTw = 1 / wTh;
            float widthScale = hTw * defaultAspectRatio;
            float heightScale = wTh / defaultAspectRatio;

            var rect = Camera.main.rect;
            if (widthScale < 1)
                rect.x = (1 - widthScale) / 2;
            if (heightScale < 1)
                rect.y = (1 - heightScale) / 2;
            rect.width = widthScale;
            rect.height = heightScale;
            Camera.main.rect = rect;
        }
    }
}
