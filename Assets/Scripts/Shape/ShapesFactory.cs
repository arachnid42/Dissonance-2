using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Shape
{
#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(ShapesFactory))]
    public class ShapeFactoryEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            ShapesFactory factory = (ShapesFactory)target;
            if(GUILayout.Button("CREATE SHAPES"))
            {
                factory.Build();    
            }
            if(GUILayout.Button("DELETE SHAPES"))
            {
                factory.Delete();
            }
            if (factory.created.Count > 0)
            {
                if(GUILayout.Button("RESET MESH ROTATION"))
                {
                    factory.ResetMeshLocalRotation();
                }
                if(factory.testingMaterials && GUILayout.Button("SET DEFAULT MATERIAL"))
                {
                    factory.testingMaterials = false;
                    factory.SetMaterials();
                }
                else if(!factory.testingMaterials && GUILayout.Button("SET TESTING MATERIALS"))
                {
                    factory.testingMaterials = true;
                    factory.SetMaterials();
                }
            }
        }
    }
#endif

    public class ShapesFactory : MonoBehaviour
    {
        private System.Type[] containerComponents =
        {
            typeof(RandomRotation),
            typeof(Rotation),
            typeof(Destruction),
            typeof(ColorPicker),
            typeof(Colors),
            typeof(Falling),
            typeof(Scale),
            typeof(Controller)
        };

        public List<GameObject> created = new List<GameObject>();
        public List<Color> colors = new List<Color>();
        public List<Material> testMaterials = new List<Material>();

        public bool testingMaterials = false;
        public string shapeTag = "Shape";
        public string shapeMeshTag = "ShapeMesh";
        public LayerMask shapeMeshLayer;
        public LayerMask shapeLayer;
        public Material material;
        public GameObject shapes;

        public void UpdateTestMaterials()
        {
            if (colors.Count != testMaterials.Count)
            {
                foreach (var mat in testMaterials)
                    DestroyImmediate(mat);
                testMaterials.Clear();
                foreach (var color in colors)
                    testMaterials.Add(Instantiate(material));
            }

            for(int i = 0; i < testMaterials.Count; i++)
            {
                testMaterials[i].color = colors[i];
            }
        }


        public void Build()
        {
            Delete();
            UpdateTestMaterials();
            foreach (Transform shape in shapes.transform)
            {
                created.Add(CreateShapeObject(shape.gameObject));
            }
        }

        public void Delete()
        {
            foreach(var shape in created)
            {
                DestroyImmediate(shape);
            }
            created.Clear();
        }

        public void ResetMeshLocalRotation()
        {
            foreach (var shape in created)
                shape.GetComponentInChildren<MeshRenderer>().transform.localRotation = Quaternion.identity;
        }

        public void SetMaterials()
        {
            UpdateTestMaterials();
            List<Material> sharedMaterials = new List<Material>();
            foreach (var shape in created)
            {
                
                var mr = shape.GetComponentInChildren<MeshRenderer>();
                var mf = shape.GetComponentInChildren<MeshFilter>();
                if (testingMaterials)
                {
                    sharedMaterials.AddRange(testMaterials.Take(mf.sharedMesh.subMeshCount));
                }
                else
                {
                    sharedMaterials.AddRange(Enumerable.Repeat(material, mf.sharedMesh.subMeshCount)); 
                }
                mr.sharedMaterials = sharedMaterials.ToArray();
                sharedMaterials.Clear();
            }
        }

        public GameObject CreateShapeObject(GameObject original)
        {
            
            var container = new GameObject(original.name + "-Shape", containerComponents);
            var shape = new GameObject("Shape", typeof(SphereCollider));
            var mesh = Instantiate(original, Vector3.zero, original.transform.rotation);
            mesh.transform.localScale = original.transform.lossyScale;
            mesh.name = "Mesh";
            mesh.AddComponent<MeshCollider>();

            var mf = mesh.GetComponent<MeshFilter>();
            var mr = mesh.GetComponent<MeshRenderer>();
            var materials = new Material[mf.sharedMesh.subMeshCount];
            for (int i = 0; i < materials.Length; i++)
                materials[i] = material;
            mr.sharedMaterials = materials;

            container.transform.parent = transform;
            shape.transform.parent = container.transform;
            mesh.transform.parent = shape.transform;

            var colorPicker = container.GetComponent<ColorPicker>();
            colorPicker.radius = mr.bounds.extents.magnitude;
            colorPicker.distance = colorPicker.radius * 2;

            mesh.layer = shapeMeshLayer;
            shape.layer = shapeLayer;

            mesh.tag = shapeMeshTag;
            shape.tag = shapeTag;

            return container;
        }
    }
}
