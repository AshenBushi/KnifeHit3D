using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    [SerializeField] private SlowMode _slowMode;
    [SerializeField] private SkipTarget _skipTarget;

    private bool _canEnableSkills = true;

    private void OnEnable()
    {
        SessionHandler.Instance.IsSessionStarted += OnSessionStarted;
        SessionHandler.Instance.IsSessionRestarted += OnSessionRestarted;
    }

    private void OnDisable()
    {
        SessionHandler.Instance.IsSessionStarted -= OnSessionStarted;
        SessionHandler.Instance.IsSessionRestarted += OnSessionRestarted;
    }

    private void OnSessionStarted()
    {
        StartCoroutine(TryEnableButtons());
    }
    
    private void OnSessionRestarted()
    {
        _skipTarget.gameObject.SetActive(false);
        _slowMode.gameObject.SetActive(false);
    }

    private IEnumerator TryEnableButtons()
    {
        yield return new WaitForSeconds(2f);

        if(!_canEnableSkills) yield break;
        
        if (DataManager.Instance.GameData.PlayerData.SlowMode > 0)
        {
            _slowMode.gameObject.SetActive(true);
        }

        _skipTarget.gameObject.SetActive(true);
    }

    public void AllowSkills()
    {
        _canEnableSkills = true;
        
        Debug.Log("Skills enabled");
    }

    public void DisallowSkills()
    {
        _canEnableSkills = false;
        
        Debug.Log("Skills disabled");
    }
}
