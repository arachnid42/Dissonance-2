using System;
using System.Collections;
using Assets.Scripts.Sound;
using UnityEngine;
using Assets.Scripts.Shape;
using Assets.Scripts.Indicator;
using Assets.Scripts.ShapeBasket;

namespace Assets.Scripts.Game
{

    public class Tutorial: MonoBehaviour
    {
        private const int BASIC_TUTORIAL_REPETITIONS = 2;
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

        private Master m = null;

        public IEnumerator TryToStart(Master master)
        {
            m = master;
            return BasicTutorial();
        }
        
        private void SetTile(GameObject tile)
        {
            m.State.Mapping.TileSwitcher.SetTile(tile);
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


        private IEnumerator RepeatableBasicTutorial(GameMode mode,int times = 1)
        {
            for (int i = 0; i < times; i++)
                yield return BasicTutorialMode(mode);
        }

        private IEnumerator BasicTutorialMode(GameMode mode)
        {
            yield return SpawnShapes(1, 1);

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

        private IEnumerator SpawnShapes(int number, int shapesOnScreen, float time = 0.1f)
        {
            m.Actions.SwitchSpawnPreset(shapesOnScreen);
            for(int i = 0; i < number; i++)
            {
                var shape = m.Actions.GetNextShape(false);
                if (shape != null)
                    m.State.shapesOnScreen.Add(shape);
                yield return new WaitForSecondsRealtime(time);
            }
        }
    }
}
