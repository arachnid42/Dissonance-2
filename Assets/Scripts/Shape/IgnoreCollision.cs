using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Shape
{
    class IgnoreCollision : MonoBehaviour
    {
        public void Start()
        {
            SphereCollider mainCollider = GetComponentInChildren<SphereCollider>();
            MeshCollider meshCollider = GetComponentInChildren<MeshCollider>();
            Physics.IgnoreCollision(mainCollider, meshCollider);
        }
    }
}
