using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Scripts.Handlers
{
    public class TargetHandler : MonoBehaviour
    {
        [SerializeField] private ExperienceHandler _experienceHandler;
        [SerializeField] private KnifeHandler _knifeHandler;
        [SerializeField] private GamemodHandler _gamemodHandler;
        [SerializeField] private LevelProgressDisplayer _levelProgressDisplayer;
        [SerializeField] private HitScoreDisplayer _hitScoreDisplayer;
        [SerializeField] private TargetMover _targetMover;
        [SerializeField] private List<TargetSpawner> _spawners;

        private TargetSpawner _currentSpawner;
        
        private int Gamemod => DataManager.GameData.ProgressData.CurrentGamemod;
        
        public Target CurrentTarget { get; private set; }
        
        public List<Target> Targets => _spawners[Gamemod].Targets;

        public event UnityAction IsLevelSpawned;
        public event UnityAction IsLevelComplete;

        private void OnEnable()
        {
            _targetMover.IsTargetChanged += OnTargetChanged;
            _gamemodHandler.IsModChanged += SpawnLevel;
        }

        private void OnDisable()
        {
            _targetMover.IsTargetChanged -= OnTargetChanged;
            _gamemodHandler.IsModChanged -= SpawnLevel;
            
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
            _currentSpawner = _spawners[Gamemod];
            _currentSpawner.SpawnLevel();
            _levelProgressDisplayer.ShowLevelDisplay();
            SetCurrentTarget();
            IsLevelSpawned?.Invoke();
        }

        private void OnTargetBreak(int exp)
        {
            _experienceHandler.AddExp(exp);
            _knifeHandler.DisallowThrow();
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
            _experienceHandler.AddExp(exp);
            _knifeHandler.DisallowThrow();
            _levelProgressDisplayer.NextPoint();
            
            _targetMover.RotateCube(CurrentTarget.Base, currentEdge);
            SendHitScore();
        }
        
        private void OnTargetChanged()
        {
            _knifeHandler.AllowThrow();
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
            _knifeHandler.SetKnifeAmount(CurrentTarget.HitToBreak);
        }

        public void ClearTargets()
        {
            _spawners[Gamemod].TryCleanTargets();
        }
    }
}