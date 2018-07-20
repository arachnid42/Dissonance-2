using UnityEngine;
using UnityEditor;

namespace Assets.Scripts.UI
{
    public class DontDestroy : MonoBehaviour
    {
        void Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}