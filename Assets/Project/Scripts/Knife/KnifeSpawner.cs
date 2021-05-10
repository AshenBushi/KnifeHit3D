using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnifeSpawner : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TargetSpawner _targetSpawner;
    [SerializeField] private List<Knife> _knives;
    
    private Knife _currentKnife;
    private int _knifeAmount = 0;
    
    public Knife CurrentTemplate => _knives[DataManager.GameData.ShopData.CurrentKnifeIndex];

    public event UnityAction IsLose;

    private void OnEnable()
    {
        _targetSpawner.IsNewTargetSet += OnNewTargetSet;
    }

    private void OnDisable()
    {
        _targetSpawner.IsNewTargetSet -= OnNewTargetSet;
        if (_currentKnife == null) return;
        _currentKnife.IsStuck -= SpawnKnife;
        _currentKnife.IsBounced -= OnKnifeBounced;
    }
    
    private void OnKnifeBounced()
    {
        IsLose?.Invoke();
    }

    private void OnNewTargetSet(int amount)
    {
        _knifeAmount = amount;
    }
    
    public void SpawnKnife()
    {
        if (_currentKnife != null) return;
        _currentKnife = Instantiate(_knives[DataManager.GameData.ShopData.CurrentKnifeIndex], _player.transform);
        _currentKnife.IsStuck += SpawnKnife;
        _currentKnife.IsBounced += OnKnifeBounced;
    }

    public void ThrowKnife()
    {
        if (_currentKnife == null)
        {
            SpawnKnife();
        }

        _knifeAmount--;
        _currentKnife.Throw();
        _currentKnife = null;

        if (_knifeAmount < 1)
        {
            _player.DisallowThrow();
        }
    }
    
    public void Reload()
    {
        _currentKnife.IsStuck -= SpawnKnife;
        _currentKnife.IsBounced -= OnKnifeBounced;
        Destroy(_currentKnife.gameObject);
        _currentKnife = null;
        SpawnKnife();
    }
}
