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


        private Coroutine destructionCorotine;
        

        public void StartDestructionWithParticleEffect(float delay = 0)
        {
            StartDestruction(delay, true);
        }

        //if time < 0 use this.time instead if time = 0 destroy without starting coroutine
        public void StartDestruction(float delay = 0,bool particles = false, float time = -1)
        {
            if (IsDestructionStarted)
            {
                return;
            }
            if (time == 0)
            {
                Destroy(gameObject);
            }
            else
            {
                destructionCorotine = StartCoroutine(DestructionCoroutine(delay: delay, particles: particles, time: time < 0 ? this.time : time));
            }
        }

        public bool IsDestructionStarted
        {
            get
            {
                return destructionCorotine != null;
            }
        }


        private IEnumerator DestructionCoroutine(float delay, bool particles, float time)
        {

            GameObject particleEffect = null;
            if (particles)
            {
                particleEffect = Instantiate(this.particleEffect);
                particleEffect.GetComponent<ShapeParticles>().SetShapeAndColor(GetComponent<RandomRotation>().CurrentRotation.type, GetComponent<ColorPicker>().GetColor());
                particleEffect.name = string.Format("{0} Particles", name);
            }
            if (delay > 0)
                yield return new WaitForSeconds(delay);
                
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
                
            OnDestroy(gameObject);
            yield return new WaitForEndOfFrame();
            Destroy(gameObject);

        }
    }

}
