using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Shape;
using Assets.Scripts.Indicator;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Scripts.Game
{

#if UNITY_EDITOR
    [CustomEditor(typeof(Difficulty)), CanEditMultipleObjects]
    public class DifficultyEditor : Editor
    {
        private static ShapeType[] additional = new ShapeType[]
        {
            ShapeType.Circle,
            ShapeType.Diamond,
            ShapeType.Pentagon,
            ShapeType.Pentagram,
            ShapeType.Hexagon,
            ShapeType.Hexagram,
            ShapeType.Octogram,
            ShapeType.Decagram
        };

        private static ShapeType[] basic = new ShapeType[]
        {
            ShapeType.Square,
            ShapeType.Triangle
        };

        private bool visible = false;
        private Difficulty difficulty = null;
        private SerializedProperty additionalShapes = null;
        public void Awake()
        {
            difficulty = (Difficulty)target;
            additionalShapes = serializedObject.FindProperty("additionalShapes");
        }


        private int IndexOf(ShapeType type)
        {
            for (int i = 0; i < additionalShapes.arraySize; i++)
                if (additionalShapes.GetArrayElementAtIndex(i).enumValueIndex == (int)type)
                    return i;
            return -1;
        }

        private bool Contains(ShapeType type)
        {
            return IndexOf(type) >= 0;
        }

        private void Remove(ShapeType type)
        {
            int index = IndexOf(type);
            if (index >= 0)
            {
                additionalShapes.DeleteArrayElementAtIndex(index);
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void Add(ShapeType type)
        {
            //Debug.Log("Add");
            if (IndexOf(type) < 0)
            {
                //Debug.Log("INDEX CHECK PASSED!");
                int index = additionalShapes.arraySize;
                additionalShapes.InsertArrayElementAtIndex(index);
                additionalShapes.GetArrayElementAtIndex(index).enumValueIndex = (int)type;
                serializedObject.ApplyModifiedProperties();
            }
                
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            //GUILayout.Label(string.Format("IS ARRAY:{0} SIZE:{1}", additionalShapes.isArray, additionalShapes.arraySize));
            visible = EditorGUILayout.Foldout(visible, "Shape Types:" + (additionalShapes.arraySize + RotationDescriptor.basicTypes.Length), true);
            if (!visible)
                return;

            EditorGUI.indentLevel++;
            EditorGUI.BeginDisabledGroup(true);
            foreach (var type in basic)
            {
                GUILayout.Toggle(true, type.ToString());
            }
            EditorGUI.EndDisabledGroup();
            foreach(var type in additional)
            {
                
                bool value = Contains(type);
                if (GUILayout.Toggle(value, type.ToString()))
                {
                    if (!value)
                    {
                        Add(type);
                    }
                        
                }
                else
                {
                    if (value)
                    {
                        Remove(type);
                    }
                        
                }
            }
            EditorGUI.indentLevel--;
        }
    }

#endif

    [System.Serializable]
    public class Difficulty : MonoBehaviour
    {
        public const float

            BONUS_AUTO_USE_REACTION_TIME = 1.3f,

            MIN_REACTION_TIME = 0.75f,
            MAX_REACTION_TIME = 5,

            MIN_MODE_CHANGE_REACTION_TIME = 0.6f,
            MAX_MODE_CHANGE_REACTION_TIME = 0.9f,

            MIN_RANDOM_ROTATION_REACTION_TIME = 0.5f,
            MAX_RANDOM_ROTATION_REACTION_TIME = 1f,

            MAX_RANDOM_ROTATION_TIME_MUL = 0.5f,
            MIN_RANDOM_ROTATION_TIME_MUL = 0.1f,

            EXPLOSION_BONUS_REACTION_TIME_MUL = 1.75f,
            FREEZE_BONUS_REACTION_TIME_MUL = 2.0f,
            HEART_BONUS_REACTION_TIME_MUL = 1.25f,

            MAX_ROTATION_SPEED = 360,
            MIN_ROTATION_SPEED = 15;
        public const int 
            
            MAX_TARGET_SCORE = 9999,
            MAX_TARGET_TIME = 9999, 

            HEART_BONUS_MAX = 9,
            FREEZE_BONUS_MAX = 99,
            EXPLOSION_BONUS_MAX = 99,

            MIN_SHAPES_ON_SCREEN = 1,
            MAX_SHAPES_ON_SCREEN = 9,
            
            MAX_AT_ONCE_MODE_CHANGES = 3,
            MIN_AT_ONCE_MODE_CHANGES = 1;


        [System.Serializable]
        public class Bonus
        {
            [HideInInspector]
            public int max = 4;
            [Range(0, 100)]
            public int number = 1;
            [Range(1, 100)]
            public int scoreInterval = 10;
            [Range(0f, 1f)]
            public float probability = 0.5f;
            [Range(0, 100)]
            public int startScore = 10;
            public Bonus(int max)
            {
                this.max = max;
            }
            public Slowdown slowdown = new Slowdown(Slowdown.Type.Bonus);
            public bool useSlowdown = true;

            public Bonus Copy()
            {
                return new Bonus(max)
                {
                    number = number,
                    scoreInterval = scoreInterval,
                    probability = probability,
                    startScore = startScore,
                    slowdown = slowdown.Copy()
                };
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
        [System.Serializable]
        public class RandomRotation
        {
            [Range(0,0.5f)]
            public float rotationReactionTime = 0.5f;
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
                    rotationReactionTime = rotationReactionTime,
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
            [Range(1,9)]
            public int min = 1;
            [Range(1,9)]
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
            [Range(0,9999)]
            public int score = 100;
            [Range(0,9999)]
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

            public Slowdown slowdown = null;
            public Slowdown bonusCatchSlowdown = null;

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

        public Target target = new Target();

        public Slowdown slowdown = new Slowdown(Slowdown.Type.Mode);
        public Slowdown bonusCatchSlowdown = new Slowdown(Slowdown.Type.BonusCatch);

        public RandomRotation randomRotation = new RandomRotation();
        public Rotation rotation = new Rotation();

        public GameMode initialGameMode = GameMode.None;
        public ModeChange modeChange = new ModeChange();
        public Mode basketMode = new Mode();
        public Mode basketShapes = new Mode();
        public Mode basketColors = new Mode();

        public Bonus heart = new Bonus(HEART_BONUS_MAX);
        public Bonus freeze = new Bonus(FREEZE_BONUS_MAX);
        public Bonus explosion = new Bonus(EXPLOSION_BONUS_MAX);

        public ShapesOnScreen shapesOnScreen = new ShapesOnScreen();
        public ReactionTime playerReactionTime = new ReactionTime();

        [HideInInspector]
        public List<ShapeType> additionalShapes;

        public void UpdateData(Data data)
        {
            target = data.target.Copy();

            slowdown = data.slowdown.Copy();
            bonusCatchSlowdown = data.bonusCatchSlowdown.Copy();

            randomRotation = data.randomRotation.Copy();
            rotation = data.rotation.Copy();

            initialGameMode = data.initialGameMode;
            modeChange = data.modeChange.Copy();
            basketMode = data.basketMode.Copy();
            basketShapes = data.basketShapes.Copy();
            basketColors = data.basketColors.Copy();

            heart = data.heart.Copy();
            explosion = data.explosion.Copy();
            freeze = data.freeze.Copy();

            shapesOnScreen = data.shapesOnScreen.Copy();
            playerReactionTime = data.playerReactionTime.Copy();

            additionalShapes = new List<ShapeType>(data.additionalShapes);
        }

        public Data GetData()
        {
            return new Data()
            {
                target = target.Copy(),

                slowdown = slowdown.Copy(),
                bonusCatchSlowdown = bonusCatchSlowdown.Copy(),

                randomRotation = randomRotation.Copy(),
                rotation = rotation.Copy(),

                initialGameMode = initialGameMode,
                modeChange = modeChange.Copy(),
                basketMode = basketMode.Copy(),
                basketShapes = basketShapes.Copy(),
                basketColors = basketColors.Copy(),

                heart = heart.Copy(),
                explosion = explosion.Copy(),
                freeze = freeze.Copy(),

                shapesOnScreen = shapesOnScreen.Copy(),
                playerReactionTime = playerReactionTime.Copy(),

                additionalShapes = new List<ShapeType>(additionalShapes)
            };
        }

    }
    
}
