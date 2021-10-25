using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

namespace KnifeFest
{
    public class FinalCutscene : MonoBehaviour
    {
        private const int LENGTH_CUTSCENE = 20;
        [SerializeField] private StepCutscene _template;
        [SerializeField] private StepCutscene _templateStepEnd;
        [SerializeField] private PathFollower _pathFollower;
        [SerializeField] private KnifeFollower _knifeFollower;

        private List<StepCutscene> _steps = new List<StepCutscene>();

        public static UnityAction OnCreatingCurscene, OnStartingCutscene;


        private void OnEnable()
        {
            OnCreatingCurscene += StartCreateCurscene;
            OnStartingCutscene += StartCutscene;
        }

        private void OnDisable()
        {
            OnCreatingCurscene -= StartCreateCurscene;
            OnStartingCutscene -= StartCutscene;
        }

        private void StartCreateCurscene()
        {
            StartCoroutine(CreateCutscene());
        }

        private IEnumerator CreateCutscene()
        {
            yield return new WaitForSeconds(0.2f);
            _pathFollower.PathCreator.EditorData.PathModifiedByUndo();

            for (int i = 0; i < LENGTH_CUTSCENE; i++)
            {
                if (i == 0)
                {
                    _steps.Add(Instantiate(_template, new Vector3(_template.transform.position.x, _template.transform.position.y, _pathFollower.PathCreator.path.length + 30f), Quaternion.Euler(0, 90, 0), transform));
                }
                else
                {
                    _steps.Add(Instantiate(_template, new Vector3(_template.transform.position.x, _template.transform.position.y, _steps[i - 1].transform.position.z + 20f), Quaternion.Euler(0, 90, 0), transform));
                }

                _steps[i].Init();
            }

            _steps.Add(Instantiate(_templateStepEnd, new Vector3(_templateStepEnd.transform.position.x, _templateStepEnd.transform.position.y, _steps[_steps.Count - 1].transform.position.z + 20f), Quaternion.Euler(0, 90, 0), transform));
            _steps[_steps.Count - 1].SetEndStep();

            for (int i = 0; i < _steps.Count; i++)
            {
                if (i == 0)
                    _steps[i].ChangeIndexMultiprier(0.8f);
                else
                    _steps[i].ChangeIndexMultiprier(_steps[i - 1].Multiplier);

                _steps[i].UpdatingTextsMultiplier();
                _steps[i].gameObject.SetActive(true);
            }

            for (int i = 0; i < _steps.Count; i++)
            {
                _steps[i].ChangeColorMark(i, _steps.Count);
            }
        }

        private void StartCutscene()
        {
            _knifeFollower.GetComponent<Camera>().DOFieldOfView(70, 0.5f);

            _pathFollower.Knife.WeightDisable();

            _pathFollower.PathCreator.bezierPath.AddSegmentToEnd(new Vector3(0f, 0f, _steps[_steps.Count - 1].transform.position.z - 5.5f));
            _pathFollower.PathCreator.EditorData.PathModifiedByUndo();

            StartCoroutine(ControlKnifeRoutine());
        }

        private IEnumerator ControlKnifeRoutine()
        {
            PlayerInput.Instance.Disable();
            _pathFollower.AddSpeedKnifeMoving(1.5f);
            yield return new WaitForSeconds(0.1f);
            _pathFollower.AllowMoveCutscene();
            yield return new WaitForSeconds(0.3f);
            _pathFollower.Knife.AllowStartingCutscene();
            _knifeFollower.AllowStartingCutscene();
        }


    }
}
