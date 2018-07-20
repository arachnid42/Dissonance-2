using UnityEngine;
using Assets.Scripts.Shape;
using System.Collections;

namespace Assets.Scripts.ShapeBasket
{
    public class ShapeToTile : MonoBehaviour
    {

        public ShapeType shapeType;
        public GameObject tile = null;
        
        [SerializeField]
        private float time = 1f;
        [SerializeField]
        private AnimationCurve scaleCurve = null;
        private Quaternion originalRotation = Quaternion.identity;
        private Coroutine tileSwitchCoroutine = null;

        public GameObject Tile
        {
            get
            {
                return tile;
            }
            set
            {
                SetTile(value);
            }
        }


        public void SetTile(GameObject value, bool immediate = false)
        {
            if (tile == value)
            {
                return;
            }
            tile = value;
            if (tileSwitchCoroutine != null)
                StopCoroutine(tileSwitchCoroutine);
            tileSwitchCoroutine = StartCoroutine(SwitchTileCoroutine(tile, immediate));
        }

        private IEnumerator SwitchTileCoroutine(GameObject tile,bool immediate = false)
        {
            float startingStage = immediate ? 1 : 0;
            float positionStage = startingStage;
            float fromStartToMiddleRotationStage = startingStage;
            float fromMiddleToEndRotationStage = startingStage;

            Vector3 startPosition = transform.localPosition;
            Vector3 endPosition = tile.transform.localPosition;
            endPosition.z = startPosition.z;

            float rotationSide = -Mathf.Sign(endPosition.x - startPosition.x);
            Quaternion startRotation = transform.localRotation;
            Vector3 rotAxis = transform.rotation * Vector3.forward;
            Quaternion middleRotation = startRotation * Quaternion.AngleAxis(180 * rotationSide, rotAxis);
            Quaternion endRotation = middleRotation * Quaternion.AngleAxis(180 * rotationSide, rotAxis);
            float stageIncrease = 0;
            Vector3 startingScale = transform.localScale;
            while (positionStage <=1 || fromMiddleToEndRotationStage <=1 || fromStartToMiddleRotationStage <=1)
            {
                stageIncrease = (Time.unscaledDeltaTime / time);
                if (fromStartToMiddleRotationStage <= 1)
                {
                    fromStartToMiddleRotationStage += stageIncrease * 2;
                    transform.localRotation = Quaternion.Lerp(startRotation, middleRotation, fromStartToMiddleRotationStage);
                }
                else if (fromMiddleToEndRotationStage <= 1)
                {
                    fromMiddleToEndRotationStage += stageIncrease * 2;
                    transform.localRotation = Quaternion.Lerp(middleRotation, endRotation, fromMiddleToEndRotationStage);
                }
                transform.localPosition = Vector3.Lerp(startPosition, endPosition, positionStage);
                positionStage += stageIncrease;
                transform.localScale = startingScale * scaleCurve.Evaluate(positionStage);
                yield return null;
            }
            transform.localRotation = originalRotation;
        }

        private void Awake()
        {
            originalRotation = transform.localRotation;
        }

    }

}
