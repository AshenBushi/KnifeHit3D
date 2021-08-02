﻿using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class WinScreen : UIScreen
{
    [SerializeField] private GameObject _cup;
    [SerializeField] private TMP_Text _rewardText;
    [SerializeField] private DoubleReward _doubleReward;
    
    private bool _isShowedDoubleReward = false;

    public event UnityAction<bool> IsScreenDisabled;

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        _doubleReward.IsWatchedReward += OnWatchedReward;
    }

    private void OnDisable()
    {
        _doubleReward.IsWatchedReward -= OnWatchedReward;
    }

    private void OnWatchedReward()
    {
        _isShowedDoubleReward = true;
    }

    public override void Enable()
    {
        base.Enable();
        SoundManager.Instance.PlaySound(SoundName.Win);
        _cup.SetActive(true);

        _rewardText.text = DataManager.Instance.GameData.ProgressData.CurrentGamemod switch
        {
            0 => LevelManager.Instance.CurrentMarkLevel.Reward.ToString(),
            1 => LevelManager.Instance.CurrentCubeLevel.Reward.ToString(),
            2 => LevelManager.Instance.CurrentFlatLevel.Reward.ToString(),
            _ => LevelManager.Instance.CurrentMarkLevel.Reward.ToString()
        };
    }

    public override void Disable()
    {
        base.Disable();
        _cup.SetActive(false);
        IsScreenDisabled?.Invoke(_isShowedDoubleReward);
    }

    public void Win()
    {
        Enable();
    }

    public void Continue()
    {
        SoundManager.Instance.PlaySound(SoundName.ButtonClick);
        
        Disable();
    }
}
