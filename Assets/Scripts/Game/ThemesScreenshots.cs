using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Assets.Scripts.Game
{
    [CustomEditor(typeof(ThemesScreenshots))]
    public class ThemesScreenshotsEditor: Editor
    {
        private bool visible = false;
        private List<string> names = null;
        private string[] namesArray = null;
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
                names = null;
                return;
            }
            if (!ColorsPresets.Ready || !ColorsPresetsManager.Ready)
            {
                return;
            }
            if (names == null)
            {
                List<string> lnames = new List<string>(ColorsPresets.Instance.PresetCount);
                for (int i = 0; i < ColorsPresets.Instance.PresetCount; i++)
                {
                    lnames.Add(ColorsPresets.Instance[i].name);
                }
                names = lnames;
                namesArray = lnames.ToArray();
            }
          

            var screenshots = (ThemesScreenshots)target;
            var current = ColorsPresets.Instance.CurrentPreset;
            int index = EditorGUILayout.Popup(names.IndexOf(current.name), namesArray);
            ColorsPresets.Instance.PresetName = namesArray[index];
            if (current != ColorsPresets.Instance.CurrentPreset)
            {
                Debug.LogFormat("NAME:{0}", ColorsPresets.Instance.PresetName);
                ColorsPresetsManager.Instance.ApplyCurrentColorPreset();
            }


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
    }

    public class ThemesScreenshots : MonoBehaviour
    {
        [System.Serializable]
        public class Config
        {
            public string screenshotNameFormat = "{0}//screenshot.png";
            public string path = "Assets//Textures/UI//Themes";
            public Master master;
            public GameObject shapesOnScreen;
            public GameObject UI;
        }

        public Config config = new Config();

        public void PrepareFieldForScreenshot()
        {
            DifficultyLevels.Instance.LevelName = "Endless";
            config.master.State.Reset();
            config.master.State.Mapping.ShapeBasketHidden = false;
            config.master.State.Mapping.IndicatorHidden = false;
            config.master.State.Mapping.ScoreHidden = false;
            config.master.State.Mapping.TimerHidden = true;
            config.master.State.BasketGameMode = Indicator.GameMode.Shape;
            config.master.State.Score = 10;
            config.master.State.ExplosionBonuses = 13;
            config.master.State.FreezeBonuses = 9;
            config.master.State.HeartBonuses = 3;
            //config.shapesOnScreen.SetActive(true);
            config.UI.SetActive(false);
        }
    }
}
