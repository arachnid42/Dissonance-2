using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Game
{
    public class SpawnsPreset : MonoBehaviour
    {
        public GameObject[] spawns;
        public Vector3 scale;

        public void OnDrawGizmos()
        {
            foreach(GameObject spawn in spawns)
            {
                Gizmos.DrawWireCube(spawn.transform.position, spawn.transform.localScale);
            }   
        }

        public Dictionary<GameObject, List<GameObject>> CreateShapesToSpawnsMapping()
        {
            Dictionary<GameObject, List<GameObject>> mapping = new Dictionary<GameObject, List<GameObject>>();
            foreach(GameObject spawn in spawns)
            {
                mapping.Add(spawn, new List<GameObject>());
            }
            return mapping;
        }
    }
}
