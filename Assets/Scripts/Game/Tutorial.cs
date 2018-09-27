using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Sound;
using UnityEngine;
using Assets.Scripts.Shape;
using Assets.Scripts.Indicator;
using Assets.Scripts.ShapeBasket;

namespace Assets.Scripts.Game
{

    public class Tutorial: MonoBehaviour
    {
        private const int BASIC_TUTORIAL_REPETITIONS = 3;
        [SerializeField]
        private TutorialGestureAnimation tutorialGestureAnimationPrototype = null;
        private class TileData
        {
            public Color color;
            public ShapeType shape;
            public bool Matches(GameMode mode, TileData other)
            {
                if (mode == GameMode.Color)
                    return color == other.color;
                else if (mode == GameMode.Shape)
                    return shape == other.shape;
                else
                    return false;
            }
        }

        private bool autoplay = false;

        private Master m = null;

        public IEnumerator TryToStart(Master master)
        {
            m = master;
            m.State.tutorial = new State.Tutorial();

            yield return BasicTutorial();
            //yield return ResetScore();

            yield return BonusCatchTutorial(ShapeType.Heart);
            yield return HeartBonusUseTutorial();


            yield return BonusCatchTutorial(ShapeType.Snowflake);
            StartCoroutine(AutoPlay(destroyShapes:false));
            yield return BonusUseTutorial(ShapeType.Snowflake);
            autoplay = false;

            yield return WaitUntilAllShapesWillBeDestroyed();
            //yield return ResetScore();

            yield return BonusCatchTutorial(ShapeType.Explosion);
            StartCoroutine(AutoPlay());
            yield return BonusUseTutorial(ShapeType.Explosion);
            
            yield return PauseTutorial();
            autoplay = false;

            yield return WaitUntilAllShapesWillBeDestroyed();
            m.State.SetGameOverData(true);
            m.Listeners.OnGameOver(true);
            m.Stop();
            
        }
        
        private IEnumerator ResetScore()
        {
            while(m.State.Score > 0)
            {
                yield return new WaitForSecondsRealtime(0.1f);
                m.State.Score--;
            }
            m.Actions.SwitchSpawnPreset(1);
        }

        private void SetTile(GameObject tile)
        {
            m.State.Mapping.TileSwitcher.SetTile(tile);
        }

        private IEnumerator WaitUntilAllShapesWillBeDestroyed()
        {
            while (m.State.shapesOnScreen.Count > 0)
                yield return null;
        }

        private GameObject SelectOtherTile()
        {
            GameObject currentTile = GetCurrentTile();
            GameObject newTile = null;
            var tiles = m.State.Mapping.ShapeMixer.Tiles;
            do
            {
                newTile = tiles[UnityEngine.Random.Range(0, tiles.Length)];
            } while (currentTile == newTile);
            SetTile(newTile);
            return newTile;
        }


        private void SwapMode()
        {
            SoundsController.PlaySound(SoundsController.SoundSFX.MODE_CHANGED);
            m.State.SwapBasketGameModes();
        }

        private IEnumerator BasicTutorial()
        {
            yield return RepeatableBasicTutorial(m.State.BasketGameMode, BASIC_TUTORIAL_REPETITIONS);
            SwapMode();
            yield return RepeatableBasicTutorial(m.State.BasketGameMode, BASIC_TUTORIAL_REPETITIONS);
            m.State.startTime = Time.time;
        }

        private IEnumerator SwipeUpDown(TutorialGestureAnimation tutorialGesture)
        {
            while (true)
            {
                if (tutorialGesture.isActiveAndEnabled)
                    yield return tutorialGesture.SwipeAnimation(TutorialGestureAnimation.SwipeAnimationType.TOP, false);
                else
                    break;
                if (tutorialGesture.isActiveAndEnabled)
                    yield return tutorialGesture.SwipeAnimation(TutorialGestureAnimation.SwipeAnimationType.BOTTOM, false);
                else
                    break;
            }
            Destroy(tutorialGesture.gameObject);
        }

        private IEnumerator PauseTutorial()
        {
            var tutorialGesture = Instantiate(tutorialGestureAnimationPrototype);
            tutorialGesture.circle.SetActive(true);
            m.State.tutorial.controls.pause = true;
            var swipeUpDwon = StartCoroutine(SwipeUpDown(tutorialGesture));
            do
            {
                yield return null;
            } while (!m.State.paused);
            tutorialGesture.gameObject.SetActive(false);
        }

        private IEnumerator HeartBonusUseTutorial()
        {
            m.State.tutorial = new State.Tutorial();
            SpawnShape();
            yield return new WaitForSecondsRealtime(0.25f);
            var shape = GetClosestShape();
            var tile = GetCurrentTile();
            if(GetTileData(tile).Matches(m.State.BasketGameMode, GetShapeData(shape)))
            {
                SelectOtherTile();
            }
            while (m.State.HeartBonuses > 0)
                yield return null;
            yield return new WaitForSecondsRealtime(2f);
        }

        private IEnumerator  BonusUseTutorial(ShapeType type)
        {
            m.State.tutorial = new State.Tutorial();
            m.State.tutorial.controls.bonusUse = true;
            var tutorialGesture = Instantiate(tutorialGestureAnimationPrototype);
            switch (type)
            {
                case ShapeType.Snowflake:
                    tutorialGesture.PlaySwipeAnimation(TutorialGestureAnimation.SwipeAnimationType.RIGHT);
                    break;
                case ShapeType.Explosion:
                    tutorialGesture.PlaySwipeAnimation(TutorialGestureAnimation.SwipeAnimationType.LEFT);
                    break;
                default:
                    Destroy(tutorialGesture);
                    yield break;
            }
            while (GetBonusCountByType(type) > 0)
                yield return null;
            tutorialGesture.StopAnimation();
            Destroy(tutorialGesture);
        }



        private IEnumerator RepeatableBasicTutorial(GameMode mode,int times = 1)
        {
            for (int i = 0; i < times; i++)
                yield return BasicTutorialMode(mode);
        }

        private IEnumerator AutoPlay(Action onComplete = null, bool destroyShapes = false)
        {
            autoplay = true;
            GameObject tile = null;
            GameObject closestShape = null;
            do
            {
                if (!autoplay && destroyShapes)
                {
                    m.Actions.ExplodeShapesOnScreen();
                }
                if (autoplay && m.State.shapesOnScreen.Count < m.State.shapesOnScreenLimit)
                {
                    SpawnShape();
                    yield return null;
                }

                var newClosestShape = GetClosestShape();
                if (newClosestShape != null && closestShape != newClosestShape)
                {
                    tile = GetCorrectTileFor(m.State.BasketGameMode, GetShapeData(newClosestShape));
                    SetTile(tile);
                    closestShape = newClosestShape;
                }
                yield return null;
            } while (autoplay || m.State.shapesOnScreen.Count > 0);
            if (onComplete != null)
                onComplete();
        }



        private IEnumerator BonusCatchTutorial(ShapeType type)
        {
            m.State.tutorial = new State.Tutorial();
            var bonus = SpawnShape(prototype: GetBonusPrototypeByType(type));
            if (bonus == null)
                yield break;
            float initialCollisionTime = m.Actions.GetCollisionTimeWithBasket(bonus);
            do
            {
                yield return null;
            } while (m.Actions.GetCollisionTimeWithBasket(bonus) > initialCollisionTime * 0.5f);
            yield return SetTimeScale(0, initialCollisionTime * 0.25f);
            var tapAnimation = Instantiate(tutorialGestureAnimationPrototype);
            PlaceTouchAnimationAt(tapAnimation, bonus);
            tapAnimation.PlayTapAnimation();
            m.State.tutorial.controls.bonusPick = true;
            while (GetBonusCountByType(type) == 0)
                yield return null;
            tapAnimation.StopAnimation();
            yield return SetTimeScale(1, initialCollisionTime * 0.25f);
            Destroy(tapAnimation.gameObject);
            while (m.State.shapesOnScreen.Count > 0)
                yield return null;

        }

        private IEnumerator BasicTutorialMode(GameMode mode)
        {
            m.State.tutorial.controls.backet = false;
            SpawnShape();
            yield return new WaitForSecondsRealtime(0.25f);
            var shape = GetClosestShape();
            var shapeData = GetShapeData(shape);
            var prevTile = GetCurrentTile();
            var tileData = GetTileData(prevTile);
            var correctTile = GetCorrectTileFor(mode, shapeData);

            if (tileData.Matches(mode, shapeData))
            {
                prevTile = SelectOtherTile();
                tileData = GetTileData(prevTile);
            }

            float initialCollisionTime = m.Actions.GetCollisionTimeWithBasket(shape);
            while (m.Actions.GetCollisionTimeWithBasket(shape) > initialCollisionTime * 0.45f)
                yield return null;

            var touchAnimation = Instantiate(tutorialGestureAnimationPrototype);
            PlaceTouchAnimationAt(touchAnimation, correctTile);
            touchAnimation.PlayTapAnimation();
            yield return SetTimeScale(0, initialCollisionTime * 0.25f);
            m.State.tutorial.controls.backet = true;
            do
            {
                var currentTile = GetCurrentTile();
                if (currentTile != prevTile)
                {
                    prevTile = currentTile;
                    tileData = GetTileData(currentTile);
                }
                yield return null;
            } while (!tileData.Matches(mode,shapeData));
            m.State.tutorial.controls.backet = false;
            Destroy(touchAnimation.gameObject);
            yield return SetTimeScale(1, initialCollisionTime * 0.25f);
            while (m.State.shapesOnScreen.Count > 0)
            {
                yield return null;
            }
        }

        private void PlaceTouchAnimationAt(TutorialGestureAnimation animation, GameObject tile)
        {
            Vector3 oldPosition = animation.transform.position;
            Vector3 newPosition = tile.transform.position;
            newPosition.z = oldPosition.z;
            animation.transform.position = newPosition;
        }

        private IEnumerator SetTimeScale(float end, float time)
        {
            float start = Time.timeScale;
            float stage = 0;
            while(Time.timeScale != end)
            {
                stage += Time.unscaledDeltaTime / time;
                Time.timeScale = Mathf.Lerp(start, end, stage);
                yield return null;
            }
        }

        private GameObject GetCurrentTile()
        {
            return m.State.Mapping.TileSwitcher.Tile;
        }

        private GameObject GetCorrectTileFor(GameMode mode, TileData shape)
        {
            foreach(var tile in m.State.Mapping.ShapeMixer.Tiles)
            {
                if (GetTileData(tile).Matches(mode, shape))
                    return tile;
            }
            return null;
        }

        private TileData GetClosestShapeData()
        {
            return GetShapeData(GetClosestShape());
        }

        private TileData GetTileData(GameObject tile)
        {
            return new TileData()
            {
                shape = m.State.Mapping.ShapeMixer.tileToShape[tile].GetComponent<ShapeToTile>().shapeType,
                color = tile.GetComponent<MeshRenderer>().sharedMaterial.color
            };
        }

        private TileData GetShapeData(GameObject shape)
        {
            return new TileData()
            {
                color = shape.GetComponent<ColorPicker>().GetColor(),
                shape = shape.GetComponent<RandomRotation>().CurrentRotation.type
            };
        }

        private GameObject GetClosestShape(bool ignoreBonus = false)
        {
            float min = float.PositiveInfinity, time = float.PositiveInfinity;
            GameObject closest = null;
            foreach (var shape in m.State.shapesOnScreen)
            {
                var controller = shape.GetComponent<Shape.Controller>();
                if (controller.Destruction.IsPartiallyDestroyed || ignoreBonus && controller.Bonus != null)
                    continue;
                time = m.Actions.GetCollisionTimeWithBasket(shape);
                if(time < min)
                {
                    closest = shape;
                    min = time;
                }
            }
            return closest;
        }

        //private IEnumerator SpawnShapes(int number, int shapesOnScreen, float time = 0.1f)
        //{
        //    if(shapesOnScreen>0)
        //        m.Actions.SwitchSpawnPreset(shapesOnScreen);
        //    for(int i = 0; i < number; i++)
        //    {
        //        var shape = m.Actions.GetNextShape(false);
        //        if (shape != null)
        //            m.State.shapesOnScreen.Add(shape);
        //        yield return new WaitForSecondsRealtime(time);
        //    }
        //}

        private GameObject GetBonusPrototypeByType(ShapeType type)
        {
            switch (type)
            {
                case ShapeType.Snowflake:
                    return m.State.Mapping.bonus.freeze;
                case ShapeType.Explosion:
                    return m.State.Mapping.bonus.explosion;
                case ShapeType.Heart:
                    return m.State.Mapping.bonus.heart;
            }
            return null;
        }

        private GameObject SpawnShape(bool bonus = false, GameObject prototype = null)
        {
            var shape = m.Actions.GetNextShape(prototype: prototype, bonus:bonus);
            if (shape != null)
                m.State.shapesOnScreen.Add(shape);
            return shape;
        }

        private int GetBonusCountByType(ShapeType type)
        {
            switch (type)
            {
                case ShapeType.Snowflake:
                    return m.State.FreezeBonuses;
                case ShapeType.Explosion:
                    return m.State.ExplosionBonuses;
                case ShapeType.Heart:
                    return m.State.HeartBonuses;
            }
            return 0;
        }
    }
}
