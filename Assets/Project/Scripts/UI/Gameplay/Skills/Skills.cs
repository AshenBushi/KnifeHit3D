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
        _sessionHandler.IsGameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        _sessionHandler.IsGameStarted -= OnGameStarted;
    }

    private void OnGameStarted()
    {
        StartCoroutine(TryEnableButtons());
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
