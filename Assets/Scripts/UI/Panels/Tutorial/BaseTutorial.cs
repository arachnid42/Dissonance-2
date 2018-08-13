using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;

namespace Assets.Scripts.UI.Panels.Tutorial
{
    public abstract class BaseTutorial : BasePanel
    {
        public abstract void Next(BasePanel current, BasePanel next);

        private void OnDisable()
        {
            // Do not remove
        }
    }
}

