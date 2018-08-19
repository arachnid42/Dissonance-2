using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Assets.Scripts.Game
{
#if UNITY_EDITOR
    [CustomEditor(typeof(ThemesScreenshots))]
    public class ThemesScreenshotsEditor: Editor
    {
        private bool visible = false;
        private List<string> themes = null;
        private string[] themeNamesArray = null;
        private List<Indicator.GameMode> modes = null;
        private Indicator.GameMode gameMode = Indicator.GameMode.None;
        private string[] modeNamesArray = null;
        private List<string> levels = null;
        private string[] levelNamesArray = null;
        private bool shapeBasketHidden = false;
        private bool indicatorHidden = false;
        private bool scoreHidden = false;
        private bool timerHidden = true;
        private int score = 10;
        private int explosionBonuses = 13;
        private int freezeBonuses = 9;
        private int heartBonuses = 3;
        public override void OnInspectorGUI()
        {
            DrawCustomInspector();
            DrawDefaultInspector();
        }

        public void DrawCustomInspector()
        {
            if (!Application.isPlaying)
            {
                GUILayout.Label("ONLY WORKS IN PLAY MODE!");
                themes = null;
                modes = null;
                levels = null;
                return;
            }
            if (!ColorsPresets.Ready || !ColorsPresetsManager.Ready)
            {
                return;
            }


            var screenshots = (ThemesScreenshots)target;

            ThemesSelectionList();
            LevelsSelectionList();
            ModesSelectionList(screenshots);

            shapeBasketHidden = GUILayout.Toggle(shapeBasketHidden, "Shape basket hidden");
            screenshots.config.master.State.Mapping.ShapeBasketHidden = shapeBasketHidden;
            indicatorHidden = GUILayout.Toggle(indicatorHidden, "Indicator hidden");
            screenshots.config.master.State.Mapping.IndicatorHidden = indicatorHidden;
            scoreHidden = GUILayout.Toggle(scoreHidden, "Score hidden");
            screenshots.config.master.State.Mapping.ScoreHidden = scoreHidden;
            timerHidden = GUILayout.Toggle(timerHidden, "Timer hidden");
            screenshots.config.master.State.Mapping.TimerHidden = timerHidden;

            GUILayout.Label("Score");
            score = Mathf.FloorToInt(GUILayout.HorizontalSlider(score, 0,100));
            screenshots.config.master.State.Score = score;

            GUILayout.Label("Explosion Bonuses");
            explosionBonuses = Mathf.FloorToInt(GUILayout.HorizontalSlider(explosionBonuses, 0, 100));
            screenshots.config.master.State.ExplosionBonuses = explosionBonuses;

            GUILayout.Label("Freeze Bonuses");
            freezeBonuses = Mathf.FloorToInt(GUILayout.HorizontalSlider(freezeBonuses, 0, 100));
            screenshots.config.master.State.FreezeBonuses = freezeBonuses;

            GUILayout.Label("Heart Bonuses");
            heartBonuses = Mathf.FloorToInt(GUILayout.HorizontalSlider(heartBonuses, 0, 100));
            screenshots.config.master.State.HeartBonuses = heartBonuses;



            if (GUILayout.Button("MAKE SCREENSHOT"))
            {
                var name = string.Format(screenshots.config.screenshotNameFormat, ColorsPresets.Instance.CurrentPreset.name);
                var path = Path.Combine(screenshots.config.path, name);
                ScreenCapture.CaptureScreenshot(path);
            }
            if(GUILayout.Button("PREPARE FIELD"))
            {
                screenshots.PrepareFieldForScreenshot();
            }
        }

        private void ThemesSelectionList()
        {
            if (themes == null)
            {
                List<string> lnames = new List<string>(ColorsPresets.Instance.PresetCount);
                for (int i = 0; i < ColorsPresets.Instance.PresetCount; i++)
                {
                    lnames.Add(ColorsPresets.Instance[i].name);
                }
                themes = lnames;
                themeNamesArray = lnames.ToArray();
            }
            var current = ColorsPresets.Instance.CurrentPreset;
            int index = EditorGUILayout.Popup(themes.IndexOf(current.name), themeNamesArray);
            ColorsPresets.Instance.PresetName = themeNamesArray[index];
            if (current != ColorsPresets.Instance.CurrentPreset)
            {
                //Debug.LogFormat("NAME:{0}", ColorsPresets.Instance.PresetName);
                ColorsPresetsManager.Instance.ApplyCurrentColorPreset();
            }
        }

        private void LevelsSelectionList()
        {
            if (levels == null)
            {
                List<string> lnames = new List<string>(DifficultyLevels.Instance.transform.childCount);
                for (int i = 0; i < DifficultyLevels.Instance.transform.childCount; i++)
                {
                    //Debug.LogFormat("Adding: {0} level", DifficultyLevels.Instance.transform.GetChild(i).name);
                    lnames.Add(DifficultyLevels.Instance.transform.GetChild(i).name);
                }
                levels = lnames;
                levelNamesArray = lnames.ToArray();
            }
            var current = DifficultyLevels.Instance.CurrentDifficulty;
            int index = EditorGUILayout.Popup(levels.IndexOf(current.name), levelNamesArray);
            //Debug.LogFormat("NAME:{0} INDEX:{1}", current.name, index);
            //DifficultyLevels.Instance.LevelName = levelNamesArray[index];
            if (current.name != levelNamesArray[index])
            {
                //Debug.LogFormat("NAME:{0}", ColorsPresets.Instance.PresetName);
                //ColorsPresetsManager.Instance.ApplyCurrentColorPreset();
                DifficultyLevels.Instance.LevelName = levelNamesArray[index];

            }
        }

        private void ModesSelectionList(ThemesScreenshots screenshots)
        {
            if (modes == null)
            {
                modes = new List<Indicator.GameMode>(3);
                modes.Add(Indicator.GameMode.None);
                modes.Add(Indicator.GameMode.Shape);
                modes.Add(Indicator.GameMode.Color);
                modeNamesArray = new string[] { "None", "Shape", "Color" };
            }
            int index = EditorGUILayout.Popup(modes.IndexOf(gameMode), modeNamesArray);
            gameMode = modes[index];
            Debug.LogFormat("Applying game mode:{0} Index: {1}", gameMode,index);

            screenshots.config.master.State.BasketGameMode = gameMode;

        }
    }
#endif
    public class ThemesScreenshots : MonoBehaviour
    {
        [System.Serializable]
        public class Config
        {
            public string screenshotNameFormat = "{0}//screenshot.png";
            public string path = "Assets//Textures/UI//Themes";
            [HideInInspector]
            public Master master;
            public GameObject shapesOnScreen;
            public GameObject UI;
        }

        public Config config = new Config();

        private IEnumerator Start()
        {
            while(Field.Instance == null || Field.Instance.Master == null)
            {
                yield return null;
            }
            config.master = Field.Instance.Master;
        }

        public void PrepareFieldForScreenshot()
        {
            //DifficultyLevels.Instance.LevelName = "Endless";
            config.master.State.Reset();
            Time.timeScale = 0;
            config.master.State.Mapping.ShapeBasketHidden = false;
            config.master.State.Mapping.IndicatorHidden = false;
            config.master.State.Mapping.ScoreHidden = false;
            config.master.State.Mapping.TimerHidden = true;
            //config.master.State.BasketGameMode = Indicator.GameMode.Shape;
            config.master.State.Score = 10;
            config.master.State.ExplosionBonuses = 13;
            config.master.State.FreezeBonuses = 9;
            config.master.State.HeartBonuses = 3;
            config.shapesOnScreen.SetActive(true);
            config.UI.SetActive(false);
        }
    }
}
