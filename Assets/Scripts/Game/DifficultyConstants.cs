using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Game
{
    public partial class Difficulty
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

    }
}
