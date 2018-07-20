using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Game;

namespace Assets.Scripts.Shape
{
    public class Bonus : MonoBehaviour
    {
        public System.Action<ShapeType> OnTouch = null;
        private bool touchDetected = false;
        public void DetectTouch()
        {
            if (touchDetected)
                return;
            touchDetected = true;
            var controller = GetComponent<Controller>();
            OnTouch(controller.RandomRotation.CurrentRotation.type);
            controller.Destruction.StartDestructionWithParticleEffect();
        }

        public void Start()
        {
            var mr = GetComponentInChildren<MeshRenderer>();
            Material color = mr.sharedMaterial;
            var rr = GetComponent<RandomRotation>();

            switch (rr.CurrentRotation.type)
            {
                case ShapeType.Explosion:
                    color = ColorsPresetsManager.Instance.Materials.explosionBonus;
                    break;
                case ShapeType.Heart:
                    color = ColorsPresetsManager.Instance.Materials.heartBonus;
                    break;
                case ShapeType.Snowflake:
                    color = ColorsPresetsManager.Instance.Materials.freezeBonus;
                    break;
            }

            mr.sharedMaterial = color;
        }

    }
}
