using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Game;

namespace Assets.Scripts.Shape
{
    public enum ShapeType
    {
        None = 0,
        Circle = 1,
        Square = 2,
        Triangle = 3,
        Diamond = 4,
        Pentagon = 8,
        Hexagon = 9,
        Pentagram = 10,
        Hexagram = 11,
        Octogram = 12,
        Decagram = 13,
        // bonuses
        Heart = 5,
        Snowflake = 6,
        Explosion = 7
    }

    [Serializable]
    public class RotationDescriptor
    {
        private static Dictionary<ShapeType, Vector3> scales = new Dictionary<ShapeType, Vector3>()
        {
            {ShapeType.Triangle, Vector3.one},
            {ShapeType.Square, Vector3.one*0.8f},
            {ShapeType.Diamond, Vector3.one*1.15f},
            {ShapeType.Circle, Vector3.one*0.95f},
            {ShapeType.Pentagon, Vector3.one},
            {ShapeType.Pentagram, Vector3.one},
            {ShapeType.Hexagon, Vector3.one},
            {ShapeType.Hexagram, Vector3.one},
            {ShapeType.Octogram, Vector3.one},
            {ShapeType.Decagram, Vector3.one},
            {ShapeType.Snowflake, Vector3.one},
            {ShapeType.Explosion, Vector3.one},
            {ShapeType.Heart, Vector3.one }
        };

        [SerializeField]
        public static readonly ShapeType[] basicTypes = new ShapeType[]
        {
            ShapeType.Triangle,
            ShapeType.Square
        };

        public Vector3 rotation = Vector3.zero;
        public ShapeType type = ShapeType.None;
        public Vector3 Scale
        {
            get { return scales[type]; }
        }

        public bool IsBasic
        {
            get { return basicTypes.Contains(type); }
        }
    }

    #if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(RandomRotation))]
    public class RandomRotationEditor: UnityEditor.Editor
    {
        private int index = 0;
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (!Application.isPlaying)
            {
                
                RandomRotation rotator = (RandomRotation)target;
                if(targets.Length == 1)
                {
                    if (index >= 0)
                    {
                        var descriptor = rotator.Rotations[index];
                        rotator.transform.rotation = Quaternion.identity;
                        rotator.transform.localScale = Vector3.one;
                        rotator.ChildTransform.localRotation = Quaternion.Euler(descriptor.rotation);
                        rotator.ChildTransform.localScale = descriptor.Scale;
                        GUILayout.Label(string.Format("Type:{0} Index:{1}, Rotation:{2}", descriptor.type.ToString(), index, descriptor.rotation));
                    }
                    if (index + 1 < rotator.Rotations.Length && GUILayout.Button("Next"))
                    {
                        if (!rotator.gameObject.activeSelf)
                            rotator.gameObject.SetActive(true);
                        index++;
                        Camera.main.Render();
                    }
                    if (index - 1 >= 0 && GUILayout.Button("Prev"))
                    {
                        index--;
                        Camera.main.Render();
                    }
                }

                
                if (GUILayout.Button("RESET"))
                {
                    index = -1;
                    rotator.transform.rotation = Quaternion.identity;
                    rotator.transform.localScale = Vector3.one;
                    rotator.ChildTransform.rotation = Quaternion.identity;
                    rotator.ChildTransform.localScale = Vector3.one;
                    rotator.gameObject.SetActive(false);
                    Camera.main.Render();
                }

            }
        }

    }
    #endif


    public class RandomRotation : MonoBehaviour
    {

        static readonly float ANGLE_DELTA = 1f;

        public RotationDescriptor[] randomRotations = new RotationDescriptor[0];
        public State.Slowdown slowdown = null; 
        private RotationDescriptor currentRotation = null;
        private Coroutine rotationCoroutine = null;

        public RotationDescriptor[] Rotations
        {
            get { return randomRotations; }
        }

        public bool Started
        {
            get
            {
                return rotationCoroutine!=null || IsStatic;
            }
        }

        public RotationDescriptor CurrentRotation
        {
            get
            {
                return currentRotation;
            }
        }

        public bool IsStatic
        {
            get { return randomRotations.Length == 1; }
        }

        public void StartRandomRotation(float time = 1, float stage = 0)
        {
            if (rotationCoroutine != null)
                StopCoroutine(rotationCoroutine);
            rotationCoroutine = StartCoroutine(RotationCoroutine(stage: stage, time: time));
        }

        public Transform ChildTransform
        {
            get
            {
                return transform.GetChild(0);
            }
        }

        private IEnumerator RotationCoroutine(float stage = 0, float time = 1)
        {
            currentRotation = GetRandomRotation();
            Quaternion start = ChildTransform.localRotation, end = Quaternion.Euler(currentRotation.rotation);
            Vector3 scaleStart = ChildTransform.localScale, scaleEnd = currentRotation.Scale;
            //Debug.LogFormat("{0}:{1}", name, scaleEnd);
            for (; stage <= 1; stage += (Time.deltaTime / time * slowdown.speedScale))
            {
                ChildTransform.localRotation = InterpolateRotation(start, end, stage);
                ChildTransform.localScale = InterpolateScale(scaleStart, scaleEnd, stage);
                yield return null;
            }
            ChildTransform.localRotation = end;
            rotationCoroutine = null;
        }

        private void Awake()
        {
            if(randomRotations.Length < 2 && randomRotations.Length != 1)
            {
                throw new Exception("Rotations array should contain more than 1 or more than 2 elements");
            }
            StartRandomRotation(stage: 1);
        }

        private Quaternion InterpolateRotation(Quaternion start, Quaternion end, float stage)
        {
            return Quaternion.Slerp(start, end, stage);
        }

        private Vector3 InterpolateScale(Vector3 start, Vector3 end, float stage)
        {
            return Vector3.Slerp(start, end, stage);
        }

        private bool IsRotationAroundWorldForward(Quaternion start, Quaternion end)
        {

            Quaternion fromTo = InterpolateRotation(start, end, 0.5f);
            Vector3 de = (fromTo.eulerAngles - end.eulerAngles).normalized;
            Vector3 ds = (fromTo.eulerAngles - start.eulerAngles).normalized;
            Vector3 d = Vector3.zero;

            d.x = Mathf.CeilToInt(Mathf.Abs(de.x * ds.x));
            d.y = Mathf.CeilToInt(Mathf.Abs(de.y * ds.y));
            d.z = Mathf.CeilToInt(Mathf.Abs(de.z * ds.z));

            bool x, y, z, singleAxisRotation;
            Vector3 probableForward = Vector3.zero, startAxis, endAxis = Vector3.zero;
            x = d.x != 0;
            y = d.y != 0;
            z = d.z != 0;
            singleAxisRotation = d.x + d.y + d.z == 1;
            startAxis = start * Vector3.forward;
            endAxis = end * Vector3.forward;
            if (singleAxisRotation)
            {
                if (x)
                {
                    probableForward = Vector3.right;
                }
                else if (y)
                {
                    probableForward = Vector3.up;
                }
                else if (z)
                {
                    probableForward = Vector3.forward;
                }
                return probableForward == startAxis || -probableForward == startAxis || probableForward == endAxis || -probableForward == endAxis;
            }
            else
            {
                return startAxis == endAxis || startAxis == -endAxis;
            }
        }


        private RotationDescriptor GetRandomRotation()
        {
            if (randomRotations.Length == 1)
                return randomRotations[0];

            Vector3 rotation = Vector3.zero;
            RotationDescriptor descriptor = new RotationDescriptor();
            float angle = 0f;
            bool rotationAroundWorldForward = false;
            do
            {
                if (randomRotations.Length > 0)
                {
                    descriptor = randomRotations[UnityEngine.Random.Range(0, randomRotations.Length)];
                    rotation = descriptor.rotation;
                }
                else
                {
                    rotation = Vector3.zero;
                }
                Quaternion end = Quaternion.Euler(rotation);
                Quaternion start = ChildTransform.localRotation;
                rotationAroundWorldForward = IsRotationAroundWorldForward(start, end);
                angle = Quaternion.Angle(ChildTransform.localRotation, Quaternion.Euler(rotation));

            } while (angle < ANGLE_DELTA || rotationAroundWorldForward);

            return descriptor;
        }

    }

}
