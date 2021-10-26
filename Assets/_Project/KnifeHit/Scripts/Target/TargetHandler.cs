using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
            Player.Instance.DepositMoney(5);
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
        Player.Instance.DepositMoney(5);
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
        if ((KnifeHitManager.Instance.CurrentKnifeHitMod % 3) <= 2)
        {
            switch (CurrentSpawnerIndex)
            {
                case 0:
                    MetricaManager.SendEvent("target_com_(" + DataManager.Instance.GameData.ProgressData.CurrentMarkLevel + ")");
                    LevelManager.Instance.NextMarkLevel();
                    break;
                case 1:
                    MetricaManager.SendEvent("cube_com_(" + DataManager.Instance.GameData.ProgressData.CurrentCubeLevel + ")");
                    LevelManager.Instance.NextCubeLevel();
                    break;
                case 2:
                    MetricaManager.SendEvent("disk_com_(" + DataManager.Instance.GameData.ProgressData.CurrentFlatLevel + ")");
                    LevelManager.Instance.NextFlatLevel();
                    break;
            }
        }
        else
        {
            switch (CurrentSpawnerIndex)
            {
                case 0:
                    MetricaManager.SendEvent("target2_com_(" + DataManager.Instance.GameData.ProgressData.CurrentMarkLevel + ")");
                    LevelManager.Instance.NextMarkLevel();
                    break;
                case 1:
                    MetricaManager.SendEvent("cube2_com_(" + DataManager.Instance.GameData.ProgressData.CurrentCubeLevel + ")");
                    LevelManager.Instance.NextCubeLevel();
                    break;
                case 2:
                    MetricaManager.SendEvent("disk2_com_(" + DataManager.Instance.GameData.ProgressData.CurrentFlatLevel + ")");
                    LevelManager.Instance.NextFlatLevel();
                    break;
            }
        }

        RewardHandler.Instance.GiveLevelCompleteReward();

        SessionHandler.Instance.CompleteLevel();
    }

    public void SpawnLevel(int spawnerIndex)
    {
        _currentSpawner?.TryCleanTargets();
        _currentSpawner = _spawners[spawnerIndex];
        ColorManager.Instance.RandomColorPreset();
        _currentSpawner.SpawnLevel();
        _levelProgressDisplayer.ShowLevelDisplay();
        SetCurrentTarget();
        IsLevelSpawned?.Invoke();
    }

    public void CleanTargets()
    {
        _currentSpawner?.TryCleanTargets();
    }

    public void EnableSlowMode()
    {
        foreach (var target in Targets)
        {
            target.Rotator.ChangeSpeed(0.5f);
        }
    }

    public void DisableSlowMode()
    {
        foreach (var target in Targets)
        {
            target.Rotator.ChangeSpeed(1f);
        }
    }
}