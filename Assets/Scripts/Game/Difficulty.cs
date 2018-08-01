using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Shape;
using Assets.Scripts.Indicator;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Scripts.Game
{

    [System.Serializable]
    public partial class Difficulty : MonoBehaviour
    {


        public Target target = new Target();

        [HideInInspector]
        public Slowdown slowdown = new DefaultSlowdown();
        [HideInInspector]
        public Slowdown bonusCatchSlowdown = new BonusCatchSlowdown();

        public RandomRotation randomRotation = new RandomRotation();
        public Rotation rotation = new Rotation();

        public GameMode initialGameMode = GameMode.None;
        public ModeChange modeChange = new ModeChange();
        public Mode basketMode = new Mode();
        public Mode basketShapes = new Mode();
        public Mode basketColors = new Mode();

        public Bonus heart = new HeartBonus();
        public Bonus freeze = new FreezeBonus();
        public Bonus explosion = new ExplosionBonus();

        public ShapesOnScreen shapesOnScreen = new ShapesOnScreen();
        public ReactionTime playerReactionTime = new ReactionTime();

        [HideInInspector]
        public List<ShapeType> additionalShapes;

        public void UpdateData(Data data)
        {
            target = data.target.Copy();

            //slowdown = data.slowdown.Copy();
            //bonusCatchSlowdown = data.bonusCatchSlowdown.Copy();

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

                //slowdown = slowdown.Copy(),
                //bonusCatchSlowdown = bonusCatchSlowdown.Copy(),

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

        private bool ShouldShowBonusTutorial(Bonus bonus)
        {
            return bonus.max > 0 && bonus.number > 0 && bonus.probability > 0;
        }

        public bool ShouldShowExplosionBonusTutorial()
        {
            return ShouldShowBonusTutorial(explosion);
        }

        public bool ShouldShowFreezeBonusTutorial()
        {
            return ShouldShowBonusTutorial(freeze);
        }

        public bool ShouldShowLifeBonusTutorial()
        {
            return ShouldShowBonusTutorial(heart);
        }

    }
    
}
