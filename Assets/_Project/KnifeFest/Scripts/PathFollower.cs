using PathCreation;
using UnityEngine;

namespace KnifeFest
{
    public class PathFollower : MonoBehaviour
    {
        [SerializeField] private PathCreator _pathCreator;
        [SerializeField] private Knife _knife;
        [SerializeField] private float _speed = 5;
        [SerializeField] private bool _canMove;

        private float _progress;
        private float _distanceTravelled;

        private void OnEnable()
        {
            _pathCreator.pathUpdated += OnPathChanged;
        }

        private void OnDisable()
        {
            _pathCreator.pathUpdated -= OnPathChanged;
        }

        private void Start()
        {
            transform.position = _pathCreator.path.GetPointAtDistance(0f, EndOfPathInstruction.Stop);
        }

        private void Update()
        {
            if(!_canMove) return;
            MoveAlongPath();
        }

        private void OnPathChanged()
        {
            _distanceTravelled = _pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }

        private void MoveAlongPath()
        {
            _distanceTravelled += _speed * Time.deltaTime;
            transform.position = _pathCreator.path.GetPointAtDistance(_distanceTravelled, EndOfPathInstruction.Stop);
            _progress = _pathCreator.path.GetClosestTimeOnPath(transform.position);

            if (_knife.KnifeWeight <= 0)
            {
                _canMove = false;
                SessionHandler.Instance.FailLevel();
            }
            
            if (!(_progress >= 1)) return;
            _canMove = false;
            //SessionHandler.Instance.CompleteLevel();
        }

        public void AllowMove()
        {
            _canMove = true;
        }
        
        public void DisallowMove()
        {
            _canMove = false;
        }
    }
}