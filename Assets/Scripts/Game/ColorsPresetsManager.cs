using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

namespace Assets.Scripts.Game
{
    #if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(ColorsPresetsManager))]
    public class ColorsPresetsInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            string buttonText = !Application.isPlaying ? "UPDATE SCENE" : "PREVIEW";
            if (GUILayout.Button(buttonText))
            {
                ((ColorsPresetsManager)target).ApplyCurrentColorPreset();
            }
        }
    }
    #endif

    [ExecuteInEditMode]
    public class ColorsPresetsManager : MonoBehaviour
    {




        [Serializable]
        public class ColorsRecipients
        {
            public MeshRenderer background = null;
            public MeshRenderer bonusesBackground = null;
            public TextMesh[] texts;
            public TextMesh[] hoverTexts;
            public ParticleSystem explosionParticles;
            public ParticleSystem freeezeParticles;
        }

        [Serializable]
        public class MaterialOriginals
        {
            public Material solid;
            public Material texture;
        }
        [Serializable]
        public class MaterialsInstances
        {

            public List<Material> main = new List<Material>();
            public List<Material> basketMain = new List<Material>();
            public Material background;
            public Material bonusesBackground;
            public Material tileShapes;
            public Material freezeBonus;
            public Material explosionBonus;
            public Material heartBonus;

        }

        private static readonly int GEOMETRY_RENDER_QUEUE = 2002;
        private static readonly int FONT_RENDER_QUEUE = 2001;
        private static readonly int HOVER_FONT_RENDER_QUEUE = 2011;
        private static readonly int TEXTURE_RENDER_QUEUE = 2000;
        private static readonly int TRANSPARENT_RENDER_QUEUE = 2010;

        public static bool Ready
        {
            get { return instance != null && instance.IsReady; }
        }

        private static ColorsPresetsManager instance = null;
        public static ColorsPresetsManager Instance
        {
            get { return instance; }
        }

        [SerializeField]
        private bool recreateMaterials = false;
        [SerializeField]
        private MaterialOriginals originals;
        private MaterialsInstances materials = new MaterialsInstances();
        private bool isReady = false;
        private Coroutine applyCurrentColorPresetCoroutine;

        public bool IsReady
        {
            get { return isReady; }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                ApplyCurrentColorPreset();
            }else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void ResetBasketMaterialColors()
        {
            for (int i = 0; i < CurrentPreset.main.Length; i++)
                materials.basketMain[i].color = CurrentPreset.main[i];
        }

        public void ApplyCurrentColorPreset()
        {
            if(applyCurrentColorPresetCoroutine == null)
                applyCurrentColorPresetCoroutine = StartCoroutine(ApplyCurrentColorPresetCoroutine());
        }

        private IEnumerator ApplyCurrentColorPresetCoroutine()
        {
            while (ColorsPresets.Instance == null || !ColorsPresets.Instance.IsReady)
                yield return null;
            while (Field.Instance == null || Field.Instance.ColorsRecipients == null)
                yield return null;
            var colorsRecipients = Field.Instance.ColorsRecipients;
            for (int i = 0; i < CurrentPreset.main.Length; i++)
            {
                if (i < materials.main.Count)
                {
                    materials.basketMain[i] = UpdateMaterial(originals.solid, materials.basketMain[i], CurrentPreset.main[i], null, GEOMETRY_RENDER_QUEUE);
                    materials.main[i] = UpdateMaterial(originals.solid, materials.main[i], CurrentPreset.main[i], null, GEOMETRY_RENDER_QUEUE);
                }
                else
                {
                    var mat = UpdateMaterial(originals.solid, null, CurrentPreset.main[i], null, GEOMETRY_RENDER_QUEUE);
                    var basketMat = UpdateMaterial(originals.solid, null, CurrentPreset.main[i], null, GEOMETRY_RENDER_QUEUE);
                    materials.main.Add(mat);
                    materials.basketMain.Add(basketMat);
                }
            }

            materials.background = UpdateMaterial(originals.texture, materials.background, Color.white, CurrentPreset.background, TEXTURE_RENDER_QUEUE);
            materials.bonusesBackground = UpdateMaterial(originals.texture, materials.bonusesBackground, CurrentPreset.bonusCounterBackground, null ,TRANSPARENT_RENDER_QUEUE);

            materials.tileShapes = UpdateMaterial(originals.solid, materials.tileShapes, CurrentPreset.tileShapes, null, GEOMETRY_RENDER_QUEUE);

            materials.freezeBonus = UpdateMaterial(originals.solid, materials.freezeBonus, CurrentPreset.freezeBonus, null, GEOMETRY_RENDER_QUEUE);
            materials.heartBonus = UpdateMaterial(originals.solid, materials.heartBonus, CurrentPreset.heartBonus, null, GEOMETRY_RENDER_QUEUE);
            materials.explosionBonus = UpdateMaterial(originals.solid, materials.explosionBonus, CurrentPreset.explosionBonus, null, GEOMETRY_RENDER_QUEUE);

            foreach (var text in colorsRecipients.texts)
            {
                text.color = CurrentPreset.font;
                text.font.material.renderQueue = FONT_RENDER_QUEUE;
            }

            foreach(var text in colorsRecipients.hoverTexts)
            {
                text.color = CurrentPreset.font;
                text.font.material.renderQueue = HOVER_FONT_RENDER_QUEUE;
            }
                

            colorsRecipients.background.sharedMaterial = materials.background;
            colorsRecipients.bonusesBackground.sharedMaterial = materials.bonusesBackground;
            var main = colorsRecipients.freeezeParticles.main;
            main.startColor = CurrentPreset.snowflakeParticles;
            var colorOverLifetime = colorsRecipients.explosionParticles.colorOverLifetime;
            colorOverLifetime.color = new ParticleSystem.MinMaxGradient(CurrentPreset.explosionGradientOne, CurrentPreset.explosionGradientTwo);
            isReady = true;
            Resources.UnloadUnusedAssets();
            applyCurrentColorPresetCoroutine = null;
        }

        private Material UpdateMaterial(Material original, Material current, Color color, Texture texture, int renderQueue = 2000)
        {

            var mat = current == null || recreateMaterials? Instantiate(original) : current;
            if (recreateMaterials && current != null)
                DestroyImmediate(current, true);

            if (texture != null)
            {
                mat.mainTexture = texture;
            }
            else
            {
                mat.color = color;
            }
            mat.renderQueue = renderQueue;
            return mat;
        }

        public ColorsPreset CurrentPreset
        {
            get { return ColorsPresets.Instance.CurrentPreset; }
        }


        public MaterialsInstances Materials
        {
            get { return materials; }
        }

        private void OnDestroy()
        {
            DestroyImmediate(materials.background);
            DestroyImmediate(materials.bonusesBackground);
            DestroyImmediate(materials.explosionBonus);
            DestroyImmediate(materials.freezeBonus);
            DestroyImmediate(materials.heartBonus);
            DestroyImmediate(materials.tileShapes);
            foreach (var mat in materials.main)
                DestroyImmediate(mat);
            foreach (var mat in materials.basketMain)
                DestroyImmediate(mat);
        }
    }
}
