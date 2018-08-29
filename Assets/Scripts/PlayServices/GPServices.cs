using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using System;

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
    }
}
