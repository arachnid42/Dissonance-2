using UnityEngine;
using System.Collections;
using Assets.Scripts.Game;

namespace Assets.Scripts.Shape
{
    public class Rotation : MonoBehaviour
    {
        public float speed = 60f;
        public State.Slowdown slowdown;
        // Update is called once per frame
        public void Update()
        {
            transform.Rotate(0, 0, speed * slowdown.speedScale * Time.deltaTime, Space.World);
        }

    }
}


