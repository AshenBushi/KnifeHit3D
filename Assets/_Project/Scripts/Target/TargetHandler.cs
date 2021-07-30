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
        
        public Target CurrentTarget { get; private set; }
        public List<Target> Targets => _currentSpawner.Targets;

        public int CurrentSpawnerIndex => _spawners.IndexOf(_currentSpawner);

        public event UnityAction IsLevelSpawned;

        private void OnEnable()
        {
            _targetMover.IsTargetChanged += OnTargetChanged;
        }

        private void OnDisable()
        {
            _targetMover.IsTargetChanged -= OnTargetChanged;

            if (CurrentTarget is null) return;
            CurrentTarget.IsTargetBreak -= OnTargetBreak;
            CurrentTarget.IsEdgePass -= OnEdgePass;
        }

        private void OnTargetBreak(int exp)
        {
            ExperienceHandler.Instance.AddExp(exp);
            PlayerInput.Instance.DisallowTap();
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
                CompleteLevel();
            }
        }

        private void OnEdgePass(int exp, int currentEdge)
        {
            ExperienceHandler.Instance.AddExp(exp);
            PlayerInput.Instance.DisallowTap();
            _levelProgressDisplayer.NextPoint();
            
            _targetMover.RotateCube(CurrentTarget.Base, currentEdge);
            SendHitScore();
        }
        
        private void OnTargetChanged()
        {
            PlayerInput.Instance.AllowTap();
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

        private void CompleteLevel()
        {
            var rewardIndex = 0;
            
            switch (CurrentSpawnerIndex)
            {
                case 0:
                    MetricaManager.SendEvent("target_lvl_complete_(" + DataManager.GameData.ProgressData.CurrentMarkLevel + ")");
                    rewardIndex = LevelManager.CurrentMarkLevel.KnifeReward;
                    LevelManager.NextMarkLevel();
                    break;
                case 1:
                    MetricaManager.SendEvent("cube_lvl_start_(" + DataManager.GameData.ProgressData.CurrentMarkLevel + ")");
                    rewardIndex = LevelManager.CurrentCubeLevel.KnifeReward;
                    LevelManager.NextCubeLevel();
                    break;
                case 2:
                    MetricaManager.SendEvent("flat_lvl_complete_(" + DataManager.GameData.ProgressData.CurrentMarkLevel + ")");
                    rewardIndex = LevelManager.CurrentFlatLevel.KnifeReward;
                    LevelManager.NextFlatLevel();
                    break;
            }
            
            SessionHandler.Instance.CompleteLevel(rewardIndex);
        }
        
        public void SpawnLevel(int spawnerIndex)
        {
            _currentSpawner?.TryCleanTargets();
            _currentSpawner = _spawners[spawnerIndex];
            _currentSpawner.SpawnLevel();
            _levelProgressDisplayer.ShowLevelDisplay();
            SetCurrentTarget();
            IsLevelSpawned?.Invoke();
        }
    }
}