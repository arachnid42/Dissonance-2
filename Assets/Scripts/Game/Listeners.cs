using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class Listeners
    {
        public Action OnPause = () => { };
        public Action<bool> OnGameOver = win => { PersistentState.Instance.data.timesPlayed++; };
    }
}
