using UnityEngine;
using UnityEditor;
using Assets.Scripts.Game;


namespace Assets.Scripts.UI.Mapping
{
    public class ConfigurableMapper : MonoBehaviour
    {
        [SerializeField]
        private StartingModeMapper startingModeMapper;
        [SerializeField]
        private ShapesMapper shapesMapper;
        [SerializeField]
        private ShapesOnScreenSpoiler bonusSpoiler;
        [SerializeField]
        private RotationSpeedSpolier rotationSpeedSpoiler;
        [SerializeField]
        private RandomRotationSpoiler randomRotationSpoiler;
        [SerializeField]
        private ReactionTimeSpoiler reactionTimeSpoiler;
        [SerializeField]
        private GameModeChangeSpoiler gameModeChangeSpoiler, gameColorsChangeSpoiler, gameShapesChangeSpoiler;
        [SerializeField]
        private ModesChangeSpoiler modesChangeSpoiler;
        [SerializeField]
        private BonusSpoiler lifeBonusSpoiler, freezeBonusSpoiler, explosionBonusSpoiler;
        [SerializeField]
        private TargetMapper targetMapper;

        public void MapFromConfigurableData()
        {
            Difficulty.Data data = PersistentState.Instance.data.configurableModeData;

            startingModeMapper.SetData(data.initialGameMode);
            shapesMapper.SetData(data.additionalShapes);
            bonusSpoiler.SetData(data.shapesOnScreen);
            rotationSpeedSpoiler.SetData(data.rotation);
            randomRotationSpoiler.SetData(data.randomRotation);
            reactionTimeSpoiler.SetData(data.playerReactionTime);
            gameModeChangeSpoiler.SetData(data.basketMode);
            gameColorsChangeSpoiler.SetData(data.basketColors);
            gameShapesChangeSpoiler.SetData(data.basketShapes);
            modesChangeSpoiler.SetData(data.modeChange);
            lifeBonusSpoiler.SetData(data.heart);
            freezeBonusSpoiler.SetData(data.freeze);
            explosionBonusSpoiler.SetData(data.explosion);
            targetMapper.SetData(data.target);
        }

        public void MapToConfigurableData()
        {

            Difficulty.Data data = PersistentState.Instance.data.configurableModeData;
            Difficulty.Data odata = PersistentState.Instance.temp.configurableModeOriginalData;

            data.initialGameMode = startingModeMapper.GetData();
            data.additionalShapes = shapesMapper.GetData();
            data.shapesOnScreen = bonusSpoiler.GetData();
            data.rotation = rotationSpeedSpoiler.GetData();

            data.randomRotation = randomRotationSpoiler.GetData();
            data.randomRotation.reactionTime = odata.randomRotation.reactionTime;
            data.randomRotation.rotationReactionTime = odata.randomRotation.rotationReactionTime;

            data.playerReactionTime = reactionTimeSpoiler.GetData();
            data.basketMode = gameModeChangeSpoiler.GetData();
            data.basketColors = gameColorsChangeSpoiler.GetData();
            data.basketShapes = gameShapesChangeSpoiler.GetData();
            data.modeChange = modesChangeSpoiler.GetData();

            data.heart = lifeBonusSpoiler.GetData();
            data.heart.slowdown = odata.slowdown.Copy();

            data.freeze = freezeBonusSpoiler.GetData();
            data.freeze.slowdown = odata.freeze.slowdown.Copy();

            data.explosion = explosionBonusSpoiler.GetData();
            data.explosion.slowdown = odata.explosion.slowdown.Copy();

            data.target = targetMapper.GetData();
            PersistentState.Instance.data.configurableModeData = data;
        }
    }
}
