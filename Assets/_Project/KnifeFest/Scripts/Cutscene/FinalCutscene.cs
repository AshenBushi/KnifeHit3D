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
                    _steps.Add(Instantiate(_template, new Vector3(-10, 0, _pathFollower.PathCreator.path.length + 30f), Quaternion.Euler(0, 90, 0), transform));
                }
                else
                {
                    _steps.Add(Instantiate(_template, new Vector3(-10, 0, _steps[i - 1].transform.position.z + 20f), Quaternion.Euler(0, 90, 0), transform));
                }
            }

            for (int i = 0; i < _steps.Count; i++)
            {
                if (i == 0)
                    _steps[i].ChangeIndexMultiprier(0.8f);
                else
                    _steps[i].ChangeIndexMultiprier(_steps[i - 1].Multiplier);

                _steps[i].UpdatingTextsMultiplier();
            }

            KnifeFestManager.IsLoadingDataComplete?.Invoke();

            StartCoroutine(StartAnimations());
        }

        private void StartCutscene()
        {
            _pathFollower.Knife.WeightDisable();

            _pathFollower.PathCreator.bezierPath.AddSegmentToEnd(new Vector3(0f, 0f, _steps[_steps.Count - 1].transform.position.z));
            _pathFollower.PathCreator.EditorData.PathModifiedByUndo();

            for (int i = 0; i < _steps.Count; i++)
            {
                _steps[i].Wall.ChangeColor();
            }

            StartCoroutine(ControlKnifeRoutine());
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
            _pathFollower.Speed += 5f;
            yield return new WaitForSeconds(0.2f);
            _pathFollower.CanMoveCutscene = true;
            _pathFollower.Knife.transform.DOMoveX(0f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            _pathFollower.Knife.IsStartingCutscene = true;
            _knifeFollower.AllowCutscene();
        }


    }
}
