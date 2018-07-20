using System;
using Assets.Scripts.Shape;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Assets.Scripts.Game
{

#if UNITY_EDITOR
    [CustomEditor(typeof(DifficultyLevelsGenerator))]
    public class DifficultyLevelsGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var generator = (DifficultyLevelsGenerator)target;
            if (GUILayout.Button("BUILD"))
            {
                generator.Build();
            }
            if (GUILayout.Button("CLEAR"))
            {
                generator.Clear();
            }
            DrawDefaultInspector();
        }
    }
#endif

    public class DifficultyLevelsGenerator : MonoBehaviour
    {

        [Serializable]
        public class Tier
        {
            public static ShapeType[] additional = new ShapeType[]
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


            [Serializable]
            public class Config
            {
                public int startChangeLevelIndex = 0;
                public int endChangeLevelIndex = 9;

                public int ChangeIndex(int i)
                {
                    if (i <= endChangeLevelIndex)
                    {
                        return Mathf.Max(0, i - startChangeLevelIndex);
                    }
                    else
                    {
                        return endChangeLevelIndex;
                    }
                }

            }

            [Serializable]
            public class ModeChange : Config
            {

                public int maxAtOnce = 1;
                public float maxAtOnceChangePerLevel = 0;

                public int scoreCooldown = 1;
                public float scoreCooldownChangePerLevel = 0f;

                public float timeCooldown = 1;
                public float timeCooldownChangePerLevel = 0f;

                public int startScore = 1;
                public float startScoreChangePerLevel = 0;

                public float reactionTime = 0.75f;
                public float reactionTimeChangePerLevel = 0;

                public void Update(Difficulty.ModeChange modeChange, int i)
                {
                    i = ChangeIndex(i);
                    modeChange.scoreCooldown = Mathf.FloorToInt(Mathf.Max(scoreCooldown + scoreCooldownChangePerLevel*i,1));
                    modeChange.maxAtOnce = Mathf.FloorToInt(Mathf.Clamp(maxAtOnce + maxAtOnceChangePerLevel * i, Difficulty.MIN_AT_ONCE_MODE_CHANGES, Difficulty.MAX_AT_ONCE_MODE_CHANGES));
                    modeChange.reactionTime = Mathf.Clamp(reactionTime + i * reactionTimeChangePerLevel, Difficulty.MIN_MODE_CHANGE_REACTION_TIME, Difficulty.MAX_MODE_CHANGE_REACTION_TIME);
                    modeChange.startScore = Mathf.FloorToInt(Mathf.Max(0, startScore + startScoreChangePerLevel * i));
                }

            }
            [Serializable]
            public class Mode : Config
            {
                public int scoreInterval = 10;
                public float scoreIntervalChangePerLevel = 0;

                public float timeInterval = 10;
                public float timeIntervalChangePerLevel = 0;

                public float probability = 0f;
                public float probabilityChangePerLevel = 0;

                public float scoreBasedDivider = 1;

                public void Update(Difficulty.Mode mode, int i)
                {
                    i = ChangeIndex(i);
                    mode.scoreBased = i % scoreBasedDivider == 0;
                    mode.scoreInterval = Mathf.FloorToInt(Mathf.Max(scoreInterval + scoreIntervalChangePerLevel * i, 0));
                    mode.timeInterval = Mathf.Max(scoreInterval + scoreIntervalChangePerLevel * i, 1);
                    mode.probability = Mathf.Clamp(probability + probabilityChangePerLevel * i, 0, 1);
                }
            }

            [Serializable]
            public class Bonus : Config
            {
                public int number = 0;
                public float numberChangePerLevel = 0;

                public float probability = 0;
                public float probabilityChangePerLevel = 0;

                public int scoreInterval = 1;
                public float scoreIntervalChangePerLevel = 0;

                public int startScore = 1;
                public float startScoreChangePerLevel = 0;

                public void Update(Difficulty.Bonus bonus,int i)
                {
                    i = ChangeIndex(i);
                    bonus.probability = Mathf.Clamp(probability + probabilityChangePerLevel * i, 0.0f, 1f);
                    bonus.scoreInterval = Mathf.FloorToInt(Mathf.Max(scoreInterval + scoreIntervalChangePerLevel * i, 0));
                    bonus.number = Mathf.FloorToInt(Mathf.Max(number + numberChangePerLevel * i, 0));
                    bonus.startScore = Mathf.FloorToInt(Math.Max(startScore + startScoreChangePerLevel * i, 0));
                }

            }
             



            [Serializable]
            public class Target : Config
            {

                public int score = 10;
                public int perLevelScoreIncrease = 5;
                public int time = 20;
                public int perLevelTimeIncrease = 10;
                public int scoreBasedDivider = 2;

                public void Update(Difficulty.Target target, int i)
                {
                    i = ChangeIndex(i);
                    target.score = Mathf.FloorToInt(Mathf.Clamp(score + i * perLevelScoreIncrease, 1, Difficulty.MAX_TARGET_SCORE));
                    target.time = Mathf.FloorToInt(Mathf.Clamp(time + i * perLevelTimeIncrease, 1, Difficulty.MAX_TARGET_TIME));
                    target.scoreBased = i % scoreBasedDivider == 0;
                }

            }

            [Serializable]
            public class PlayerReactionTime : Config
            {

                public float min = 4f;
                public float minChangePerLevel = -0.1f;
                public float max = 4f;
                public float maxChangePerLevel = -0.1f;
                public float changePerScore = -0.1f;
                public float changePerScoreChangePerLevel = -0.1f;

                public void Update(Difficulty.ReactionTime reactionTime, int i)
                {
                    i = ChangeIndex(i);
                    reactionTime.min = Mathf.Clamp(min + i * minChangePerLevel, Difficulty.MIN_REACTION_TIME, Difficulty.MAX_REACTION_TIME);
                    reactionTime.max = Mathf.Clamp(max + i * maxChangePerLevel, reactionTime.min, Difficulty.MAX_REACTION_TIME);
                    reactionTime.changePerScore = Mathf.Clamp(changePerScore + i * changePerScoreChangePerLevel, -(reactionTime.max - reactionTime.min), 0);
                }
            }

            [Serializable]
            public class Rotation : Config
            {
                public float min = 60;
                public float minChangePerLevel = 2.5f;
                public float max = 90;
                public float maxChangePerLevel = 2.5f;
                public void Update(Difficulty.Rotation rotation, int i)
                {
                    i = ChangeIndex(i);
                    rotation.min = Mathf.Clamp(rotation.min + i * minChangePerLevel, Difficulty.MIN_ROTATION_SPEED, Difficulty.MAX_ROTATION_SPEED);
                    rotation.max = Mathf.Clamp(rotation.max + i * maxChangePerLevel, rotation.min, Difficulty.MAX_ROTATION_SPEED);
                }
            }

            [Serializable]
            public class RandomRotation : Config
            {

                public float probability = 0.0f;
                public float probabilityChangePerLevel = 0.1f;
                public float reactionTime = 1f;
                public float reactionTimeChangePerLevel = -0.1f;
                public float rotationReactionTime = 0.25f;
                public float rotationReactionTimeChangePerLevel = -0.1f;
                public float timeInterval = 1f;
                public float timeIntervalChangePerLevel = 0;

                public void Update(Difficulty.RandomRotation randomRotation, int i)
                {
                    i = ChangeIndex(i);
                    randomRotation.probability = Mathf.Clamp(probability + i * probabilityChangePerLevel, 0, 1);
                    randomRotation.timeInterval = Mathf.Max(0, timeInterval + timeIntervalChangePerLevel * i);
                    randomRotation.reactionTime = Mathf.Clamp(reactionTime + i * reactionTimeChangePerLevel, Difficulty.MIN_RANDOM_ROTATION_REACTION_TIME, Difficulty.MAX_RANDOM_ROTATION_REACTION_TIME);
                    randomRotation.rotationReactionTime = Mathf.Clamp(rotationReactionTime + i * rotationReactionTimeChangePerLevel, randomRotation.reactionTime * Difficulty.MIN_RANDOM_ROTATION_TIME_MUL, randomRotation.reactionTime * Difficulty.MAX_RANDOM_ROTATION_TIME_MUL);
                }
                
            }

            [Serializable]
            public class ShapesOnScreen : Config
            {
                public int min = 1;
                public float minChangePerLevel = 0.25f;
                public float max = 1;
                public float maxChangePerLevel = 0.25f;
                public int interval = 1;
                public float intervalChangePerLevel = 0.25f;

                public void Update(Difficulty.ShapesOnScreen shapesOnScreen, int i)
                {
                    i = ChangeIndex(i);
                    shapesOnScreen.min = Mathf.FloorToInt(Mathf.Clamp(min + minChangePerLevel * i, Difficulty.MIN_SHAPES_ON_SCREEN, Difficulty.MAX_SHAPES_ON_SCREEN));
                    shapesOnScreen.max = Mathf.FloorToInt(Mathf.Clamp(max + maxChangePerLevel * i, shapesOnScreen.min, Difficulty.MAX_SHAPES_ON_SCREEN));
                    shapesOnScreen.increaseScoreInterval = Mathf.FloorToInt(Mathf.Max(0, interval + i * intervalChangePerLevel));
                }
            }

            [Serializable]
            public class InitialBaskeMode : Config
            {
                public int shapeModeDivider = 2;

                public void Update(Difficulty difficulty, int i)
                {
                    i = ChangeIndex(i);
                    if (i == endChangeLevelIndex)
                    {
                        difficulty.initialGameMode = Indicator.GameMode.None;
                        return;
                    }
                    difficulty.initialGameMode = i % shapeModeDivider == 0 ? Indicator.GameMode.Shape : Indicator.GameMode.Color;
                }
            }

            public int levels = 10;
            public Target target = new Target();
            public PlayerReactionTime reactionTime = new PlayerReactionTime();
            public ShapesOnScreen shapesOnScreen = new ShapesOnScreen();

            public RandomRotation randomRotation = new RandomRotation();
            public Rotation rotation = new Rotation();

            public InitialBaskeMode initialBaskeMode = new InitialBaskeMode();

            public ModeChange mode = new ModeChange();

            public Mode basketMode = new Mode(), basketColors = new Mode(), basketShapes = new Mode();
            public Bonus freezeBonus = new Bonus(), explosionBonus = new Bonus(), heartBonus = new Bonus();

            public void Update(Difficulty difficulty,int tierIndex,int levelIndex)
            {
                mode.Update(difficulty.modeChange, levelIndex);
                basketMode.Update(difficulty.basketMode, levelIndex);
                basketColors.Update(difficulty.basketColors, levelIndex);
                basketShapes.Update(difficulty.basketShapes, levelIndex);

                heartBonus.Update(difficulty.heart, levelIndex);
                explosionBonus.Update(difficulty.explosion, levelIndex);
                freezeBonus.Update(difficulty.freeze, levelIndex);

                rotation.Update(difficulty.rotation, levelIndex);
                randomRotation.Update(difficulty.randomRotation, levelIndex);

                shapesOnScreen.Update(difficulty.shapesOnScreen, levelIndex);

                reactionTime.Update(difficulty.playerReactionTime, levelIndex);

                target.Update(difficulty.target, levelIndex);

                difficulty.additionalShapes = new List<ShapeType>(additional.Take(Mathf.Clamp(tierIndex, 0, additional.Length)));

                initialBaskeMode.Update(difficulty, levelIndex);

            }
        }

        public GameObject baseLevel = null;
        public Tier[] tiers = new Tier[0];
        public List<GameObject> levels = new List<GameObject>();
             

        private string GenerateLevelName(int tierIndex, int levelIndex)
        {
            return string.Format("T:{0} L:{1}", tierIndex+1, levelIndex+1);
        }

        private GameObject SpawnLevel(Tier tier,int tierIndex , int levelIndex)
        {

            var level = Instantiate(baseLevel, transform);
            level.name = GenerateLevelName(tierIndex, levelIndex);
            tier.Update(level.GetComponent<Difficulty>(), tierIndex, levelIndex);
            return level;
        }

        public void Build()
        {
            Clear();
            for(int i = 0; i < tiers.Length; i++)
            {
                BuildTier(tiers[i], i);
            }
            MoveHiddenLevels();
        }

        public void MoveHiddenLevels()
        {
            var difficultyLevels = GetComponent<DifficultyLevels>();
            for(int i = 0; i < difficultyLevels.hiddenLevelsEndOffset; i++)
            {
                transform.GetChild(0).SetAsLastSibling();
            }
        }

        public void BuildTier(Tier tier, int index)
        {
            for(int i = 0; i < tier.levels; i++)
            {
                levels.Add(SpawnLevel(tier, index, i));
            }
        }

        public void Clear()
        {
            foreach(var level in levels)
            {
                DestroyImmediate(level);
            }
            levels.Clear();
        }

    }
}
