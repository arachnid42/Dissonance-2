using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using System;
using Assets.Scripts.Game;

namespace Assets.Scripts.PlayServices
{
    public class GPServices : MonoBehaviour
    {

        public Action<bool> onEndlessTimeUpdated = success => Debug.Log("OnEndlessTimeUpdated:"+success);
        public Action<bool> onEndlessScoreUpdated = success => Debug.Log("OnEndlessScoreUpdated:" + success);

        public static GPServices Instance
        {
            get; private set;
        }

        public static bool Ready
        {
            get; private set;
        }

        public bool debug = true;
        private void Awake()
        {
            var config = new PlayGamesClientConfiguration.Builder()
                .RequestEmail()
                .Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = debug;
            PlayGamesPlatform.Activate();
            if (!Social.localUser.authenticated)
            {
                Social.localUser.Authenticate(SignIn);
            }
            else
            {
                SignIn(true);
            }
            Instance = this;
        }

        private void SignIn(bool success)
        {
            if (success)
            {
                Debug.Log("Successfuly signed in");
                Ready = true;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.Log("Sign in failed");
                Ready = false;
            }
        }

        public void OpenEndlessTimeLeaderBoard()
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_endless_time);
        }

        public void OpenEndlessScoreRecord()
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_endless_score);
        }

        public void UpdateEndlessTimeLeaderBoard(int seconds)
        {
            Social.ReportScore(seconds*1000, GPGSIds.leaderboard_endless_time,onEndlessTimeUpdated);
        }

        public void UpdateEndlessScoreLeaderBoard(int score)
        {
            Social.ReportScore(score, GPGSIds.leaderboard_endless_score, onEndlessScoreUpdated);
        }

        public void IncrementFreezeBonusAchievements()
        {
            UnlockAchievement(GPGSIds.achievement_its_too_cold_here);
            IncrementAchievement(GPGSIds.achievement_mr_freese);
        }

        public void IncrementExplosionBonusAchievements()
        {
            UnlockAchievement(GPGSIds.achievement_blow_them_all_up);
            IncrementAchievement(GPGSIds.achievement_demolition_man);
        }

        public void IncrementHeartBonusAchievements()
        {
            UnlockAchievement(GPGSIds.achievement_shape_of_my_heart);
            IncrementAchievement(GPGSIds.achievement_cupid);
        }

        private void IncrementAchievement(string id, int step = 1, Action<bool> callback = null)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(id, step, callback);
        }

        private void UnlockAchievement(string id,float progress = 100f,Action<bool> callback = null)
        {
            Social.ReportProgress(id, progress, callback);
        }

        public void IncrementGameOverAchievements(Difficulty level, State.GameOver data)
        {
            if (level.name == "Endless")
            {
                UnlockAchievement(GPGSIds.achievement_neverending_story);
            }
            else if(level.name == "Configurable")
            {
                UnlockAchievement(GPGSIds.achievement_configuration_master);
            }
            if (data.win)
            {
                UnlockAchievement(GPGSIds.achievement_on_the_way_to_perfection);
                if(level.name == DifficultyLevels.Instance.transform.GetChild(DifficultyLevels.Instance.LevelCount - 1).name)
                {
                    UnlockAchievement(GPGSIds.achievement_reaction_master);
                }
            }
            else
            {
                if (data.mode == Indicator.GameMode.Shape)
                {
                    IncrementAchievement(GPGSIds.achievement_oh_these_shapes);
                }
                else
                {
                    IncrementAchievement(GPGSIds.achievement_oh_these_colors);
                }
                IncrementAchievement(GPGSIds.achievement_this_only_makes_you_stronger);
                IncrementAchievement(GPGSIds.achievement_never_give_up);
                IncrementAchievement(GPGSIds.achievement_unstoppable);
            }
            IncrementAchievement(GPGSIds.achievement_reaction_veteran);
            IncrementAchievement(GPGSIds.achievement_reaction_hero);
            IncrementAchievement(GPGSIds.achievement_reaction_legend);
        }

        public void IncrementThemeSwitchAchievements()
        {
            UnlockAchievement(GPGSIds.achievement_what_about_other_theme);
        }

    }
}
