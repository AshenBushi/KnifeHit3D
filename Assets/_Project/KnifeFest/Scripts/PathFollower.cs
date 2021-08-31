using PathCreation;
using UnityEngine;

namespace KnifeFest
{
    public class PathFollower : MonoBehaviour
    {
        [SerializeField] private PathCreator _pathCreator;
        [SerializeField] private float _speed = 5;
        [SerializeField] private bool _canMove;
    
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
            transform.rotation = _pathCreator.path.GetRotationAtDistance(0f, EndOfPathInstruction.Stop);
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
            transform.rotation = _pathCreator.path.GetRotationAtDistance(_distanceTravelled, EndOfPathInstruction.Stop);
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