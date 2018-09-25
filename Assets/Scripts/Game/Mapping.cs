using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Indicator;
using Assets.Scripts.ShapeBasket;

namespace Assets.Scripts.Game
{
    [System.Serializable]
    public class Mapping : MonoBehaviour
    {
 
        [System.Serializable]
        public class Bonus
        {
            public GameObject freeze;
            public GameObject explosion;
            public GameObject heart;
        }
        [System.Serializable]
        public class ParticleEffect
        {
            public GameObject freeze;
            public GameObject explosion;
        }

        [System.Serializable]
        public class Field
        {
            public GameObject shapeBasket;
            public GameObject scoreCounter;
        }

        public Field field = new Field();
        public ParticleEffect particleEffect = new ParticleEffect();
        public Bonus bonus = new Bonus();
        public ShapesPreset shapes = null;

        public SpawnPresets spawnPresets;

        [SerializeField]
        private Indicator.Controller indicatorController = null;
        [SerializeField]
        private ShapeBasket.Controller shapeBasketController = null;


        public int Score
        {
            get
            {
                return indicatorController.ScoreCounter.Value;
            }
            set
            {
                indicatorController.ScoreCounter.Value = value;
            }
        }


        public bool ScoreHidden
        {
            get { return indicatorController.ScoreHidden; }

            set { indicatorController.ScoreHidden = value; }
        }

        public bool TimerHidden
        {
            get { return indicatorController.TimerHidden; }

            set { indicatorController.TimerHidden = value; }
        }

        public float Timer
        {
            get
            {
                return indicatorController.Timer.Seconds;
            }
            set
            {
                indicatorController.Timer.Seconds = value;
            }
        }

        public GameMode Mode
        {
            get
            {
                return indicatorController.Mode.mode;
            }
            set
            {
                indicatorController.Mode.SetGameMode(value);
                shapeBasketController.ShapeCatcher.mode = value;
            }
        }

        public GameMode SwapMode()
        {
            Debug.Log("SWAP MODES MAPPING");
            GameMode mode = indicatorController.Mode.SwapGameModes();
            shapeBasketController.ShapeCatcher.mode = mode;
            return mode;
        }

        public bool ShapeBasketHidden
        {
            get
            {
                return shapeBasketController.Hide.Hidden;
            }

            set
            {
                shapeBasketController.Hide.Hidden = value;
            }
        }

        public bool IndicatorHidden
        {
            get
            {
                return indicatorController.Hide.Hidden;
            }
            set
            {
                indicatorController.Hide.Hidden = value;
            }
        }

        public TileSwitcher TileSwitcher
        {
            get
            {
                return shapeBasketController.TileSwitcher;
            }
        }

        public ShapeMixer ShapeMixer
        {
            get
            {
                return shapeBasketController.ShapeMixer;
            }
        }

        public BasketBody BasketBody
        {
            get { return shapeBasketController.BasketBody; }
        }

        public Indicator.Bonuses Bonuses
        {
            get
            {
                return indicatorController.Bonuses;
            }
        }

    }
}
