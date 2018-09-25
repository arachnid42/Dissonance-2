using Assets.Scripts.Localization;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class Field : MonoBehaviour
    {
        [SerializeField]
        private GameObject field916, field918;
        [SerializeField]
        private Tutorial tutorial;

        public Tutorial Tutorial
        {
            get
            {
                return tutorial;
            }
        }

        public static bool Ready
        {
            get;set;
        }

        public bool Is918
        {
            get;private set;
        }

        public Master Master
        {
            get; private set;
        }
        public ColorsPresetsManager.ColorsRecipients ColorsRecipients
        {
            get; private set;
        }

        public LocalizationManager.Listener[] LocalizationListeners
        {
            get; private set;
        }

        public static Field Instance
        {
            get;private set;
        }

        private void SelectFieldByAspect()
        {
            float currentAspect = Camera.main.aspect;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }else if(Instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void Set918Active(bool active)
        {            
            GameObject selected = null;
            Is918 = active;
            if (active)
            {
                selected = field918;
                field918.SetActive(active);
                Destroy(field916);
            }
            else
            {
                selected = field916;
                field916.SetActive(!active);
                Destroy(field918);
            }
            Master = selected.GetComponentInChildren<Master>();
            ColorsRecipients = selected.GetComponentInChildren<FieldColorsRecipients>().colorsRecipients;
            LocalizationListeners = selected.GetComponentInChildren<FieldLocalization>().listeners;
            Ready = true;
        }
    }


}
