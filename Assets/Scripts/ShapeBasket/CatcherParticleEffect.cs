using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Shape;

namespace Assets.Scripts.ShapeBasket
{
    public class CatcherParticleEffect : MonoBehaviour
    {

        public GameObject diamond;
        public GameObject square;
        public GameObject triangle;
        public GameObject circle;
        public GameObject pentagon;
        public GameObject pentagram;
        public GameObject hexagon;
        public GameObject hexagram;
        public GameObject octagram;
        public GameObject decagram;

        public GameObject heart;
        public GameObject snowflake;
        public GameObject explosion;

        public ShapeType tileShapeType;
        public ShapeType shapeShapeType;
        public Color tileColor;
        public Color shapeColor;


        public GameObject CreateParticles(GameObject particles, Color color)
        {
            particles = Instantiate(particles, transform);
            var main = particles.GetComponent<ParticleSystem>().main;
            main.startColor = color;
            return particles;
        }

        public void Start()
        {
            GameObject shapeParticles = null;
            GameObject tileParticles = null;
            Dictionary<ShapeType, GameObject> shapeTypeToParticle = new Dictionary<ShapeType, GameObject>
            {
                { ShapeType.Circle, circle },
                { ShapeType.Diamond, diamond },
                { ShapeType.Square, square },
                { ShapeType.Triangle, triangle },

                { ShapeType.Pentagon, pentagon},
                { ShapeType.Pentagram, pentagram },
                { ShapeType.Hexagon, hexagon },
                { ShapeType.Hexagram, hexagram },
                { ShapeType.Octogram, octagram },
                { ShapeType.Decagram, decagram },

                { ShapeType.Heart, heart },
                { ShapeType.Snowflake, snowflake },
                { ShapeType.Explosion, explosion }
            };
            shapeParticles = CreateParticles(shapeTypeToParticle[shapeShapeType], shapeColor);
            tileParticles = CreateParticles(shapeTypeToParticle[tileShapeType], tileColor);
            shapeParticles.SetActive(true);
            tileParticles.SetActive(true);
        }

    }
}

