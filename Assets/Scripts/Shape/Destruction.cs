using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Shape
{
    public class Destruction:MonoBehaviour
    {

        public GameObject particleEffect = null;

        public System.Action<GameObject> OnDestroy;

        [SerializeField]
        private float time = 0.2f;
        [SerializeField]
        private AnimationCurve curve = null;

        private bool partiallyDestroyed = false;
        private Coroutine destructionCorotine;
        

        public void StartDestructionWithParticleEffect(float delay = 0)
        {
            StartDestruction(delay, true);
        }

        //if time < 0 use this.time instead if time = 0 destroy without starting coroutine
        public void StartDestruction(float delay = 0,bool particles = false, float time = -1, bool completely = false)
        {
            if (Started)
            {
                return;
            }
            if (time == 0)
            {
                DestroyCompletely();
            }
            else
            {
                destructionCorotine = StartCoroutine(DestructionCoroutine(delay: delay, particles: particles, time: time < 0 ? this.time : time, completely: completely));
            }
        }

        public bool Started
        {
            get { return destructionCorotine != null; }
        }

        public bool IsPartiallyDestroyed
        {
            get { return partiallyDestroyed; }
        }

        public void DestroyCompletely(bool particles = false, float delay = 0)
        {
            if (!Started)
            {
                StartDestruction(completely: true, particles: particles, delay: delay);
            }
            else
            {
                StartCoroutine(DestroyCompletelyCoroutine());
            }
        }

        private void OnDrawGizmos()
        {
            if (partiallyDestroyed)
                Gizmos.DrawSphere(transform.position, 10f);
        }

        private IEnumerator DestroyCompletelyCoroutine()
        {
            while (!partiallyDestroyed)
                yield return null;
            OnDestroy(gameObject);
            yield return null;
            Destroy(gameObject);
        }

        private IEnumerator DestructionCoroutine(float delay, bool particles, float time, bool completely)
        {
            if (delay > 0)
                yield return new WaitForSeconds(delay);

            GameObject particleEffect = null;
            if (particles)
            {
                particleEffect = Instantiate(this.particleEffect);
                particleEffect.GetComponent<ShapeParticles>().SetShapeAndColor(GetComponent<RandomRotation>().CurrentRotation.type, GetComponent<ColorPicker>().GetColor());
                particleEffect.name = string.Format("{0} Particles", name);
            }

            Vector3 startScale = transform.localScale;
            float stage = 0;

            do
            {
                stage += Time.deltaTime / time;
                transform.localScale = startScale * curve.Evaluate(stage);
                yield return null;
            } while (stage < 1);

            if (particles)
            {
                particleEffect.transform.position = transform.position;
                particleEffect.SetActive(true);
            }
            partiallyDestroyed = true;
            if (completely)
            {
                yield return DestroyCompletelyCoroutine();
            }
        }
    }

}
