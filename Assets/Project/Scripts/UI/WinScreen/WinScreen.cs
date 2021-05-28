﻿using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class WinScreen : UIScreen
{
    [SerializeField] private GameObject _knifeRewardPanel;
    [SerializeField] private GameObject _adPanel;
    [SerializeField] private GameObject _preview;
    [SerializeField] private GameObject _cup;
    [SerializeField] private TMP_Text _rewardText;
    [SerializeField] private DoubleReward _doubleReward;
    
    private bool _isALottery = false;
    private bool _isShowedDoubleReward = false;

    public event UnityAction IsCanStartLottery;

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

    private IEnumerator WinAnimation(int knifeReward, GameObject template, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        Enable();

        if (knifeReward > 0)
        {
            DataManager.GameData.ShopData.OpenedKnives.Add(knifeReward);
            DataManager.GameData.ShopData.CurrentKnifeIndex = knifeReward;
            _knifeRewardPanel.SetActive(true);
            Instantiate(template, _preview.transform);
        }
        else
        {
            EnableAdPanel();
        }
    }
    
    private void HandleOnAdClosed(object sender, EventArgs e)
    {
        if (_isALottery)
        {
            MetricaManager.SendEvent("bns_lvl");
            MetricaManager.SendEvent("bns_lvl_start");
            Disable();
            IsCanStartLottery?.Invoke();
        }
        else
        {
            SceneManager.LoadScene(sceneBuildIndex: 1);
        }
    }

    private void EnableAdPanel()
    {
        _adPanel.SetActive(true);
        _cup.SetActive(true);

        _rewardText.text = DataManager.GameData.ProgressData.CurrentGamemod switch
        {
            0 => LevelManager.CurrentTargetLevel.Reward.ToString(),
            1 => LevelManager.CurrentCubeLevel.Reward.ToString(),
            2 => LevelManager.CurrentFlatLevel.Reward.ToString(),
            _ => LevelManager.CurrentTargetLevel.Reward.ToString()
        };
    }

    public override void Enable()
    {
        base.Enable();
        SoundManager.PlaySound(SoundNames.Win);
    }

    public override void Disable()
    {
        base.Disable();
        _cup.SetActive(false);
    }

    public void Win(bool isALottery, int knifeReward, GameObject template, float delay)
    {
        _isALottery = isALottery;
        StartCoroutine(WinAnimation(knifeReward, template, delay));
    }

    public void Continue()
    {
        SoundManager.PlaySound(SoundNames.ButtonClick);

        if (_isALottery)
        {
            MetricaManager.SendEvent("bns_lvl");
            MetricaManager.SendEvent("bns_lvl_start");
            Disable();
            IsCanStartLottery?.Invoke();
        }
        else
        {
            if (!_isShowedDoubleReward && AdManager.Interstitial.IsLoaded())
            {
                AdManager.Interstitial.OnAdClosed += HandleOnAdClosed;
                AdManager.ShowInterstitial();
            }
            else
            {
                SceneManager.LoadScene(sceneBuildIndex: 1);
            }
        }
        
    }

    public void Collect()
    {
        SoundManager.PlaySound(SoundNames.ButtonClick);
        _adPanel.SetActive(true);
        _cup.SetActive(true);
        _knifeRewardPanel.SetActive(false);
    }
}
