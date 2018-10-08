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
                TUTORIAL_COMPLETED = "tutorial_completed",
                PURCHASE_ATTEMPT = "purchase_attempt",
                PURCHASE_COMPLETED = "purchase_completed";
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
                SCORE_BASED = "score_based",
                PRODUCT_ID = "product_id";

        }

        public static class Panels
        {
            public const string
                DONATION = "donation",
                DONATION_MESSAGE = "donation_message",
                THEMES = "themes",
                CONFIGURABLE = "configurable",
                CONFIGURABLE_MENU = "configurable_menu",
                ENDLESS = "endless",
                MAIN_MENU = "main_menu",
                LEVELS = "levels";
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
            FirebaseAnalytics.LogEvent(CombineEventNameWithParameter(Names.PANEL_OPENED, name));
        }

        public static void PurchaseAttempt(string productId)
        {
            //Debug.Log("event name:" + CombineEventNameWithParameter(Names.PURCHASE_ATTEMPT, productId));
            FirebaseAnalytics.LogEvent(CombineEventNameWithParameter(Names.PURCHASE_ATTEMPT, productId));
        }

        public static void PurchaseCompleted(string productId)
        {
            FirebaseAnalytics.LogEvent(CombineEventNameWithParameter(Names.PURCHASE_COMPLETED, productId));
        }

        private static string CombineEventNameWithParameter(string eventName,string paramName)
        {
            return string.Format("{0}_{1}", eventName, paramName);
        }
    }
}
