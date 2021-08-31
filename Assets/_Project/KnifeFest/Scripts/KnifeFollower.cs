using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnifeFest
{
    public class KnifeFollower : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        private Vector3 _lastTargetPosition;

        private void Start()
        {
            ColorManager.Instance.RandomColorPreset();
        }

        private void Update()
        {
            if(_lastTargetPosition == _target.position) return;
            Follow();
        }

        private void Follow()
        {
            var position = _target.position;
            transform.position = new Vector3(position.x, position.y + 4, position.z - 5);
            _lastTargetPosition = position;
        }
    }
}