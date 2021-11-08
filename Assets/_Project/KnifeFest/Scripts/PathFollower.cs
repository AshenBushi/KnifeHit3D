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
        [SerializeField] private bool _canMoveCutscene;

        private float _progress;
        private float _distanceTravelled;

        public PathCreator PathCreator => _pathCreator;
        public Knife Knife => _knife;

        private void OnEnable()
        {
            PlayerInput.Instance.IsTapped += AllowMove;
            _pathCreator.pathUpdated += OnPathChanged;
            _knife.OnAddedSpeed += AddSpeed;
        }

        private void OnDisable()
        {
            PlayerInput.Instance.IsTapped -= AllowMove;
            _pathCreator.pathUpdated -= OnPathChanged;
            _knife.OnAddedSpeed -= AddSpeed;
        }

        private void Start()
        {
            transform.position = _pathCreator.path.GetPointAtDistance(0f, EndOfPathInstruction.Stop);
        }

        private void Update()
        {
            if (_canMoveCutscene)
            {
                MoveAlongPath();
                return;
            }
            if (_canMove)
                MoveAlongPath();
        }

        public void AllowMove()
        {
            _canMove = true;
        }

        public void DisallowMove()
        {
            _canMove = false;
        }

        public void AddSpeedKnifeMoving(float speed)
        {
            _speed += speed;
        }

        public void AllowMoveCutscene()
        {
            _canMoveCutscene = true;
        }

        public void DisallowMoveCutscene()
        {
            _canMoveCutscene = false;
        }

        private void OnPathChanged()
        {
            _distanceTravelled = _pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }

        private void MoveAlongPath()
        {
            _distanceTravelled += _speed * Time.fixedDeltaTime;
            transform.position = _pathCreator.path.GetPointAtDistance(_distanceTravelled, EndOfPathInstruction.Stop);

            if (!_canMoveCutscene)
            {
                _progress = _pathCreator.path.GetClosestTimeOnPath(transform.position);
                if (_knife.KnifeWeight <= 0)
                {
                    _canMove = false;

                    foreach (var wall in WallSpawner.Instance.Walls)
                    {
                        wall.AllowUsing();
                    }

                    MetricaManager.SendEvent("arrow_fail_(" + DataManager.Instance.GameData.ProgressData.CurrentKnifeFestLevel + ")");
                    SessionHandler.Instance.FailLevel();
                }

                if (!(_progress >= 1)) return;
                _canMove = false;
                FinalCutscene.OnStartingCutscene?.Invoke();
            }
            else
            {
                if (_knife.KnifeWeight <= 0)
                {
                    _speed = 0;
                    _canMoveCutscene = false;
                    SessionHandler.Instance.CompleteLevelWithCutscene(_knife.MultiplierLastStepCutscene);
                }
            }
        }

        private void AddSpeed()
        {
            _speed += 1.5f;
        }
    }
}