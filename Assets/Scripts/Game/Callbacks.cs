using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Shape;
using Assets.Scripts.ShapeBasket;

namespace Assets.Scripts.Game
{
    public class Callbacks
    {
        private Master master = null;
        public Callbacks(Master master)
        {
            this.master = master;
        }

        public void OnShapeCatch(bool match)
        {
            if (match)
            {
                master.State.Score++;
                master.Actions.IncreaseDifficultyState();
                if (master.State.score == 1)
                    master.State.firstScoreTime = Time.time;
            }
            else if(!master.Actions.UseHeartBonus())
            {
                master.State.SetGameOverData(false);
                master.Listeners.OnGameOver(false);
                master.Stop();
            }
        }

        public void OnDestroy(GameObject shape)
        {
            switch (shape.GetComponent<Shape.Controller>().RandomRotation.CurrentRotation.type)
            {
                case ShapeType.Explosion:
                    master.State.explosion.onScreen--;
                    break;
                case ShapeType.Heart:
                    master.State.heart.onScreen--;
                    break;
                case ShapeType.Snowflake:
                    master.State.freeze.onScreen--;
                    break;
            }
            master.State.RemoveShape(shape);
        }


        public void OnBonusTouch(ShapeType type)
        {
            switch (type)
            {
                case ShapeType.Explosion:
                    if (master.State.ScaledPlayerReactionTime <= Difficulty.BONUS_AUTO_USE_REACTION_TIME)
                    {
                        master.Actions.UseExplosionBonus(immediately:true);
                    }
                    else
                    {
                        master.State.ExplosionBonuses++;
                    }
                    break;
                case ShapeType.Heart:
                    master.State.HeartBonuses++;
                    break;
                case ShapeType.Snowflake:
                    if (master.State.ScaledPlayerReactionTime <= Difficulty.BONUS_AUTO_USE_REACTION_TIME)
                    {
                        master.Actions.UseFreezeBonus(immediately: true);
                    }
                    else
                    {
                        master.State.FreezeBonuses++;
                    }
                    break;
            }
            master.Actions.BonusCatchSlowdown();
        }

        
        public void SetShapeBasketCallbacks(GameObject shapeBasket)
        {
            ShapeCatcher shapeCatcher = shapeBasket.GetComponent<ShapeCatcher>();
            shapeCatcher.OnCatch = OnShapeCatch;
        }

        public void SetShapeCallbacks(GameObject shape)
        {

            Shape.Controller controller = shape.GetComponent<Shape.Controller>();

            if (controller.Bonus != null)
                controller.Bonus.OnTouch += OnBonusTouch;
            if (controller.Destruction != null)
                controller.Destruction.OnDestroy += OnDestroy;

        }
    }
}
