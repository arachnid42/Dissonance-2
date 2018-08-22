using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Shape;
using Assets.Scripts.Indicator;
using Assets.Scripts.ShapeBasket;

namespace Assets.Scripts.Game
{
    [System.Serializable]
    public class State : MonoBehaviour
    {
        public class GameOver
        {
            public bool win = false;
            public Difficulty.Target target;
            public int score = 0;
            public int time = 0;
            public GameMode mode = GameMode.None;
        }

        [System.Serializable]
        public class Bonus
        {

            public int lastScore = 0;
            public int lastUsedScore = 0;
            public int number = 0;
            public int current = 0;
            public int onScreen = 0;
            public void Reset()
            {
                current = 0;
                number = 0;
                lastScore = 0;
                onScreen = 0;
                lastUsedScore = 0;
            }

        }
        [System.Serializable]
        public class Mode
        {
            public float lastChangeTime = 0;
            public int lastChangeScore = 0;
            public void Reset()
            {
                lastChangeTime = 0;
                lastChangeScore = 0;
            }
        }
        [System.Serializable]
        public class Slowdown
        {
            public float startedTime = 0f;
            public float timeInterval = 1f;
            public float speedScale = 1f;
            public void Reset()
            {
                timeInterval = 1;
                startedTime = 0;
                speedScale = 1;
            }
        }
        [System.Serializable]
        public class RandomRotation
        {
            public float lastTime = 0;
            public void Reset()
            {
                lastTime = Time.time;
            }
        }
        [System.Serializable]
        public class ModeChange
        {
            public float lastTime = 0;
            public int lastScore = 0;
            public void Reset()
            {
                lastTime = 0;
                lastScore = 0;
            }
        }


        public float startTime = 0f;
        public float firstScoreTime = -1;
        public Slowdown slowdown = new Slowdown();

        public RandomRotation randomRotation = new RandomRotation();

        public ModeChange modeChange = new ModeChange();

        public Mode basketMode = new Mode();
        public Mode basketColors = new Mode();
        public Mode basketShapes = new Mode();

        public Bonus freeze = new Bonus();
        public Bonus heart = new Bonus();
        public Bonus explosion = new Bonus();

        public GameOver gameOver = null;

        public int shapesOnScreenLimit = 1;
        public int score = 0;
        public bool started = false;
        public bool paused = false;
        public GameMode mode = GameMode.Color;
        public GameObject lastSpawnedShapePrototype = null;

        public long coroutinesStarted = 0;
        public Difficulty.Slowdown.Type slowdownType = Difficulty.Slowdown.Type.None;

        public List<GameObject> shapesOnScreen;
        public Dictionary<GameObject, List<GameObject>> shapesToSpawns;

        [SerializeField]
        private float playerReactionTime = 5f;

        [SerializeField]
        private Mapping mapping = null;
        


        public int Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
                mapping.Score = value;
            }
        }

        public Mapping Mapping
        {
            get { return mapping; }
        }

        public float Timer
        {
            get
            {
                return mapping.Timer;
            }
            set
            {
                mapping.Timer = value;
            }

        }

        public GameMode BasketGameMode
        {
            get
            {
                return mode;
            }
            set
            {
                mapping.Mode = value;
                mode = value;
            }
        }


        public bool Started
        {
            get
            {
                return started;
            }
            set
            {
                started = value;
                //paused = false;
                mapping.ShapeBasketHidden = !value;
                mapping.IndicatorHidden = !value;
            }
        }

        public int HeartBonuses
        {
            get
            {
                return mapping.Bonuses.Hearts;
            }
            set
            {
                heart.current = value;
                mapping.Bonuses.Hearts = value;
            }
        }

        public int FreezeBonuses
        {
            get
            {
                return mapping.Bonuses.Freezes;
            }
            set
            {
                freeze.current = value;
                mapping.Bonuses.Freezes = value;
            }
        }

        public int ExplosionBonuses
        {
            get
            {
                return mapping.Bonuses.Explosions;
            }
            set
            {
                explosion.current = value;
                mapping.Bonuses.Explosions = value;
            }
        }

        public float PlayerReactionTime
        {
            get
            {
                return playerReactionTime;
            }
            set
            {
                var difficulty = DifficultyLevels.Instance.CurrentDifficulty;
                playerReactionTime = Mathf.Clamp(value, difficulty.playerReactionTime.min, difficulty.playerReactionTime.max);
            }
        }

        public float ScaledPlayerReactionTime
        {
            get { return slowdown.speedScale == 0 ? float.PositiveInfinity : PlayerReactionTime / slowdown.speedScale; }
        }

        public Difficulty Difficulty
        {
            get { return DifficultyLevels.Instance.CurrentDifficulty; }
        }

        public SpawnsPreset ActiveSpawnPreset
        {
            get
            {
                return mapping.spawnPresets[shapesOnScreenLimit - 1];
            }
        }


        public void SetGameOverData(bool win)
        {
            float time = firstScoreTime < 0 ? 0 : Time.time - firstScoreTime;
            if (Difficulty.target.endless)
            {
                time = Mathf.Max(time,0);
            }
            else
            {
                time = Mathf.Clamp(time, 0, Difficulty.target.time);
            }
            gameOver = new GameOver()
            {
                win = win,
                target = Difficulty.target.Copy(),
                score = score,
                time = Mathf.FloorToInt(time),
                mode = mode
            };
        }


        public void Reset()
        {

            coroutinesStarted = 0;
            slowdownType = Difficulty.Slowdown.Type.None;
            lastSpawnedShapePrototype = null;
            paused = false;
            Started = false;
            Score = 0;
            startTime = Time.time;
            firstScoreTime = -1;
            mapping.Timer = Difficulty.target.endless?0:Difficulty.target.time;
            mapping.TimerHidden = Difficulty.target.scoreBased;
            mapping.ScoreHidden = !Difficulty.target.scoreBased;
            freeze.Reset();
            heart.Reset();
            explosion.Reset();
            basketColors.Reset();
            basketShapes.Reset();
            basketMode.Reset();
            modeChange.Reset();
            mapping.Bonuses.Hearts = heart.current;
            mapping.Bonuses.Explosions = explosion.current;
            mapping.Bonuses.Freezes = explosion.current;
            if(Difficulty.initialGameMode == GameMode.None)
            {
                if (Random.value < 0.5)
                    BasketGameMode = GameMode.Shape;
                else
                    BasketGameMode = GameMode.Color;
            }
            else
            {
                BasketGameMode = Difficulty.initialGameMode;
            }
            shapesOnScreenLimit = 1;
            playerReactionTime = Difficulty.playerReactionTime.max;
            shapesOnScreen = new List<GameObject>(Difficulty.shapesOnScreen.max);
            shapesToSpawns = ActiveSpawnPreset.CreateShapesToSpawnsMapping();
            slowdown.Reset();
            mapping.shapes.Filter(Difficulty.additionalShapes.ToArray());
            mapping.BasketBody.SetShapes(mapping.shapes.types);
        }

        public void RemoveShape(GameObject shape)
        {
            shapesOnScreen.Remove(shape);
            List<GameObject> list = null;
            foreach (GameObject spawn in ActiveSpawnPreset.spawns)
            {
                if (shapesToSpawns.TryGetValue(spawn, out list))
                {
                    list.Remove(shape);
                }
            }
        }

        public GameMode SwapBasketGameModes()
        {
            GameMode mode = mapping.SwapMode();
            this.mode = mode;
            return mode;
        }

    }
}
