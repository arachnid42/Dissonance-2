using System;
using Firebase.Analytics;
using Assets.Scripts.Game;
using UnityEngine;

namespace Assets.Scripts.Analytics
{
    public static class Events
    {
        public static class Names
        {
            public const string
                PANEL_OPENED = "panel_opened",
                GAME_OVER = "game_over",
                TUTORIAL_COMPLETED = "tutorial_completed";
        }

        public static class Parameters
        {
            public const string
                PANEL_NAME = "panel_name",
                WIN = "win",
                LEVEL_NAME = "level_name",
                TARGET_SCORE = "target_score",
                TARGET_TIME = "target_time",
                SCORE = "score",
                TIME = "time",
                SCORE_BASED = "score_based";

        }

        public static class Panels
        {
            public const string
                DONATION = "donation",
                DONATION_MESSAGE = "donation_message",
                THEMES = "themes",
                CONFIGURABLE = "configurable",
                ENDLESS = "endless";
        }

        public static void GameOver()
        {
            var state = Field.Instance.Master.State;
            var target = state.Difficulty.target;
            var gameOverData = state.gameOver;

            if (gameOverData.tutorial)
            {

                var parameters = new Parameter[]
                {
                    new Parameter(Parameters.TIME, state.stopTime - state.firstScoreTime)
                };

                FirebaseAnalytics.LogEvent(Names.TUTORIAL_COMPLETED, parameters);
                return;
            }
            else
            {
                var parameters = target.scoreBased ? new Parameter[]
                {
                    new Parameter(Parameters.SCORE_BASED,1),
                    new Parameter(Parameters.TARGET_SCORE, target.score),
                    new Parameter(Parameters.SCORE, gameOverData.score),
                    new Parameter(Parameters.LEVEL_NAME, state.Difficulty.name)
                } : new Parameter[]
                {
                    new Parameter(Parameters.SCORE_BASED,0),
                    new Parameter(Parameters.TARGET_TIME, target.time),
                    new Parameter(Parameters.TIME, gameOverData.time),
                    new Parameter(Parameters.LEVEL_NAME, state.Difficulty.name)
                };
                FirebaseAnalytics.LogEvent(Names.GAME_OVER, parameters);
            }


        }

        public static void PanelOpened(string name)
        {
            var parameters = new Parameter[]
            {
                new Parameter(Parameters.PANEL_NAME,name)
            };
            FirebaseAnalytics.LogEvent(Names.PANEL_OPENED, parameters);
        }



    }
}
