using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Analytics;

namespace Assets.Scripts.Game
{
    public class Listeners
    {
        public Action OnPause = () => { };
        public Action<bool> OnGameOver = win => {
            PersistentState.Instance.data.timesPlayed++;
            if(win)
                PersistentState.Instance.data.gamesWon++;
            Events.GameOver();
        };
    }
}
