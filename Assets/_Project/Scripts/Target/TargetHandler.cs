using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Scripts.Handlers
{
    public class TargetHandler : Singleton<TargetHandler>
    {
        [SerializeField] private LevelProgressDisplayer _levelProgressDisplayer;
        [SerializeField] private HitScoreDisplayer _hitScoreDisplayer;
        [SerializeField] private TargetMover _targetMover;
        [SerializeField] private List<TargetSpawner> _spawners;

        private TargetSpawner _currentSpawner;
        
        private int Gamemod => (int)GamemodHandler.Instance.CurrentGamemod;
        
        public Target CurrentTarget { get; private set; }
        
        public List<Target> Targets => _spawners[Gamemod].Targets;

        public event UnityAction IsLevelSpawned;
        public event UnityAction IsLevelComplete;

        private void OnEnable()
        {
            _targetMover.IsTargetChanged += OnTargetChanged;
            GamemodHandler.Instance.IsModChanged += SpawnLevel;
        }

        private void OnDisable()
        {
            _targetMover.IsTargetChanged -= OnTargetChanged;
            GamemodHandler.Instance.IsModChanged -= SpawnLevel;
            
            if (CurrentTarget is null) return;
            CurrentTarget.IsTargetBreak -= OnTargetBreak;
            CurrentTarget.IsEdgePass -= OnEdgePass;
        }

        private void Start()
        {
            SpawnLevel();
        }

        private void SpawnLevel()
        {
            _currentSpawner?.TryCleanTargets();

            if (GamemodHandler.Instance.CurrentGamemod == GamemodName.Lottery) return;
            
            _currentSpawner = _spawners[Gamemod];
            _currentSpawner.SpawnLevel();
            _levelProgressDisplayer.ShowLevelDisplay();
            SetCurrentTarget();
            IsLevelSpawned?.Invoke();
        }

        private void OnTargetBreak(int exp)
        {
            ExperienceHandler.Instance.AddExp(exp);
            KnifeHandler.Instance.DisallowThrow();
            CurrentTarget.IsTargetBreak -= OnTargetBreak;
            CurrentTarget.IsEdgePass -= OnEdgePass;
            _currentSpawner.RemoveTarget(CurrentTarget);

            if (Targets.Count > 0)
            {
                _levelProgressDisplayer.NextPoint();
                _targetMover.MoveTargets(Targets);
                SetCurrentTarget();
            }
            else
            {
                IsLevelComplete?.Invoke();
            }
        }

        private void OnEdgePass(int exp, int currentEdge)
        {
            ExperienceHandler.Instance.AddExp(exp);
            KnifeHandler.Instance.DisallowThrow();
            _levelProgressDisplayer.NextPoint();
            
            _targetMover.RotateCube(CurrentTarget.Base, currentEdge);
            SendHitScore();
        }
        
        private void OnTargetChanged()
        {
            KnifeHandler.Instance.AllowThrow();
        }

        private void SetCurrentTarget()
        {
            CurrentTarget = Targets[0];
            CurrentTarget.IsTargetBreak += OnTargetBreak;
            CurrentTarget.IsEdgePass += OnEdgePass;
            SendHitScore();
        }

        private void SendHitScore()
        {
            _hitScoreDisplayer.SpawnHitScores(CurrentTarget.HitToBreak);
            KnifeHandler.Instance.SetKnifeAmount(CurrentTarget.HitToBreak);
        }

        public void ClearTargets()
        {
            _spawners[Gamemod].TryCleanTargets();
        }
    }
}