using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private ExperienceHandler _experienceHandler;
    [SerializeField] private HitScoreDisplayer _hitScoreDisplayer;
    [SerializeField] private LevelProgressDisplayer _levelProgressDisplayer;
    [SerializeField] private float _spawnZ;
    [SerializeField] private float _spawnStep;
    [SerializeField] private float _animationDuration;
    [Header("Target Settings")]
    [SerializeField] private Target _targetTemplate;
    [SerializeField] private float _targetSpawnY;
    [SerializeField] private Vector3 _targetExplodePosition;
    [Header("Cube Settings")]
    [SerializeField] private Target _cubeTemplate;
    [SerializeField] private float _cubeSpawnY;
    [SerializeField] private Vector3 _cubeExplodePosition;
    [Header("Flat Settings")]
    [SerializeField] private Target _flatTemplate;
    [SerializeField] private float _flatSpawnY;
    [SerializeField] private Vector3 _flatExplodePosition;

    
    private List<Target> _targets = new List<Target>();
    private Tween _tween;
    private List<int> _expRewards = new List<int>();
    
    public Target CurrentTarget { get; private set; }

    public event UnityAction<GiftSpawner> IsLevelSpawned;
    public event UnityAction<bool> IsWin;
    public event UnityAction<int> IsNewTargetSet;

    private void OnDisable()
    {
        if (CurrentTarget == null) return;
        CurrentTarget.IsRotate -= OnRotate;
        CurrentTarget.IsBreak -= OnTargetBreak;
        CurrentTarget.IsEdgePass -= OnEdgePass;
    }
    
    private void OnEdgePass()
    {
        _player.AllowThrow();
        _levelProgressDisplayer.NextPoint();
        SendHitCount();
    }

    private void OnRotate(int expReward)
    {
        _player.DisallowThrow();
        _experienceHandler.AddExp(expReward);
    }
    
    private void OnTargetBreak(TargetBase targetBase, int expReward)
    {
        CurrentTarget.IsRotate -= OnRotate;
        CurrentTarget.IsBreak -= OnTargetBreak;
        CurrentTarget.IsEdgePass -= OnEdgePass;
        _experienceHandler.AddExp(expReward);
        StartCoroutine(TargetBreakAnimation(targetBase));
    }
    
    private void Move()
    {
        foreach (var target in _targets)
        {
            var position = target.transform.position;
            _tween = target.transform.DOMove(new Vector3(position.x, position.y, position.z - _spawnStep), _animationDuration);
        }

        _tween.OnComplete(() =>
        {
            _player.AllowThrow();
        });
    }

    private IEnumerator TargetBreakAnimation(TargetBase targetBase)
    {
        switch (DataManager.GameData.ProgressData.CurrentGamemod)
        {
            case 0:
                targetBase.Detonate(_targetExplodePosition);
                break;
            case 1:
                targetBase.Detonate(_cubeExplodePosition);
                break;
            case 2:
                targetBase.Detonate(_flatExplodePosition);
                break;
            default:
                targetBase.Detonate(_targetExplodePosition);
                break;
        }
        
        _targets.Remove(CurrentTarget);
        
        if (_targets.Count > 0)
        {
            _levelProgressDisplayer.NextPoint();
            SetCurrentTarget();
            Move();
        }
        else
        {
            IsWin?.Invoke(true);
        }
        
        yield return new WaitForSeconds(2f);
        
        Destroy(targetBase.gameObject);
    }

    private void TryCleanTargets()
    {
        if (_targets.Count < 1) return;
        foreach (var target in _targets)
        {
            Destroy(target.gameObject);
        }

        _targets.Clear();
    }
    
    private void SendHitCount()
    {
        IsNewTargetSet?.Invoke(CurrentTarget.HitToBreak);
        _hitScoreDisplayer.SpawnHitScores(CurrentTarget.HitToBreak);
    }

    public void SetCurrentTarget()
    {
        CurrentTarget = _targets[0];
        CurrentTarget.IsRotate += OnRotate;
        CurrentTarget.IsBreak += OnTargetBreak;
        CurrentTarget.IsEdgePass += OnEdgePass;
        SendHitCount();
    }

    public void SetLotterySettings()
    {
        IsNewTargetSet?.Invoke(3);
        _hitScoreDisplayer.SpawnHitScores(3);
        _levelProgressDisplayer.gameObject.SetActive(false);
    }

    public void SpawnLevel(TargetLevel targetLevel, Knife obstacleTemplate)
    {
        TryCleanTargets();
        
        for (var i = 0; i < targetLevel.Targets.Count; i++)
        {
            _targets.Add(Instantiate(_targetTemplate, new Vector3(0f, _targetSpawnY, _spawnZ + _spawnStep * i), Quaternion.identity, transform));
            _targets[i].SpawnAndSetup(targetLevel.Targets[i], obstacleTemplate);

            if (targetLevel.Targets[i].HasGift)
            {
                IsLevelSpawned?.Invoke(_targets[i].GetComponentInChildren<GiftSpawner>());
            }
        }
    }
    
    public void SpawnLevel(CubeLevel cubeLevel, Knife obstacleTemplate)
    {
        TryCleanTargets();
        
        _targets.Add(Instantiate(_cubeTemplate, new Vector3(0f, _cubeSpawnY, _spawnZ), Quaternion.identity,
            transform));
        _targets[0].SpawnAndSetup(cubeLevel, obstacleTemplate);

        var giftSpawners = _targets[0].GetComponentsInChildren<GiftSpawner>();
        
        IsLevelSpawned?.Invoke(giftSpawners[cubeLevel.Cubes.FindIndex(target => target.HasGift)]);
    }
    
    public void SpawnLevel(FlatLevel flatLevel, Knife obstacleTemplate)
    {
        TryCleanTargets();
        
        for (var i = 0; i < flatLevel.Flats.Count; i++)
        {
            _targets.Add(Instantiate(_flatTemplate, new Vector3(0f, _flatSpawnY, _spawnZ + _spawnStep * i), Quaternion.identity, transform));
            _targets[i].SpawnAndSetup(flatLevel.Flats[i], obstacleTemplate);
            
            if (flatLevel.Flats[i].HasGift)
            {
                IsLevelSpawned?.Invoke(_targets[i].GetComponentInChildren<GiftSpawner>());
            }
        }
    }

    public void Reload(TargetLevel targetLevel, Knife obstacleTemplate)
    {
        for (var i = 0; i < targetLevel.Targets.Count; i++)
        {
            _targets[i].SetupTargetBase(targetLevel.Targets[i], obstacleTemplate);
        }
    }
    
    public void Reload(CubeLevel cubeLevel, Knife obstacleTemplate)
    {
        _targets[0].SetupTargetBase(cubeLevel, obstacleTemplate);
    }
    
    public void Reload(FlatLevel flatLevel, Knife obstacleTemplate)
    {
        for (var i = 0; i < flatLevel.Flats.Count; i++)
        {
            _targets[i].SetupTargetBase(flatLevel.Flats[i], obstacleTemplate);
        }
    }
}
