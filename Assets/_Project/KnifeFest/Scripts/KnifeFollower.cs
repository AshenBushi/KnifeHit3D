using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnifeFest
{
    public class KnifeFollower : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        private bool _isCutscene;

        private Vector3 _lastTargetPosition;

        private void Start()
        {
            ColorManager.Instance.RandomColorPreset();
        }

        private void Update()
        {
            if (_lastTargetPosition == _target.position) return;
            if (!_isCutscene)
                Follow();
            else
                FollowCutscene();
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
            transform.position = new Vector3(position.x, position.y + 5.5f, position.z - 6);
            _lastTargetPosition = position;
        }

        public void AllowCutscene()
        {
            StartCoroutine(AllowCutsceneRoutine());
        }

        private IEnumerator AllowCutsceneRoutine()
        {
            var position = _target.position;
            yield return transform.DOMove(new Vector3(position.x, position.y + 5.5f, position.z - 6.5f), 1f);

            _isCutscene = true;
        }
    }
}