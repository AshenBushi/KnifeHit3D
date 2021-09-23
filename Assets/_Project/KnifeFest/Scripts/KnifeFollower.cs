using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace KnifeFest
{
    public class KnifeFollower : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        private bool _isStartCutscene;

        private Vector3 _lastTargetPosition;

        private void Start()
        {
            ColorManager.Instance.RandomColorPreset();
        }

        private void Update()
        {
            if (_lastTargetPosition == _target.position) return;
            if (_isStartCutscene)
                FollowCutscene();
            else
                Follow();

        }

        public void AllowCutscene()
        {
            var position = _target.position;
            transform.DOMove(new Vector3(position.x, position.y + 6.5f, position.z - 7.5f), 0.5f);

            StartCoroutine(AllowCutsceneRoutine());
        }

        private void Follow()
        {
            var position = _target.position;
            transform.position = new Vector3(position.x, position.y + 4, position.z - 5);
            _lastTargetPosition = position;
        }

        private void FollowCutscene()
        {
            var position = _target.position;
            transform.position = new Vector3(_target.position.x, _target.position.y + 6.5f, _target.position.z - 7.5f);
            _lastTargetPosition = position;
        }

        private IEnumerator AllowCutsceneRoutine()
        {
            yield return new WaitForSeconds(0.3f);
            _isStartCutscene = true;
        }
    }
}