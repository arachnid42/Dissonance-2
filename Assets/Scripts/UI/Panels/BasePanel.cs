using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Localization;
using Assets.Scripts.Game;

namespace Assets.Scripts.UI.Panels
{
    public class BasePanel : MonoBehaviour
    {
        [SerializeField]
        private float fadeDuration;

        private const float ACTIVE_PANEL_ALPHA = 1;
        private const float INACTIVE_PANEL_ALPHA = 0;
        
        public class Animation
        {
            public List<IEnumerator> chain = new List<IEnumerator>();

            public Animation After(IEnumerator coroutine)
            {
                chain.Add(coroutine);
                return this;
            }

            private IEnumerator CycleCoroutines(IEnumerator[] chain)
            {
                foreach (IEnumerator coroutine in chain)
                    yield return UIController.Instance.StartCoroutine(coroutine);
                UIController.Instance.data.isAnimationPlays = false;
            }

            public Coroutine Start(bool playAnyway = false)
            {
                if (UIController.Instance.data.isAnimationPlays && !playAnyway)
                {
                    return null;
                }
                UIController.Instance.data.isAnimationPlays = true;
                return UIController.Instance.StartCoroutine(CycleCoroutines(chain.ToArray()));
            }

            public Animation(IEnumerator initial = null)
            {
                if (initial != null)
                    chain.Add(initial);
            }
        }

        private void OnDisable()
        {
            UIController.Instance.data.activePanel = null;
        }

        public IEnumerator AlterField(System.Action action)
        {
            while (Field.Instance == null || Field.Instance.Master.Callbacks == null || Field.Instance.Master.Listeners == null)
            {
                yield return null;
            }
            action();
        }

        public IEnumerator AlterUI(System.Action action)
        {
            while (UIController.Instance == null)
            {
                yield return null;
            }
            action();
        }

        public string Text(string key)
        {
            return LocalizationManager.Instance[key];
        }

        public void SetLabels(System.Action action)
        {
            StartCoroutine(SetLabelsCoroutine(action));
        }

        public IEnumerator SetHiddenEnumerator(bool hidden, float duration = -1, System.Action after = null, GameObject background = null)
        {
            duration = duration < 0 ? fadeDuration : duration;
            return PanelFadeCoroutine(duration, !hidden, after, background);
        }

        public Animation SetHidddenAnimation(bool hidden, float duration = -1, System.Action after = null, GameObject background = null)
        {
            return new Animation(SetHiddenEnumerator(hidden, duration, after, background));
        }


        public Animation SwitchToAnimation(BasePanel panel, float fadeOutDuration = -1, float fadeInDuration = -1, System.Action after = null, GameObject background = null)
        {
            Debug.LogFormat("SwitchToAnimation {0}", panel);
            var animation = new Animation();
            animation.After(SetHiddenEnumerator(true,fadeOutDuration));
            animation.After(panel.SetHiddenEnumerator(false, fadeInDuration, after, background));
            return animation;
        }

        private IEnumerator SetLabelsCoroutine(System.Action action)
        {
            while (LocalizationManager.Instance == null)
                yield return null;
            action();
        }


        private IEnumerator PanelFadeCoroutine(float duration, bool active, System.Action after = null, GameObject background = null)
        {
            float elapsedTime = 0f;
            float startAlpha = ACTIVE_PANEL_ALPHA;
            float endAlpha = INACTIVE_PANEL_ALPHA;
            if (active)
            {
                if (background != null)
                    background.SetActive(true);
                gameObject.SetActive(true);
                startAlpha = INACTIVE_PANEL_ALPHA;
                endAlpha = ACTIVE_PANEL_ALPHA;
            }
            if (elapsedTime == duration)
            {
                if (background != null)
                    background.GetComponent<CanvasGroup>().alpha = endAlpha;
                gameObject.GetComponent<CanvasGroup>().alpha = endAlpha;
            }else
                while (elapsedTime < duration)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                    float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
                    gameObject.GetComponent<CanvasGroup>().alpha = currentAlpha;
                    if (background != null)
                        background.GetComponent<CanvasGroup>().alpha = currentAlpha;
                    yield return null;
                }

            if (!active)
            {
                if (background != null)
                    background.SetActive(false);
                gameObject.SetActive(false);
            }
            if (after != null)
                after();
        }



        
    }
}

