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
    }

    private void OnDisable()
    {
        _inputField.IsSessionStart -= StartGame;
        _shopScreen.IsKnifeChanged -= ReloadGame;
        _targetSpawner.IsWin -= OnWin;
        _knifeSpawner.IsLose -= OnLose;
    }

    private void Start()
    {
        _targetSpawner.SpawnLevel(LevelManager.CurrentTargetLevel, _knifeSpawner.CurrentTemplate);
        _knifeSpawner.SpawnKnife();
    }

    private void StartGame()
    {
        _targetSpawner.SetCurrentTarget();
        _startScreen.StartSession();
    }

    private void ReloadGame()
    {
        _targetSpawner.Reload(LevelManager.CurrentTargetLevel, _knifeSpawner.CurrentTemplate);
        _knifeSpawner.Reload();
    }

    private void OnWin()
    {
        LevelManager.NextTargetLevel();
        _player.DepositMoney(LevelManager.CurrentTargetLevel.Reward);
        _winScreen.Win();
    }
    
    private void OnLose()
    {
        _loseScreen.Lose();
    }
}
