using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class RewardScreen : UIScreen
{
    [SerializeField] private Transform _preview;

    private Animator _animator;
    private GameObject _reward;
    
    public event UnityAction IsScreenDisabled;

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        _animator = GetComponent<Animator>();
    }

    public void ShowReward(int index)
    {
        Enable();

        _reward = Instantiate(KnifeStorage.KnifePreviews[index], _preview);
        _animator.SetTrigger("ShowReward");
    }

    public override void Disable()
    {
        base.Disable();
        Destroy(_reward);
        IsScreenDisabled?.Invoke();
    }
}
