using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Indicator
{
    public class HideElement : MonoBehaviour
    {
        [SerializeField]
        private bool hidden = false;
        [SerializeField]
        private Vector3 visibleScale = new Vector3(1, 1, 1);
        [SerializeField]
        private Vector3 hiddenScale = new Vector3(0, 0, 1);

        public bool Hidden
        {
            get { return hidden; }
            set
            {
                if (value)
                {
                    transform.localScale = hiddenScale;
                }
                else
                {
                    transform.localScale = visibleScale;
                }
                    
                hidden = value;
            }
        }

        public void Start()
        {
            Hidden = hidden;
        }
    }
}
