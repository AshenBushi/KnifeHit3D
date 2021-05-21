using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SessionHandler : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private KnifeSpawner _knifeSpawner;
    [SerializeField] private TargetSpawner _targetSpawner;
    [SerializeField] private LotterySpawner _lotterySpawner;
    [SerializeField] private StartScreen _startScreen;
    [SerializeField] private LoseScreen _loseScreen;
    [SerializeField] private WinScreen _winScreen;
    [SerializeField] private ShopScreen _shopScreen;
    [SerializeField] private LotteryScreen _lotteryScreen;
    [SerializeField] private InputField _inputField;
    [SerializeField] private AppleCounter _appleCounter;

    private GiftHandler _giftHandler;

    public event UnityAction IsGameStarted;

    private void Awake()
    {
        _giftHandler = GetComponent<GiftHandler>();
    }

    private void OnEnable()
    {
        _inputField.IsSessionStart += StartGame;
        _shopScreen.IsKnifeChanged += ReloadGame;
        _targetSpawner.IsWin += OnGameEnd;
        _knifeSpawner.IsLose += OnGameEnd;
        _giftHandler.IsGiftScreenDisabled += EndGame;
        _lotterySpawner.IsWin += OnLotteryWin;
        _lotterySpawner.IsLose += LoseGame;
        _startScreen.IsModChanged += SpawnLevel;
        _winScreen.IsCanStartLottery += StartLottery;
    }

    private void OnDisable()
    {
        _inputField.IsSessionStart -= StartGame;
        _shopScreen.IsKnifeChanged -= ReloadGame;
        _targetSpawner.IsWin -= OnGameEnd;
        _knifeSpawner.IsLose -= OnGameEnd;
        _giftHandler.IsGiftScreenDisabled += EndGame;
        _lotterySpawner.IsWin -= OnLotteryWin;
        _lotterySpawner.IsLose -= LoseGame;
        _startScreen.IsModChanged -= SpawnLevel;
        _winScreen.IsCanStartLottery -= StartLottery;
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
        _appleCounter.gameObject.SetActive(true);
        
        switch (DataManager.GameData.ProgressData.CurrentGamemod)
        {
            case 0:
                MetricaManager.SendEvent("target_lvl_start_(" + DataManager.GameData.ProgressData.CurrentTargetLevel + ")");
                break;
            case 1:
                MetricaManager.SendEvent("cube_lvl_start_(" + DataManager.GameData.ProgressData.CurrentCubeLevel + ")");
                break;
            case 2:
                MetricaManager.SendEvent("flat_lvl_start_(" + DataManager.GameData.ProgressData.CurrentFlatLevel + ")");
                break;
            default:
                MetricaManager.SendEvent("target_lvl_start_(" + DataManager.GameData.ProgressData.CurrentTargetLevel + ")");
                break;
        }
        
        IsGameStarted?.Invoke();
    }

    private void SpawnLevel()
    {
        switch (DataManager.GameData.ProgressData.CurrentGamemod)
        {
            case 0:
                _targetSpawner.SpawnLevel(LevelManager.CurrentTargetLevel, _knifeSpawner.CurrentTemplate);
                break;
            case 1:
                _targetSpawner.SpawnLevel(LevelManager.CurrentCubeLevel, _knifeSpawner.CurrentTemplate);
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
                _targetSpawner.Reload(LevelManager.CurrentCubeLevel, _knifeSpawner.CurrentTemplate);
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

    private void StartLottery()
    {
        _appleCounter.gameObject.SetActive(false);
        _lotterySpawner.SpawnLottery();
    }

    private void OnGameEnd(bool isPlayerWin)
    {
        if (_giftHandler.HasGift)
        {
            _giftHandler.ShowGiftScreen(isPlayerWin);
        }
        else
        {
            EndGame(isPlayerWin, 1f);
        }
    }

    private void EndGame(bool isPlayerWin, float delay = 0f)
    {
        if (isPlayerWin)
        {
            WinGame(delay);
        }
        else
        {
            LoseGame(delay);
        }
    }

    private void WinGame(float delay)
    {
        int knifeReward;
        GameObject rewardTemplate;
        
        switch (DataManager.GameData.ProgressData.CurrentGamemod)
        {
            case 0:
                MetricaManager.SendEvent("target_lvl_complete_(" + DataManager.GameData.ProgressData.CurrentTargetLevel + ")");
                _player.DepositMoney(LevelManager.CurrentTargetLevel.Reward);
                knifeReward = LevelManager.CurrentTargetLevel.KnifeReward;
                rewardTemplate = LevelManager.CurrentTargetLevel.KnifeRewardTemplate;
                LevelManager.NextTargetLevel();
                break;
            case 1:
                MetricaManager.SendEvent("cube_lvl_start_(" + DataManager.GameData.ProgressData.CurrentTargetLevel + ")");
                _player.DepositMoney(LevelManager.CurrentCubeLevel.Reward);
                knifeReward = LevelManager.CurrentCubeLevel.KnifeReward;
                rewardTemplate = LevelManager.CurrentCubeLevel.KnifeRewardTemplate;
                LevelManager.NextCubeLevel();
                break;
            case 2:
                MetricaManager.SendEvent("flat_lvl_complete_(" + DataManager.GameData.ProgressData.CurrentTargetLevel + ")");
                _player.DepositMoney(LevelManager.CurrentFlatLevel.Reward);
                knifeReward = LevelManager.CurrentFlatLevel.KnifeReward;
                rewardTemplate = LevelManager.CurrentFlatLevel.KnifeRewardTemplate;
                LevelManager.NextFlatLevel();
                break;
            default:
                MetricaManager.SendEvent("target_lvl_complete_(" + DataManager.GameData.ProgressData.CurrentTargetLevel + ")");
                _player.DepositMoney(LevelManager.CurrentTargetLevel.Reward);
                knifeReward = LevelManager.CurrentTargetLevel.KnifeReward;
                rewardTemplate = LevelManager.CurrentTargetLevel.KnifeRewardTemplate;
                LevelManager.NextTargetLevel();
                break;
        }

        _winScreen.Win(_appleCounter.Count >= 3, knifeReward, rewardTemplate, delay);
    }

    private void OnLotteryWin(List<RewardNames> rewards)
    {
        _lotteryScreen.SendReward(rewards);
    }
    
    private void LoseGame(float delay)
    {
        switch (DataManager.GameData.ProgressData.CurrentGamemod)
        {
            case 0:
                MetricaManager.SendEvent("target_lvl_fail_(" + DataManager.GameData.ProgressData.CurrentTargetLevel + ")");
                break;
            case 1:
                MetricaManager.SendEvent("cube_lvl_fail_(" + DataManager.GameData.ProgressData.CurrentCubeLevel + ")");
                break;
            case 2:
                MetricaManager.SendEvent("flat_lvl_fail_(" + DataManager.GameData.ProgressData.CurrentFlatLevel + ")");
                break;
            default:
                MetricaManager.SendEvent("target_lvl_fail_(" + DataManager.GameData.ProgressData.CurrentTargetLevel + ")");
                break;
        }
        
        _loseScreen.Lose(delay);
    }
}
