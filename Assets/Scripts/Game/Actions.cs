using Assets.Scripts.Shape;
using Assets.Scripts.ShapeBasket;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game
{

    public class Actions
    {

        private Master master = null;

        public Actions(Master master)
        {
            this.master = master;
        }

        public bool CheckDifficultyTarget()
        {
            if (master.State.Difficulty.target.endless)
            {
                return false;
            }
            else if (master.State.Difficulty.target.scoreBased)
            {
                return master.State.Score >= master.State.Difficulty.target.score;
            }
            else
            {
                return GetTimerTimeLeft() <= 0;
            }
        }

        public float GetTimerTimeLeft()
        {
            if (master.State.Score <= 0 || master.State.Difficulty.target.scoreBased)
                return master.State.Difficulty.target.time;
            return master.State.Difficulty.target.time - (Time.time - master.State.firstScoreTime);
        }

        public void UpdateTimer()
        {
            if (master.State.Difficulty.target.endless)
            {
                master.State.Timer += master.State.score <= 0 ? 0 : Time.deltaTime;
            }
            else
            {
                master.State.Timer = GetTimerTimeLeft();
            }
        }

        public GameObject FindShapeClosestSpawn(GameObject shape)
        {
            GameObject[] spawns = master.State.ActiveSpawnPreset.spawns;
            GameObject closestSpawn = spawns[0];
            float xDistance = Mathf.Abs(closestSpawn.transform.position.x - shape.transform.position.x);
            for(int i = 1; i <spawns.Length; i++)
            {
                float newXDistance = Mathf.Abs(spawns[i].transform.position.x - shape.transform.position.x);
                if (xDistance > newXDistance)
                {
                    xDistance = newXDistance;
                    closestSpawn = spawns[i];
                }
            }
            return closestSpawn;
        }

        public float GetDistanceBetweenBasketAndShape(GameObject shape)
        {

            SphereCollider shapeCollider = shape.GetComponentInChildren<SphereCollider>();
            Collider basketCollider = master.State.Mapping.field.shapeBasket.GetComponent<Collider>();
            return Mathf.Abs((basketCollider.bounds.center.y + basketCollider.bounds.extents.y) - (shapeCollider.bounds.center.y - shapeCollider.radius));
        }

        public float GetCollisionTimeWithBasket(GameObject shape)
        {
            float distance = GetDistanceBetweenBasketAndShape(shape);
            return distance / shape.GetComponentInChildren<Falling>().speed;
        }

        public float GetSpeedForCollisionTime(GameObject shape, float time)
        {
            return GetDistanceBetweenBasketAndShape(shape) / time;
        }

        public float GetMaxCollisionTimeWithBasket(List<GameObject> shapes)
        {
            float max = float.NegativeInfinity;
            float collisionTime;
            foreach (GameObject shape in shapes)
            {
                collisionTime = GetCollisionTimeWithBasket(shape);
                if (collisionTime > max)
                {
                    max = collisionTime;
                }
            }
            return max;
        }


        public float GetMinCollisionTimeWithBasket(List<GameObject> shapes)
        {
            float min = float.PositiveInfinity;
            float collisionTime;
            foreach (GameObject shape in shapes)
            {
                collisionTime = GetCollisionTimeWithBasket(shape);
                if (collisionTime < min)
                {
                    min = collisionTime;
                }
            }
            return min;
        }


        public GameObject FindSuitableSpawn(float basketCollisionTime)
        {
            List<GameObject> suitableSpawns = new List<GameObject>();
            foreach (GameObject spawn in master.State.ActiveSpawnPreset.spawns)
            {
                List<GameObject> shapesList = master.State.shapesToSpawns[spawn];
                if (master.State.shapesToSpawns.Count > 0)
                {
                    float maxCollisionTime = GetMaxCollisionTimeWithBasket(shapesList);
                    if (basketCollisionTime > maxCollisionTime + master.State.PlayerReactionTime)
                    {
                        suitableSpawns.Add(spawn);
                    }
                }
                else
                {
                    suitableSpawns.Add(spawn);
                }
            }
            if (suitableSpawns.Count == 0)
            {
                return null;
            }
            return suitableSpawns[Random.Range(0, suitableSpawns.Count)];
        }


        public float GetReactionTimeForNextShape()
        {
            if (master.State.shapesOnScreen.Count == 0)
            {
                return master.State.PlayerReactionTime * master.State.shapesOnScreenLimit;
            }
            List<float> shapesCollisionTime = new List<float>(master.State.shapesOnScreen.Count);
            foreach (GameObject shape in master.State.shapesOnScreen)
            {
                shapesCollisionTime.Add(GetCollisionTimeWithBasket(shape));
            }
            shapesCollisionTime.Sort();
            List<Vector2> intervals = new List<Vector2>();
            for (int i = 0; i < shapesCollisionTime.Count; i++)
            {
                bool first = i - 1 < 0;
                bool last = i + 1 >= shapesCollisionTime.Count;
                float collisionTime = shapesCollisionTime[i];
                if (first || last)
                {
                    if (first)
                    {
                        intervals.Add(new Vector2(0, collisionTime));
                    }
                    if (last)
                    {
                        intervals.Add(new Vector2(collisionTime, master.State.PlayerReactionTime * master.State.shapesOnScreenLimit));
                    }
                    continue;
                }
                float prevCollisionTime = shapesCollisionTime[i - 1];
                intervals.Add(new Vector2(prevCollisionTime, collisionTime));
            }
            List<Vector2> suitableIntervals = new List<Vector2>(intervals.Count);
            for (int i = 0; i < intervals.Count; i++)
            {
                Vector2 interval = intervals[i];
                float length = interval.y - interval.x;
                if (i == intervals.Count - 1)
                {
                    if (length >= master.State.PlayerReactionTime)
                    {
                        suitableIntervals.Add(interval);
                    }
                }
                else if (length - master.State.PlayerReactionTime >= master.State.PlayerReactionTime)
                {
                    suitableIntervals.Add(interval);
                }
            }
            if (suitableIntervals.Count == 0)
            {
                return -1;
            }
            Vector2 selectedInterval = suitableIntervals[Random.Range(0, suitableIntervals.Count)];
            return selectedInterval.x + master.State.PlayerReactionTime;
        }

        public bool CanSpawnBonus(State.Bonus State, Difficulty.Bonus difficulty)
        {
            //int score = master.State.score + master.State.scoreOffset;

            bool can = State.current + State.onScreen < difficulty.max && State.number < difficulty.number && master.State.score - State.lastScore - difficulty.startScore >= difficulty.scoreInterval;
            if (can)
            {
                State.lastScore = master.State.score;
                bool probability = difficulty.probability > Random.value;
                if (probability)
                {
                    State.number++;
                }
                return can && probability;

            }
            return false;
   
        }

        public GameObject GetNextBonus()
        {
            if(CanSpawnBonus(master.State.heart, master.State.Difficulty.heart))
            {
                master.State.heart.onScreen++;
                return master.State.Mapping.bonus.heart;
            }else if(CanSpawnBonus(master.State.freeze, master.State.Difficulty.freeze))
            {
                master.State.freeze.onScreen++;
                return master.State.Mapping.bonus.freeze;
            }else if(CanSpawnBonus(master.State.explosion, master.State.Difficulty.explosion))
            {
                master.State.explosion.onScreen++;
                return master.State.Mapping.bonus.explosion;
            }
            return null;
        }

        public GameObject GetNextRandomShape()
        {
            return master.State.Mapping.shapes[Random.Range(0, master.State.Mapping.shapes.Length)];
        }

        public GameObject CreateRandomShape(float collisionTime)
        {
            GameObject suitableSpawn = FindSuitableSpawn(collisionTime);
            if(suitableSpawn == null)
                return null;
            GameObject selectedShape = GetNextBonus();
            if(selectedShape == null)
                selectedShape = GetNextRandomShape();

            GameObject shape = Object.Instantiate(selectedShape, suitableSpawn.transform.position, suitableSpawn.transform.rotation);
            Shape.Controller shapeController = shape.GetComponent<Shape.Controller>();
            master.Callbacks.SetShapeCallbacks(shape);
            shapeController.Scale.SetScale(master.State.ActiveSpawnPreset.scale);
            shapeController.Falling.speed = GetSpeedForCollisionTime(shape, collisionTime);
            shapeController.Falling.slowdown = master.State.slowdown;
            shapeController.Rotation.slowdown = master.State.slowdown;
            shapeController.RandomRotation.slowdown = master.State.slowdown;
            shapeController.Rotation.speed = master.State.Difficulty.rotation.Speed;
            master.State.shapesToSpawns[suitableSpawn].Add(shape);
            return shape;
        }


        public GameObject GetNextShape()
        {
            float collisionTime = GetReactionTimeForNextShape();
            if (collisionTime < 0)
            {
                return null;
            }
            GameObject shape = CreateRandomShape(collisionTime);
            shape.SetActive(true);
            return shape;
        }


        public bool CanRotateShape(GameObject shape)
        {
            if (shape.GetComponent<Destruction>().Started)
            {
                return false;
            }
            float reactionTimeAfterRotation = GetCollisionTimeWithBasket(shape) * (1 - Difficulty.RANDOM_ROTATION_REACTION_TIME_MUL);
            return reactionTimeAfterRotation >= master.State.Difficulty.playerReactionTime.min * master.State.Difficulty.randomRotation.reactionTime;
        }

        public float GetRotationTime(GameObject shape)
        {
            return GetCollisionTimeWithBasket(shape.gameObject) * Difficulty.RANDOM_ROTATION_REACTION_TIME_MUL;
        }

        public void TryToStartRandomRotation()
        {
            var state = master.State;
            var difficulty = state.Difficulty.randomRotation;

            // if reaction time is not minimal target reaction time multiplyer shoul be tweaked accordingly

            if (Time.time - state.randomRotation.lastTime >= difficulty.timeInterval)
            {

                List<RandomRotation> randomRotationCandidates = new List<RandomRotation>(state.shapesOnScreen.Count);
                foreach (GameObject shape in state.shapesOnScreen)
                {
                    if (CanRotateShape(shape))
                    {
                        var randomRotation = shape.GetComponent<Shape.Controller>().RandomRotation;
                        if(!randomRotation.Started)
                            randomRotationCandidates.Add(randomRotation);
                    }
                }

                if (randomRotationCandidates.Count > 0)
                {
                    RandomRotation rotation = randomRotationCandidates[Random.Range(0, randomRotationCandidates.Count)];
                    if (Random.value <= difficulty.probability)
                        rotation.StartRandomRotation(GetRotationTime(rotation.gameObject));
                }

                state.randomRotation.lastTime = Time.time;
            }
        }

        public bool IsSlowdownOver()
        {
            return master.State.slowdown.speedScale >= 1;
        }


        private IEnumerator SpeedScaleCoroutine(
            float start,
            float end,
            float restore,
            float time,
            float stayTime,
            System.Action After = null,
            long coroutinesStarted = 0,
            float outTime = 1f
        )
        {

            float stage = 0;

            while (stage < 1)
            {
                stage += Time.deltaTime / time;
                master.State.slowdown.speedScale = Mathf.Lerp(start, end, stage);
                yield return null;
            }


            if(After != null)
                After();
            yield return new WaitForSeconds(stayTime);

            end = master.State.slowdown.speedScale;
            stage = 0;

            while (stage < 1 && master.State.coroutinesStarted == coroutinesStarted)
            {
                stage += Time.deltaTime / outTime;
                master.State.slowdown.speedScale = Mathf.Lerp(end, restore, stage);
                yield return null;
            }

            if(master.State.coroutinesStarted == coroutinesStarted)
            {
                master.State.slowdownType = Difficulty.Slowdown.Type.None;
            }
            
        }

        public void SlowDownShapesOnScreen(Difficulty.Slowdown slowdown, System.Action After = null)
        {
            float start = master.State.slowdown.speedScale;
            float end = slowdown.speedScale;
            float stayTime = slowdown.stayTime;
            float time = slowdown.inTime;
            master.State.slowdownType = slowdown.type;
            master.State.coroutinesStarted++;
            master.StartCoroutine(SpeedScaleCoroutine(start, end, 1,  time, stayTime, After, master.State.coroutinesStarted, outTime:slowdown.outTime));

        }

        public void BonusSlowdownShapesOnScreen(Difficulty.Bonus bonus)
        {
            if(bonus.slowdown!=null)
                SlowDownShapesOnScreen(bonus.slowdown);
        }

        public void BonusCatchSlowdown()
        {
            if (IsSlowdownOver())
            {
                SlowDownShapesOnScreen(master.State.Difficulty.bonusCatchSlowdown);
            }
        }

        public IEnumerator UnpauseCoroutine(float time)
        {
            while(Time.timeScale < 1 && !master.State.paused)
            {
                Time.timeScale = Mathf.Min(1, Time.timeScale + Time.unscaledDeltaTime / time);
                yield return null;
            };
        }

        public void StandardSlowDownShapesOnScreen()
        {
            SlowDownShapesOnScreen(master.State.Difficulty.slowdown);
        }


        public bool IsGameModeScoreCooldownEnded()
        {
            return master.State.score - master.State.modeChange.lastScore >= master.State.Difficulty.modeChange.scoreCooldown;
        }

        public bool IsGameModeTimeCooldownEnded()
        {
            float firstScoreTime = master.State.firstScoreTime;
            float time = Time.time - firstScoreTime;

            return firstScoreTime > 0 && time - master.State.modeChange.lastTime >= master.State.Difficulty.modeChange.timeCooldown;
        }

        public bool CanChangeMode(Difficulty.Mode d, State.Mode s)
        {
            bool can = false;
            float firstScoreTime = master.State.firstScoreTime;
            float time = Time.time - firstScoreTime;

            if (d.scoreBased && IsGameModeScoreCooldownEnded())
            {
                can = master.State.score - master.State.Difficulty.modeChange.startScore - s.lastChangeScore - master.State.Difficulty.modeChange.scoreCooldown >= d.scoreInterval;
            }
            else if(!d.scoreBased && IsGameModeTimeCooldownEnded())
            {
                can = firstScoreTime > 0 && time - s.lastChangeTime - master.State.Difficulty.modeChange.timeCooldown >= d.timeInterval;
            }

            if (can)
            {
                s.lastChangeTime = time;
                s.lastChangeScore = master.State.score;
                return can && Random.value < d.probability;
            }

            return false;
        }

        public void TryToChangeGameModeParameters()
        {
            if (!IsSlowdownOver()||GetMinCollisionTimeWithBasket(master.State.shapesOnScreen)>=master.State.PlayerReactionTime * master.State.Difficulty.modeChange.reactionTime)
            {
                return;
            }

            bool basketModeChange = CanChangeMode(master.State.Difficulty.basketMode, master.State.basketMode);
            bool basketColorsChange = CanChangeMode(master.State.Difficulty.basketColors, master.State.basketColors);
            bool basketShapesChange = CanChangeMode(master.State.Difficulty.basketShapes, master.State.basketShapes);

            if (!(basketColorsChange || basketModeChange || basketShapesChange))
            {
                return;
            }

            List<System.Action> modeChanges = new List<System.Action>();

            if(basketModeChange)
            {
                //Debug.Log("CHANGE BASKET MODE");
                modeChanges.Add(() => master.State.SwapBasketGameModes());
            }
            if (basketColorsChange)
            {
                //Debug.Log("CHANGE BASKET COLORS");
                modeChanges.Add(() => master.State.Mapping.field.shapeBasket.GetComponent<ColorsRandomizer>().StartSmoothColorRandomization());
            }
            if(basketShapesChange)
            {
                //Debug.Log("CHANGE BASKET SHAPES");
                modeChanges.Add(() => master.State.Mapping.field.shapeBasket.GetComponentInChildren<ShapeMixer>().Mix());
            }

            if (modeChanges.Count > 0)
            {
                StandardSlowDownShapesOnScreen();
                master.State.modeChange.lastScore = master.State.score;
                master.State.modeChange.lastTime = Time.time - master.State.firstScoreTime;
            }
            else
            {
                return;
            }

            while(modeChanges.Count>master.State.Difficulty.modeChange.maxAtOnce)
            {
                int index = Random.Range(0, modeChanges.Count);
                modeChanges.RemoveAt(index);
            }
            foreach (var action in modeChanges)
                action();
            
        }

        public int GetShapesOnScreenLimit()
        {
            var state = master.State;
            var difficulty = state.Difficulty.shapesOnScreen;
            int limit = 1;
            int shapesOnScreenLimit = master.State.shapesOnScreenLimit;
            int score = master.State.score;
           // Debug.Log("Score:" + score +"Curr:"+shapesOnScreenLimit +" Offs:"+ master.State.shapesOnScreenLimitOffset);
            if (shapesOnScreenLimit < difficulty.min)
            {
                //Debug.Log("up to min interval:" + difficulty.upToMinScoreInterval);
                limit =  score / difficulty.upToMinScoreInterval;
               // Debug.Log("Up to min limit:" + limit);
                return Mathf.Clamp(limit, 1, difficulty.min);
            }
            else
            {
                int scoreAfterMin = (score - difficulty.min * difficulty.upToMinScoreInterval);
                limit = difficulty.min + scoreAfterMin / difficulty.increaseScoreInterval;
                //Debug.Log("Normal limit:" + limit);
                return Mathf.Clamp(limit, difficulty.min, difficulty.max);
            }
        }

        public void SwitchSpawnPreset(int shapesOnScreen, bool align = true)
        {
            //Debug.LogFormat("Shapes on screen limit:{0}", shapesOnScreen);
            if (master.State.shapesOnScreenLimit != shapesOnScreen)
            {
                master.State.shapesOnScreenLimit = shapesOnScreen;
                master.State.shapesToSpawns = master.State.ActiveSpawnPreset.CreateShapesToSpawnsMapping();
                foreach (var shape in master.State.shapesOnScreen)
                {
                    var controller = shape.GetComponent<Shape.Controller>();
                    controller.Scale.SetSmoothScale(master.State.ActiveSpawnPreset.scale, 1f);
                    if (align)
                    {
                        var closestSpawn = FindShapeClosestSpawn(shape);
                        controller.Falling.AlignToSpawn(closestSpawn);
                        master.State.shapesToSpawns[closestSpawn].Add(shape);
                    }
                }
            }
        }

        public void IncreaseDifficultyState()
        {
            SwitchSpawnPreset(GetShapesOnScreenLimit());
            master.State.PlayerReactionTime += master.State.Difficulty.playerReactionTime.changePerScore;
        }

        public void ExplodeShapesOnScreenDelayed(float delay, float time = -1, bool particles = true)
        {
            foreach (var shape in master.State.shapesOnScreen)
            {
                var controller = shape.GetComponent<Shape.Controller>();
                if (!controller.Destruction.Started)
                    controller.Destruction.StartDestruction(delay:delay, time:time, particles:particles);
            }
        }

        public void ExplodeShapesOnScreenImmidiately()
        {
            ExplodeShapesOnScreenDelayed(0);
        }

        public void ExplodeShapesOnScreen()
        {
            Debug.Log("Slowdown:" + master.State.Difficulty.explosion.slowdown);
            if (master.State.Difficulty.explosion.slowdown != null)
            {
                SlowDownShapesOnScreen(master.State.Difficulty.explosion.slowdown, ExplodeShapesOnScreenImmidiately);
            }
            else
            {
                ExplodeShapesOnScreenImmidiately();
            }
        }

        public void UseExplosionBonus(bool immediately = false)
        {
            if (master.State.ExplosionBonuses > 0 || immediately)
            {
                Object.Instantiate(master.State.Mapping.particleEffect.explosion, master.transform).SetActive(true);
                BonusSlowdownShapesOnScreen(master.State.Difficulty.explosion);
                master.State.Score += master.State.shapesOnScreen.Count;
                ExplodeShapesOnScreen();
                if (!immediately)
                    master.State.ExplosionBonuses--;
            }

        }

        public void UseFreezeBonus(bool immediately = false)
        {
            if (master.State.FreezeBonuses > 0 || immediately)
            {
                Object.Instantiate(master.State.Mapping.particleEffect.freeze, master.transform).SetActive(true);
                BonusSlowdownShapesOnScreen(master.State.Difficulty.freeze);
                if(!immediately)
                    master.State.FreezeBonuses--;
            }
        }

        public bool UseHeartBonus()
        {
            if(master.State.HeartBonuses > 0)
            {
                master.State.HeartBonuses--;
                BonusSlowdownShapesOnScreen(master.State.Difficulty.heart);
                return true;
            }
            return false;
        }

        public void Pause()
        {
            if (!master.State.started)
            {
                master.State.paused = false;
                Time.timeScale = 1;
            }

            master.State.paused = !master.State.paused;

            if (master.State.paused)
            {
                master.Listeners.OnPause();
                Time.timeScale = 0;
            }
            else
            {
                master.StartCoroutine(UnpauseCoroutine(3));
            }
        }


        public bool ShouldUpdate()
        {
            return master.State.Started && !master.State.paused;
        }


    }



}

