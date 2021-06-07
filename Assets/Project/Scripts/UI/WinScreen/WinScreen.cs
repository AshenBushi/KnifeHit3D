using System;
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

    public event UnityAction IsScreenDisabled;

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

    private void HandleOnAdClosed(object sender, EventArgs e)
    {
        Disable();
    }

    public override void Enable()
    {
        base.Enable();
        SoundManager.PlaySound(SoundNames.Win);
        _cup.SetActive(true);

        _rewardText.text = DataManager.GameData.ProgressData.CurrentGamemod switch
        {
            0 => LevelManager.CurrentMarkLevel.Reward.ToString(),
            1 => LevelManager.CurrentCubeLevel.Reward.ToString(),
            2 => LevelManager.CurrentFlatLevel.Reward.ToString(),
            _ => LevelManager.CurrentMarkLevel.Reward.ToString()
        };
    }

    public override void Disable()
    {
        base.Disable();
        _cup.SetActive(false);
        IsScreenDisabled?.Invoke();
    }

    public void Win()
    {
        Enable();
    }

    public void Continue()
    {
        SoundManager.PlaySound(SoundNames.ButtonClick);
        
        if (!_isShowedDoubleReward && AdManager.Interstitial.IsLoaded())
        {
            AdManager.Interstitial.OnAdClosed += HandleOnAdClosed;
            AdManager.ShowInterstitial();
        }
        else
        {
            Disable();
        }
    }
}
