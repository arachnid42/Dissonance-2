using System;
using UnityEngine;

namespace Assets.Scripts.Game
{    public class DifficultyLevels : MonoBehaviour
    {
        [SerializeField]
        private string levelName = "Test";
        public int hiddenLevelsEndOffset = 2;
        private int levelIndex = 0;
        private Difficulty difficulty;
        private static DifficultyLevels instance = null;

        public static DifficultyLevels Instance
        {
            get { return instance; }
        }

        private Difficulty GetDifficulty(string name)
        {
            return transform.Find(name).GetComponent<Difficulty>();
        }

        public Difficulty CurrentDifficulty
        {
            get
            {
                if (difficulty == null)
                    difficulty = GetDifficulty(levelName);
                return difficulty;
            }
        }

        public string LevelName
        {
            get { return levelName; }
            set
            {
                difficulty = GetDifficulty(value);
                levelName = value;
            }
        }


        public int LevelIndex
        {
            get { return levelIndex; }
            set
            {
                int index = Mathf.Clamp(value, 0, LevelCount - 1);
                levelIndex = index;
                var difficulty = transform.GetChild(index);
                this.difficulty = difficulty.GetComponent<Difficulty>();
                levelName = difficulty.name;
            }
        }


        public int LevelCount
        {
            get
            {
                int count = transform.childCount - hiddenLevelsEndOffset;
                return count < 0 ? 0 : count;
            }
        }


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                //DontDestroyOnLoad(gameObject);
            }else if(instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
