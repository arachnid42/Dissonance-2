using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Assets.Scripts.Game
{

    public class Master : MonoBehaviour
    {

        [SerializeField]
        private State state = null;
        private Coroutine updateCoroutine;
        public State State
        {
            get { return state; }
        }

        public Callbacks Callbacks
        {
            get; private set;
        }

        public Listeners Listeners
        {
            get; private set;
        }


        public Actions Actions
        {
            get; private set;
        }


        private void Awake()
        {
            Actions = new Actions(this);
            Listeners = new Listeners();
            Callbacks = new Callbacks(this);
            Callbacks.SetShapeBasketCallbacks(State.Mapping.field.shapeBasket);
        }

        private void Start()
        {
            State.Reset();
        }

        private System.Action RemoveShapeFromScreen(GameObject shape)
        {
            return () =>
            {
                shape.GetComponent<Shape.Controller>().Destruction.DestroyCompletely();
            };
        }

        private void ClearScreenShapesOnScreen()
        {
            System.Action clearScreenAction = null;
            foreach(var shape in State.shapesOnScreen)
            {
                clearScreenAction += RemoveShapeFromScreen(shape);
            }
            clearScreenAction();
        }


        private IEnumerator UpdateCoroutine()
        {


            Time.timeScale = 1;
            State.Reset();
            State.Started = true;
            yield return new WaitForSecondsRealtime(1.5f);

            while (State.started)
            {
                if (State.paused)
                {
                    yield return null;
                    continue;
                }


                if (Actions.CheckDifficultyTarget())
                {
                    Actions.ExplodeShapesOnScreen();
                    state.SetGameOverData(true);
                    Listeners.OnGameOver(true);
                    state.Started = false;
                    break;
                }

                Actions.UpdateTimer();
                Actions.TryToStartRandomRotation();
                Actions.TryToChangeGameModeParameters();

                if (State.shapesOnScreen.Count < State.shapesOnScreenLimit)
                {
                    GameObject shape = Actions.GetNextShape();
                    if (shape != null)
                    {
                        state.shapesOnScreen.Add(shape);
                    }
                }

                yield return null;

            }
            Time.timeScale = 1;
            ClearScreenShapesOnScreen();
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause && State.started && !State.paused)
            {
                Actions.Pause();
            }
        }

        public void Restart()
        {
            if (updateCoroutine != null)
                StopCoroutine(updateCoroutine);
            Debug.Log(DifficultyLevels.Instance.CurrentDifficulty.name);
            updateCoroutine = StartCoroutine(UpdateCoroutine());
        }
       

        public void Stop()
        {
            State.Started = false;
        }
    }

}
