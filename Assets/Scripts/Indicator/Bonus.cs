using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Shape;
using Assets.Scripts.Game;

namespace Assets.Scripts.Indicator
{
    class Bonus : MonoBehaviour
    {
        [SerializeField]
        private ShapeType type;
        [SerializeField]
        private bool isEnabled = true;
        [SerializeField]
        private Vector3 enabledScale = new Vector3(0.8f, 0.8f, 1);
        [SerializeField]
        private Vector3 disabledScale = new Vector3(0, 0, 1);
        [SerializeField]
        private float hideTime = 0.25f, moveTime = 0.25f;
        [SerializeField]
        private AnimationCurve curve = null;

        private int number = 0;
        [SerializeField]
        private TextMesh counter;
        [SerializeField]
        private MeshRenderer bonusMr;

        private Coroutine hideCoroutine = null, moveCoroutine = null;


        public int Number
        {
            get { return number; }
            set
            {
                number = value;
                counter.text = value.ToString();
            }
        }

        public void MoveTo(Vector3 position)
        {
            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);
            moveCoroutine = StartCoroutine(MoveCoroutine(position));
        }

        public bool Enabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                if (isEnabled == value)
                    return;
                isEnabled = value;
                if (hideCoroutine != null)
                    StopCoroutine(hideCoroutine);
                hideCoroutine = StartCoroutine(HideCoroutine(isEnabled?enabledScale:disabledScale));
            }
        }

        public IEnumerator Start()
        {

            while (!ColorsPresetsManager.Ready)
                yield return null;
            if (isEnabled)
                transform.localScale = enabledScale;
            else
                transform.localScale = Vector3.forward;
            var mat = bonusMr.sharedMaterial;
            switch (type)
            {
                case ShapeType.Explosion:
                    mat = ColorsPresetsManager.Instance.Materials.explosionBonus;
                    break;
                case ShapeType.Heart:
                    mat = ColorsPresetsManager.Instance.Materials.heartBonus;
                    break;
                case ShapeType.Snowflake:
                    mat = ColorsPresetsManager.Instance.Materials.freezeBonus;
                    break;
            }
            bonusMr.sharedMaterial = mat;
        }


        private IEnumerator MoveCoroutine(Vector3 end)
        {
            float stage = 0;
            Vector3 start = transform.localPosition;
            while (stage <= 1)
            {
                stage += Time.unscaledDeltaTime / moveTime;
                transform.localPosition = Vector3.Lerp(start, end, stage);
                yield return null;
            }
            moveCoroutine = null;
        }

        private IEnumerator HideCoroutine(Vector3 end)
        {
            float stage = 0;
            Vector3 start = transform.localScale;
            while (stage <= 1)
            {
                stage += Time.unscaledDeltaTime / hideTime;
                transform.localScale = Vector3.Lerp(start, end, curve.Evaluate(stage));
                yield return null;
            }
            hideCoroutine = null;
        }

    }
}
