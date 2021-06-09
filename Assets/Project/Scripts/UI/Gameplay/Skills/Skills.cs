using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    [SerializeField] private SessionHandler _sessionHandler;
    [SerializeField] private SlowMode _slowMode;
    [SerializeField] private SkipTarget _skipTarget;

    private void OnEnable()
    {
        _sessionHandler.IsSessionStarted += OnSessionStarted;
        _sessionHandler.IsLotteryStarted += OnLotteryStarted;
    }

    private void OnDisable()
    {
        _sessionHandler.IsSessionStarted -= OnSessionStarted;
        _sessionHandler.IsLotteryStarted += OnLotteryStarted;
    }

    private void OnSessionStarted()
    {
        StartCoroutine(TryEnableButtons());
    }
    
    private void OnLotteryStarted()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator TryEnableButtons()
    {
        yield return new WaitForSeconds(2f);

        if (DataManager.GameData.PlayerData.SlowMode > 0)
        {
            _slowMode.gameObject.SetActive(true);
        }

        _skipTarget.gameObject.SetActive(true);
    }
}
