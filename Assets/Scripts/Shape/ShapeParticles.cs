using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Shape
{
    [System.Serializable]
    public struct ShapeTypeToParticles
    {
        public ShapeType type;
        public GameObject particles;
    }

    class ShapeParticles : MonoBehaviour
    {


        public List<ShapeTypeToParticles> particlesMapping;

        private GameObject currenParticles;
        private ShapeType currentShape = ShapeType.None;
        public Color currentColor = Color.black;
        

        public ShapeType ParticlesShape
        {
            get
            {
                return currentShape;
            }
            set
            {
                GameObject newParticles = null;
                if(currentShape == value)
                {
                    return;
                }
                newParticles = particlesMapping.Find(e => e.type == value).particles;
                currentShape = value;
                if (currenParticles != null)
                {
                    currenParticles.SetActive(false);
                }
                currenParticles = newParticles;
                newParticles.SetActive(true);
            }
        }

        public Color ParticlesColor
        {
            get
            {
                return currentColor;
            }

            set
            {
                if(currentColor == value)
                {
                    return;
                }
                var main = currenParticles.GetComponent<ParticleSystem>().main;
                currentColor = value;
                main.startColor = value;
            }

        }


        public void SetShapeAndColor(ShapeType shape, Color color)
        {
            ParticlesShape = shape;
            ParticlesColor = color;
        }


    }
}
