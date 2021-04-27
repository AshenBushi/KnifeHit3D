using System;
using UnityEngine;

public class SessionHandler : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private KnifeSpawner _knifeSpawner;
    [SerializeField] private TargetSpawner _targetSpawner;
    [SerializeField] private StartScreen _startScreen;
    [SerializeField] private LoseScreen _loseScreen;
    [SerializeField] private WinScreen _winScreen;
    [SerializeField] private InputField _inputField;
    [SerializeField] private ShopScreen _shopScreen;

    private void OnEnable()
    {
        _inputField.IsSessionStart += StartGame;
        _shopScreen.IsKnifeChanged += ReloadGame;
        _targetSpawner.IsWin += OnWin;
        _knifeSpawner.IsLose += OnLose;
        _startScreen.IsModChanged += SpawnLevel;
    }

    private void OnDisable()
    {
        _inputField.IsSessionStart -= StartGame;
        _shopScreen.IsKnifeChanged -= ReloadGame;
        _targetSpawner.IsWin -= OnWin;
        _knifeSpawner.IsLose -= OnLose;
        _startScreen.IsModChanged -= SpawnLevel;
    }

    private void Start()
    {
        SpawnLevel();
        
        _knifeSpawner.SpawnKnife();
    }

    private void StartGame()
    {
        _targetSpawner.SetCurrentTarget();
        _startScreen.StartSession();
    }

    private void SpawnLevel()
    {
        switch (DataManager.GameData.ProgressData.CurrentGamemod)
        {
            case 0:
                _targetSpawner.SpawnLevel(LevelManager.CurrentTargetLevel, _knifeSpawner.CurrentTemplate);
                break;
            case 1:
                //_targetSpawner.SpawnLevel(LevelManager.CurrentCubeLevel, _knifeSpawner.CurrentTemplate);
                break;
            case 2:
                _targetSpawner.SpawnLevel(LevelManager.CurrentFlatLevel, _knifeSpawner.CurrentTemplate);
                break;
            default:
                _targetSpawner.SpawnLevel(LevelManager.CurrentTargetLevel, _knifeSpawner.CurrentTemplate);
                break;
        }
    }
    
    private void ReloadGame()
    {
        switch (DataManager.GameData.ProgressData.CurrentGamemod)
        {
            case 0:
                _targetSpawner.Reload(LevelManager.CurrentTargetLevel, _knifeSpawner.CurrentTemplate);
                break;
            case 1:
                //_targetSpawner.Reload(LevelManager.CurrentCubeLevel, _knifeSpawner.CurrentTemplate);
                break;
            case 2:
                _targetSpawner.Reload(LevelManager.CurrentFlatLevel, _knifeSpawner.CurrentTemplate);
                break;
            default:
                _targetSpawner.Reload(LevelManager.CurrentTargetLevel, _knifeSpawner.CurrentTemplate);
                break;
        }
        _knifeSpawner.Reload();
    }

    private void OnWin()
    {
        switch (DataManager.GameData.ProgressData.CurrentGamemod)
        {
            case 0:
                LevelManager.NextTargetLevel();
                _player.DepositMoney(LevelManager.CurrentTargetLevel.Reward);
                break;
            case 1:
                LevelManager.NextCubeLevel();
                _player.DepositMoney(LevelManager.CurrentCubeLevel.Reward);
                break;
            case 2:
                LevelManager.NextFlatLevel();
                _player.DepositMoney(LevelManager.CurrentFlatLevel.Reward);
                break;
            default:
                break;
        }
        
        _winScreen.Win();
    }
    
    private void OnLose()
    {
        _loseScreen.Lose();
    }
}
