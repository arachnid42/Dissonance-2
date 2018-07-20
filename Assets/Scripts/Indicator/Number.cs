using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Assets.Scripts.Indicator
{
    public class Number : MonoBehaviour
    {
        public GameObject[] numberModels;
        private GameObject[] previoslyActivated = null;
        private int value = -1;
        private GameObject numberGameObject = null;


        public int Value
        {
            get
            {
                return value;
            }
            set
            {
                if (previoslyActivated == null)
                {
                    previoslyActivated = new GameObject[numberModels.Length];
                }
                if (this.value != value || numberGameObject == null)
                {
                    if(numberGameObject != null)
                    {
                        numberGameObject.SetActive(false);
                        previoslyActivated[this.value] = numberGameObject;
                    }
                    this.value = Mathf.Clamp(value, 0, 9);
                    if (previoslyActivated[this.value] == null)
                    {
                        numberGameObject = Object.Instantiate(numberModels[this.value], transform);
                    }
                    else
                    {
                        numberGameObject = previoslyActivated[this.value];
                        numberGameObject.SetActive(true);
                    }
                }
               
            }
        }
    }

}
