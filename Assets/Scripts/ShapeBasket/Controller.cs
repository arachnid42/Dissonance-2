using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.ShapeBasket
{
    public class Controller : MonoBehaviour
    {
        [SerializeField]
        private Hide hide = null;
        [SerializeField]
        private ShapeCatcher shapeCatcher = null;
        [SerializeField]
        private TileSwitcher tileSwitcher = null;
        [SerializeField]
        private BasketBody basketBody = null;
        [SerializeField]
        private ShapeMixer shapeMixer = null;

        public Hide Hide
        {
            get
            {
                return hide;
            }
        }

        public ShapeCatcher ShapeCatcher
        {
            get
            {
                return shapeCatcher;
            }
        }

        public BasketBody BasketBody
        {
            get { return basketBody; }
        }

        public TileSwitcher TileSwitcher
        {
            get
            {
                return tileSwitcher;
            }
        }

        public ShapeMixer ShapeMixer
        {
            get
            {
                return shapeMixer;
            }
        }
    }
}
