using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Indicator
{
    public enum GameMode
    {
        Shape,
        Color,
        None
    }

    public class Mode : MonoBehaviour
    {

        public GameObject shape = null;
        public GameObject color = null;
        public float time = 1f;
        public GameMode mode = GameMode.None;
        public Hide hide = null;

        [SerializeField]
        private AnimationCurve scaleCurve = null;
        private Vector3 shapeHiddenPosition;
        private Vector3 colorHiddenPosition;


        private Coroutine modeSwitchCoroutine = null;

        public bool IsChangeStarted
        {
            get
            {
                return modeSwitchCoroutine!=null;
            }
        }

        public GameMode SwapGameModes()
        {
            //Debug.Log("SWAP MODES!");
            if (mode == GameMode.Shape)
            {
                SetGameMode(GameMode.Color);
                return GameMode.Color;
            }else if(mode == GameMode.Color)
            {
                SetGameMode(GameMode.Shape);
                return GameMode.Shape;
            }
            return GameMode.None;
        }

        public void Awake()
        {
            shapeHiddenPosition = shape.transform.localPosition;
            colorHiddenPosition = color.transform.localPosition;
        }

        public void SetGameMode(GameMode mode)
        {
            if(this.mode != mode)
            {
                if (modeSwitchCoroutine != null)
                    StopCoroutine(modeSwitchCoroutine);
                modeSwitchCoroutine = StartCoroutine(ModeSwitchCoroutine(mode));
            }
            
        }

        private IEnumerator ModeSwitchCoroutine(GameMode mode)
        {

            this.mode = mode;

            Vector3 shapeEndPosition, shapeStartPosition, colorStartPosition, colorEndPosition; 


            if (mode == GameMode.Shape)
            {
                shapeEndPosition = shapeHiddenPosition;
                shapeEndPosition.x = 0;
                colorEndPosition = colorHiddenPosition;
            }
            else if (mode == GameMode.Color)
            {
                colorEndPosition = colorHiddenPosition;
                colorEndPosition.x = 0;
                shapeEndPosition = shapeHiddenPosition;
            }
            else
            {
                colorEndPosition = colorHiddenPosition;
                shapeEndPosition = shapeHiddenPosition;
            }

            shapeStartPosition = shape.transform.localPosition;
            colorStartPosition = color.transform.localPosition;

            float stage = hide.Started || hide.Hidden?1:0;
            while(stage <= 1)
            {
                stage += Time.deltaTime / time;
                color.transform.localPosition = Vector3.Lerp(colorStartPosition, colorEndPosition, stage);
                shape.transform.localPosition = Vector3.Lerp(shapeStartPosition, shapeEndPosition, stage);
                shape.transform.localScale = scaleCurve.Evaluate(stage) * Vector3.one;
                color.transform.localScale = scaleCurve.Evaluate(stage) * Vector3.one;
                yield return null;
            }

            modeSwitchCoroutine = null;
        }

    }

}