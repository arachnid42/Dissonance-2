using UnityEngine;

namespace Assets.Scripts.Indicator
{
    public class Bonuses : MonoBehaviour
    {
        private static readonly int FREEZE_BONUSES_MAX_COUNT = 10;
        private static readonly int EXPLOSION_BONUSES_MAX_COUNT = 10;
        private static readonly int HEART_BONUSES_MAX_COUNT = 9;


        [SerializeField]
        private Vector3 hiddenPosition;
        [SerializeField]
        private Vector3 visiblePosition;
        [SerializeField]
        private bool hidden = false;
        private float hideStage = 0;
        private float hideTime = 0.2f;
        private bool hideStarted = false;
        private Vector3 startPosition;
        private Vector3 endPosition;

        [SerializeField]
        private int freezeBonusesCount = 0;
        [SerializeField]
        private int explosionBonusesCount = 0;
        [SerializeField]
        private int heartBonusesCount = 0;

        [SerializeField]
        private Transform heartBonusStartPosition;
        [SerializeField]
        private Transform heartBonusesEndPosition;
        [SerializeField]
        private Bonus freezeBonus;
        [SerializeField]
        private Bonus explosionBonus;
        [SerializeField]
        private Bonus heartBonus;
        [SerializeField]
        private Bonus[] heartBonusesArray;
        [SerializeField, Range(-1,1)]
        private float heartBonusPosOffset = 0;
        [SerializeField, Range(0,1)]
        private float heartBonusSpacing = 0.1f;

        private Vector3 GetHeartBonusPosition(int i)
        {
            float lerp = heartBonusSpacing * i + heartBonusPosOffset;
            Vector3 pos = Vector3.Lerp(heartBonusStartPosition.localPosition, heartBonusesEndPosition.localPosition, lerp);
            return pos;
        }



        public int Freezes
        {
            get { return freezeBonusesCount; }
            set
            {
                freezeBonusesCount = Mathf.Clamp(value, 0, FREEZE_BONUSES_MAX_COUNT);
                if (value <= 0)
                {
                    freezeBonus.Enabled = false;
                }
                else
                {
                    freezeBonus.Enabled = true;
                }

                freezeBonus.Number = freezeBonusesCount = value;
                UpdateHiddenIfNeeded();
            }
        }

        public int Explosions
        {
            get { return explosionBonusesCount; }
            set
            {
                explosionBonusesCount = Mathf.Clamp(value, 0, EXPLOSION_BONUSES_MAX_COUNT);
                if (explosionBonusesCount <= 0)
                {
                    explosionBonus.Enabled = false;
                }
                else
                {
                    explosionBonus.Enabled = true;
                }
                explosionBonus.Number = explosionBonusesCount;
                UpdateHiddenIfNeeded();
            }
        }

        public int Hearts
        {
            get { return heartBonusesCount; }
            set
            {
                heartBonusesCount = Mathf.Clamp(value, 0, HEART_BONUSES_MAX_COUNT);
                float halfElemOffset = (heartBonusesArray.Length - heartBonusesCount) / 2;
                int startIndex = Mathf.CeilToInt(halfElemOffset);
                int endIndex = startIndex + heartBonusesCount-1;
                int centerIndex = startIndex + Mathf.FloorToInt(heartBonusesCount/2);
                float xOffset = 0;
                if (heartBonusesCount != HEART_BONUSES_MAX_COUNT)
                {
                    if (heartBonusesCount % 2 != 0)
                    {
                        xOffset = -GetHeartBonusPosition(centerIndex).x;
                    }
                    else
                    {
                        var c1 = GetHeartBonusPosition(centerIndex).x;
                        var c2 = GetHeartBonusPosition(centerIndex - 1).x;
                        xOffset = -(c2 - (c2 - c1) / 2);
                    }
                }

                
                //Debug.Log("SI:" + startIndex + ", EI:" + endIndex + ", xOffset:"+ xOffset);
                for (int i = 0; i < heartBonusesArray.Length; i++)
                {
                    var bonus = heartBonusesArray[i];
                    if(i>=startIndex && i<=endIndex)
                    {
                        var pos = GetHeartBonusPosition(i);
                        pos.x += xOffset;
                        if (bonus.Enabled)
                        {
                            bonus.MoveTo(pos);
                        }
                        else
                        {
                            bonus.transform.localPosition = pos;
                            bonus.Enabled = true;
                        }
                        
                        
                    }
                    else
                    {
                        bonus.Enabled = false;
                    }
                    
                }

                UpdateHiddenIfNeeded();
            }
        }

        private void UpdateHiddenIfNeeded()
        {
            bool newHidden = heartBonusesCount <= 0 && freezeBonusesCount <= 0 && explosionBonusesCount <= 0;
            if (hidden == newHidden)
                return;
            hideStarted = false;
            if (newHidden)
            {
                startPosition = visiblePosition;
                endPosition = hiddenPosition;
                
            }
            else
            {
                startPosition = hiddenPosition;
                endPosition = visiblePosition;
            }
            hideStage = 0;
            hideStarted = true;
            hidden = newHidden;
        }

        public bool Hidden
        {
            get
            {
                return hidden;
            }
        }

        void Start()
        {
            heartBonusesArray = new Bonus[HEART_BONUSES_MAX_COUNT];
            var original = heartBonus;
            for (int i = 0; i < heartBonusesArray.Length; i++)
            {
                var heart = Instantiate(original, transform);

                heart.transform.localPosition = GetHeartBonusPosition(i);
                heart.transform.localRotation = original.transform.localRotation;

                heartBonusesArray[i] = heart;
            }

            transform.localPosition = hidden ? hiddenPosition : visiblePosition;
        }

        public void Update()
        {
            if (hideStarted)
            {
                hideStage += Time.unscaledDeltaTime / hideTime;
                transform.localPosition = Vector3.Lerp(startPosition, endPosition, hideStage);
                hideStarted = hideStage < 1;
            }
        }



    }
}
