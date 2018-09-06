using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using System;
using Assets.Scripts.Game;
using System.Collections;

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
            Instance = this;
            var config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = debug;
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate(SignIn);
        }

        private IEnumerator Start()
        {
            while (!Field.Ready)
                yield return null;
            Field.Instance.Master.Listeners.OnGameOver += OnGameOver;
        }

        private void OnGameOver(bool win)
        {
            IncrementGameOverAchievements(DifficultyLevels.Instance.CurrentDifficulty, Field.Instance.Master.State.gameOver);
        }


        private void GetLeaderBoardScore(string id, Action<LeaderboardScoreData> callback)
        {
            PlayGamesPlatform.Instance.LoadScores(
                id,
                LeaderboardStart.PlayerCentered,
                1,
                LeaderboardCollection.Social,
                LeaderboardTimeSpan.AllTime,
                callback);
        }

        private IEnumerator RestoreEndlessRecors()
        {
            while (!PersistentState.Ready)
                yield return null;
            GetLeaderBoardScore(GPGSIds.leaderboard_endless_score, data=> {
                //Debug.Log("Is authentificated endless score:" + Social.localUser.authenticated);
                if (data.Valid)
                {
                    int newValue = (int)data.PlayerScore.value;
                    if (newValue > 0)
                    {
                        PersistentState.Instance.data.endlessModeUnlocked = true;
                    }
                    if (PersistentState.Instance.data.endlessScoreRecord <= newValue)
                    {
                        PersistentState.Instance.data.endlessScoreRecord = newValue;
                    }
                    else
                    {
                        UpdateEndlessScoreLeaderBoard(PersistentState.Instance.data.endlessScoreRecord);
                    }
                }
            });
            GetLeaderBoardScore(GPGSIds.leaderboard_endless_time, data =>
            {
                //Debug.Log("Is authentificated endless time:" + Social.localUser.authenticated);
                if (data.Valid)
                {
                    //Debug.Log("Time score date:" + data.PlayerScore.date);
                    //Debug.Log("Time leaderboard value:" + data.PlayerScore.value);
                    //Debug.Log("Time leaderboard fvalue:" + data.PlayerScore.formattedValue);
                    int newValue = Mathf.CeilToInt((float)data.PlayerScore.value / 1000);
                    if (newValue > 0)
                    {
                        PersistentState.Instance.data.endlessModeUnlocked = true;
                    }
                    if(PersistentState.Instance.data.endlessTimeRecord <= newValue)
                    {
                        PersistentState.Instance.data.endlessTimeRecord = newValue;
                    }
                    else
                    {
                        UpdateEndlessTimeLeaderBoard(PersistentState.Instance.data.endlessTimeRecord);
                    }
                }
            });
        }

        private IEnumerator RestoreLockedItemsUsingAchievements()
        {
            while (!PersistentState.Ready)
                yield return null;
            if (PlayGamesPlatform.Instance.GetAchievement(GPGSIds.achievement_reaction_master).IsUnlocked)
            {
                PersistentState.Instance.data.endlessModeUnlocked = true;
                PersistentState.Instance.data.customModeUnlocked = true;
                while (DifficultyLevels.Instance == null)
                    yield return null;
                PersistentState.Instance.data.levelsUnlocked = DifficultyLevels.Instance.LevelCount;
            }
            else
            {
                if (PlayGamesPlatform.Instance.GetAchievement(GPGSIds.achievement_configuration_master).IsUnlocked)
                {
                    PersistentState.Instance.data.customModeUnlocked = true;
                }
                if (PlayGamesPlatform.Instance.GetAchievement(GPGSIds.achievement_neverending_story).IsUnlocked)
                {
                    PersistentState.Instance.data.endlessModeUnlocked = true;
                }
            }
        }

        private void SignIn(bool success)
        {
            if (success)
            {
                Ready = true;
                DontDestroyOnLoad(gameObject);
                StartCoroutine(RestoreEndlessRecors());
                StartCoroutine(RestoreLockedItemsUsingAchievements());
            }
            else
            {
                Ready = false;
            }
        }

        public void OpenAchievements()
        {
            PlayGamesPlatform.Instance.ShowAchievementsUI();
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
