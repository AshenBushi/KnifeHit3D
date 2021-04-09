using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnifeSpawner : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private List<Knife> _knives;
    [SerializeField] private int _knifeIndex;
    
    private Knife _currentKnife;
    public Knife CurrentKnife => _currentKnife;

    public event UnityAction IsLose;
    public event UnityAction IsStuck;


    private void Awake()
    {
        _currentKnife = Instantiate(_knives[_knifeIndex], _player.transform);
        _currentKnife.IsStuck += OnKnifeStuck;
    }

    private void OnDisable()
    {
        _currentKnife.IsStuck -= OnKnifeStuck;
    }

    private void OnKnifeStuck(bool isNotLose)
    {
        _currentKnife.IsStuck -= OnKnifeStuck;
        
        if (isNotLose)
        {
            _currentKnife = Instantiate(_knives[_knifeIndex], _player.transform);
            _currentKnife.IsStuck += OnKnifeStuck;
            IsStuck?.Invoke();
        }
        else
        {
            IsLose?.Invoke();
        }
    }
}
