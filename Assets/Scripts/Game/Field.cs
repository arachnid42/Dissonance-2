using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class Field : MonoBehaviour
    {
        [SerializeField]
        private Master master;

        public Master Master
        {
            get { return master; }
        }

        public static Field Instance
        {
            get;private set;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                //DontDestroyOnLoad(gameObject);
                Instance = this;
            }else if(Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }


}
