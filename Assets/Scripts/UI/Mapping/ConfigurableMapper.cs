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
            Difficulty.Data configurableData = PersistentState.Instance.data.configurableModeData;
            startingModeMapper.SetData(configurableData.initialGameMode);
            shapesMapper.SetData(configurableData.additionalShapes);
            bonusSpoiler.SetData(configurableData.shapesOnScreen);
            rotationSpeedSpoiler.SetData(configurableData.rotation);
            randomRotationSpoiler.SetData(configurableData.randomRotation);
            reactionTimeSpoiler.SetData(configurableData.playerReactionTime);
            gameModeChangeSpoiler.SetData(configurableData.basketMode);
            gameColorsChangeSpoiler.SetData(configurableData.basketColors);
            gameShapesChangeSpoiler.SetData(configurableData.basketShapes);
            modesChangeSpoiler.SetData(configurableData.modeChange);
            lifeBonusSpoiler.SetData(configurableData.heart);
            freezeBonusSpoiler.SetData(configurableData.freeze);
            explosionBonusSpoiler.SetData(configurableData.explosion);
            targetMapper.SetData(configurableData.target);
        }

        public void MapToConfigurableData()
        {
            Difficulty.Data configurableData = PersistentState.Instance.data.configurableModeData;
            configurableData.initialGameMode = startingModeMapper.GetData();
            configurableData.additionalShapes = shapesMapper.GetData();
            configurableData.shapesOnScreen = bonusSpoiler.GetData();
            configurableData.rotation = rotationSpeedSpoiler.GetData();
            configurableData.randomRotation = randomRotationSpoiler.GetData();
            configurableData.playerReactionTime = reactionTimeSpoiler.GetData();
            configurableData.basketMode = gameModeChangeSpoiler.GetData();
            configurableData.basketColors = gameColorsChangeSpoiler.GetData();
            configurableData.basketShapes = gameShapesChangeSpoiler.GetData();
            configurableData.modeChange = modesChangeSpoiler.GetData();
            configurableData.heart = lifeBonusSpoiler.GetData();
            configurableData.freeze = freezeBonusSpoiler.GetData();
            configurableData.explosion = explosionBonusSpoiler.GetData();
            configurableData.target = targetMapper.GetData();
            PersistentState.Instance.data.configurableModeData = configurableData;
        }
    }
}
