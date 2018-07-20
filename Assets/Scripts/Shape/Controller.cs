using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Shape
{
    public class Controller : MonoBehaviour
    {
        public Rotation Rotation
        {
            get { return GetComponent<Rotation>(); }
        }
        public Destruction Destruction
        {
            get { return GetComponent<Destruction>(); }
        }
        public RandomRotation RandomRotation
        {
            get { return GetComponent<RandomRotation>(); }
        }
        public Falling Falling
        {
            get { return GetComponent<Falling>(); }
        }
        public Scale Scale
        {
            get { return GetComponent<Scale>(); }
        }
        public Bonus Bonus
        {
            get { return GetComponent<Bonus>(); }
        }
    }
}
