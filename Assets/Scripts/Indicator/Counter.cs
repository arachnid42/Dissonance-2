using UnityEngine;

namespace Assets.Scripts.Indicator
{
    public class Counter : MonoBehaviour
    {
        [SerializeField]
        private TextMesh numbers; 
        private int value = 0;
     
        public int Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                numbers.text = value.ToString();
            }
        }

        public void Start()
        {
            Value = 0;
        }

    }
}


