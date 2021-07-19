using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    [SerializeField] private SlowMode _slowMode;
    [SerializeField] private SkipTarget _skipTarget;

    private void OnEnable()
    {
        SessionHandler.Instance.IsSessionStarted += OnSessionStarted;
    }

    private void OnDisable()
    {
        SessionHandler.Instance.IsSessionStarted -= OnSessionStarted;
    }

    private void OnSessionStarted()
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
