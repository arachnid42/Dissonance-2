using UnityEngine;
using System.Collections;
using Assets.Scripts.Game;

namespace Assets.Scripts.Shape
{
    public class Falling : MonoBehaviour
    {
        public float speed = 10f;
        private float spawnSwitchTime = 2f;
        public State.Slowdown slowdown;

        private Coroutine spawnSwitchCoroutine; 

        public void AlignToSpawn(GameObject spawn)
        {
            if (spawnSwitchCoroutine != null)
                StopCoroutine(spawnSwitchCoroutine);
            spawnSwitchCoroutine = StartCoroutine(SpawnSwitchCoroutine(spawn));
        }


        private IEnumerator SpawnSwitchCoroutine(GameObject spawn)
        {
            var end = spawn.transform.position;
            float stage = 0;
            while (stage < 1)
            {
                var start = transform.position;
                stage += Time.deltaTime / spawnSwitchTime;
                start.x = Mathf.Lerp(start.x, end.x, stage);
                transform.position = start;
                yield return null;
            }
            
        }

        public void Update()
        {
            transform.Translate(Vector3.down * speed * slowdown.speedScale * Time.deltaTime, Space.World);
        }

    }
}

