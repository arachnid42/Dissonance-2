using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Indicator;
using Assets.Scripts.Shape;

namespace Assets.Scripts.Game
{
    public partial class Difficulty
    {

        [System.Serializable]
        public class Bonus
        {
            [HideInInspector]
            public int max = 4;
            [System.NonSerialized]
            public Slowdown slowdown = null;

            [Range(0, 100)]
            public int number = 1;
            [Range(1, 100)]
            public int scoreInterval = 10;
            [Range(0f, 1f)]
            public float probability = 0.5f;
            [Range(0, 100)]
            public int startScore = 10;

            public Bonus Copy()
            {
                return new Bonus()
                {
                    max = max,
                    number = number,
                    scoreInterval = scoreInterval,
                    probability = probability,
                    startScore = startScore,
                    slowdown = slowdown!=null?slowdown.Copy():null
                };
            }

            public Bonus Update(Bonus bonus)
            {
                number = bonus.number;
                scoreInterval = bonus.scoreInterval;
                startScore = bonus.startScore;
                probability = bonus.probability;
                return this;
            }
        }

        public class HeartBonus : Bonus
        {
            public HeartBonus()
            {
                max = HEART_BONUS_MAX;
                slowdown = new Slowdown(Slowdown.Type.Bonus)
                {
                    inTime = 0.1f,
                    stayTime = 2f,
                    outTime = 2f,
                    speedScale = 0.1f
                };
            }
        }

        public class ExplosionBonus: Bonus
        {
            public ExplosionBonus()
            {
                max = EXPLOSION_BONUS_MAX;
                slowdown = null;
            }
        }


        public class FreezeBonus : Bonus
        {
            public FreezeBonus()
            {
                max = FREEZE_BONUS_MAX;
                slowdown = new Slowdown(Slowdown.Type.Bonus)
                {
                    inTime = 0.1f,
                    stayTime = 5f,
                    outTime = 15f,
                    speedScale = 0.1f
                };
            }
        }

        //[System.Serializable]
        public class Slowdown
        {
            public enum Type
            {
                None,
                Bonus,
                BonusCatch,
                Mode
            }

            [Range(0.1f, 20f)]
            public float inTime = 1f;
            [Range(0.1f, 20f)]
            public float stayTime = 1f;
            [Range(0.1f, 20f)]
            public float outTime = 5f;
            [Range(0.0f, 1f)]
            public float speedScale = 0.1f;
            public Type type = Type.None;

            public Slowdown(Type type)
            {
                this.type = type;
            }

            public Slowdown Copy()
            {
                return new Slowdown(type)
                {
                    inTime = inTime,
                    stayTime = stayTime,
                    outTime = outTime,
                    speedScale = speedScale
                };
            }
        }

        public class DefaultSlowdown : Slowdown
        {
            public DefaultSlowdown() : base(Type.Mode)
            {
                inTime = 0.1f;
                stayTime = 2f;
                outTime = 1f;
                speedScale = 0.1f;
            }
        }

        public class BonusCatchSlowdown : Slowdown
        {
            public BonusCatchSlowdown() : base(Type.BonusCatch)
            {
                inTime = 0.1f;
                stayTime = 1f;
                outTime = 2f;
                speedScale = 0.5f;
            }
        }

        [System.Serializable]
        public class Mode
        {
            public bool scoreBased = true;
            [Range(0f, 1f)]
            public float probability = 0.5f;
            [Range(0f, 100f)]
            public float timeInterval = 10f;
            [Range(0, 100)]
            public int scoreInterval = 10;

            public Mode Copy()
            {
                return new Mode()
                {
                    scoreBased = scoreBased,
                    probability = probability,
                    timeInterval = timeInterval,
                    scoreInterval = scoreInterval
                };
            }
        }

        [System.Serializable]
        public class RandomRotation
        {
            [Range(0, 2f)]
            public float reactionTime = 1.1f;
            [Range(0f, 1f)]
            public float probability = 0.5f;
            [Range(0.1f, 10f)]
            public float timeInterval = 1f;

            public RandomRotation Copy()
            {
                return new RandomRotation()
                {
                    reactionTime = reactionTime,
                    probability = probability,
                    timeInterval = timeInterval
                };
            }
        }

        [System.Serializable]
        public class ShapesOnScreen
        {
            [HideInInspector]
            public int upToMinScoreInterval = 1;
            [Range(1, 9)]
            public int min = 1;
            [Range(1, 9)]
            public int max = 9;
            [Range(1, 100)]
            public int increaseScoreInterval = 5;
            public ShapesOnScreen Copy()
            {
                return new ShapesOnScreen()
                {
                    upToMinScoreInterval = upToMinScoreInterval,
                    min = min,
                    max = max,
                    increaseScoreInterval = increaseScoreInterval
                };
            }
        }
        [System.Serializable]
        public class ReactionTime
        {
            [Range(1, 5)]
            public float min = 1f;
            [Range(1, 5)]
            public float max = 5f;
            [Range(-1, 0)]
            public float changePerScore = -0.2f;
            public ReactionTime Copy()
            {
                return new ReactionTime()
                {
                    min = min,
                    max = max,
                    changePerScore = changePerScore
                };
            }
        }

        [System.Serializable]
        public class Rotation
        {
            [Range(0, 360)]
            public float min = 60;
            [Range(0, 360)]
            public float max = 180;
            public float Speed
            {
                get
                {
                    return Random.Range(min, max) * (Random.value < 0.5f ? -1 : 1);
                }
            }

            public Rotation Copy()
            {
                return new Rotation()
                {
                    min = min,
                    max = max
                };
            }
        }

        [System.Serializable]
        public class ModeChange
        {
            [Range(1, 3)]
            public int maxAtOnce = 1;
            [Range(0.5f, 1)]
            public float reactionTime = 0.75f;
            [Range(1, 100)]
            public int scoreCooldown = 10;
            [Range(1, 100)]
            public int startScore = 10;
            [Range(1, 100)]
            public float timeCooldown = 10;

            public ModeChange Copy()
            {
                return new ModeChange()
                {
                    maxAtOnce = maxAtOnce,
                    reactionTime = reactionTime,
                    scoreCooldown = scoreCooldown,
                    timeCooldown = timeCooldown,
                    startScore = startScore
                };
            }

        }
        [System.Serializable]
        public class Target
        {
            public bool endless = false;
            public bool scoreBased = true;
            [Range(0, 9999)]
            public int score = 100;
            [Range(0, 9999)]
            public int time = 100;

            public Target Copy()
            {
                return new Target()
                {
                    scoreBased = scoreBased,
                    time = time,
                    score = score,
                    endless = endless
                };
            }
        }

        [System.Serializable]
        public class Data
        {

            public Target target = null;

            //public Slowdown slowdown = null;
            //public Slowdown bonusCatchSlowdown = null;

            public ModeChange modeChange = null;

            public RandomRotation randomRotation = null;

            public Mode basketMode = null;
            public Mode basketShapes = null;
            public Mode basketColors = null;

            public Bonus heart = null;
            public Bonus freeze = null;
            public Bonus explosion = null;
            public ShapesOnScreen shapesOnScreen = null;
            public ReactionTime playerReactionTime = null;
            public Rotation rotation = null;
            public GameMode initialGameMode = GameMode.None;
            public List<ShapeType> additionalShapes = null;

        }

    }
}
