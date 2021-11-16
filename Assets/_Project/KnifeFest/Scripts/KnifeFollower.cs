using DG.Tweening;
using UnityEngine;

namespace KnifeFest
{
    public class KnifeFollower : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _offsetYPos = 4f;
        [SerializeField] private float _offsetZPos = 5.5f;
        private bool _isStartCutscene;

        private Camera _camera;
        private Vector3 _lastTargetPosition;
        private Tweener _tween;

        public Camera Camera => _camera;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void Start()
        {
            ColorManager.Instance.RandomColorPreset();
        }

        private void Update()
        {
            if (_lastTargetPosition == _target.position) return;
            Follow();
        }

        public void AllowStartingCutscene()
        {
            _tween = transform.DOMove(new Vector3(_target.position.x, _target.position.y + _offsetYPos, _target.position.z - _offsetZPos), 0.5f).SetAutoKill(false).SetLink(gameObject);
            _offsetYPos = 6.5f;
            _offsetZPos = 7f;

            _isStartCutscene = true;
        }

        private void Follow()
        {
            var position = _target.position;
            if (_isStartCutscene)
                _tween.ChangeEndValue(new Vector3(position.x, position.y + _offsetYPos, position.z - _offsetZPos), true).Restart();
            else
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(position.x, position.y + _offsetYPos, position.z - _offsetZPos), 2 / Time.deltaTime);
            _lastTargetPosition = position;
        }
    }
}