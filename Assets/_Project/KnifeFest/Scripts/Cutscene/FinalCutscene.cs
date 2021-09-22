using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KnifeFest
{
    public class FinalCutscene : MonoBehaviour
    {
        private const int LENGTH_CUTSCENE = 20;
        [SerializeField] private StepCutscene _template;
        [SerializeField] private PathFollower _pathFollower;
        [SerializeField] private Transform _mainRoad;
        [SerializeField] private KnifeFollower _knifeFollower;

        private List<StepCutscene> _steps = new List<StepCutscene>();

        public static UnityAction OnStartingCutscene;


        private void OnEnable()
        {
            OnStartingCutscene += StartCutscene;
        }

        private void OnDisable()
        {
            OnStartingCutscene -= StartCutscene;
        }

        private void StartCutscene()
        {
            _pathFollower.Knife.WeightDisable();

            int k = 0;
            for (int i = 0; i < LENGTH_CUTSCENE; i++)
            {
                if (i == 0)
                {
                    var tempObj = Instantiate(_template, new Vector3(-10, 0, (Camera.main.ScreenToWorldPoint(new Vector3(0, 0, _mainRoad.localScale.z)).z + 25f)), Quaternion.Euler(0, 90, 0), transform);

                    ChangeIndexStep(i, tempObj);

                    tempObj.IndexTwo = k % 10;
                    _steps.Add(tempObj);
                }
                else
                {
                    var tempObj = Instantiate(_template, new Vector3(-10, 0, _steps[i - 1].transform.position.z + 20f), Quaternion.Euler(0, 90, 0), transform);

                    ChangeIndexStep(i, tempObj);

                    tempObj.IndexTwo = k % 10;
                    _steps.Add(tempObj);
                }
                k += 2;
            }

            for (int i = 0; i < _steps.Count; i++)
            {
                UpdatingTextSteps(i);
            }

            _pathFollower.PathCreator.bezierPath.AddSegmentToEnd(new Vector3(0f, 0f, _steps[_steps.Count - 1].transform.position.z));
            _pathFollower.PathCreator.EditorData.PathModifiedByUndo();

            StartCoroutine(StartAnimations());
            StartCoroutine(ControlKnifeRoutine());
        }

        private void UpdatingTextSteps(int i)
        {
            _steps[i].TextLeft.text = "x" + _steps[i].Index + "." + _steps[i].IndexTwo;
            _steps[i].TextRight.text = "x" + _steps[i].Index + "." + _steps[i].IndexTwo;
        }


        private IEnumerator StartAnimations()
        {
            for (int i = 0; i < _steps.Count; i++)
            {
                _steps[i].gameObject.SetActive(true);
                if (i % 2 != 0)
                    _steps[i].GetComponent<Animator>().Play("StepCutscene_AppearRL");
                else
                    _steps[i].GetComponent<Animator>().Play("StepCutscene_Appear");
                yield return new WaitForSeconds(_steps[i].GetComponent<Animation>().clip.length / 25f);
            }
        }

        private IEnumerator ControlKnifeRoutine()
        {
            PlayerInput.Instance.Disable();
            _pathFollower.Speed += 20f;
            yield return new WaitForSeconds(0.2f);
            _pathFollower.CanMoveCutscene = true;
            _pathFollower.Knife.transform.DOMoveX(0f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            _pathFollower.Knife.IsStartingCutscene = true;
            _knifeFollower.AllowCutscene();
        }

        private void ChangeIndexStep(int i, StepCutscene step)
        {
            //согласен не очень хорошо сделал)
            //по времени не смог иначе реализовать (но т.к. всегда максимальный множитель х5.0, то можно и так оставить)
            if (i < 5)
                step.Index = 1;
            else if (i < 10 && i >= 5)
                step.Index = 2;
            else if (i < 15 && i >= 10)
                step.Index = 3;
            else if (i < 20 && i >= 15)
                step.Index = 4;
            else if (i < 25 && i >= 20)
                step.Index = 5;
        }
    }
}
