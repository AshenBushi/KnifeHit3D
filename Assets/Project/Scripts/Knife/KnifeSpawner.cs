using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnifeSpawner : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private List<Knife> _knives;

    private Knife _currentKnife;

    public Knife CurrentTemplate => _knives[DataManager.GameData.ShopData.CurrentKnifeIndex];

    public event UnityAction IsLose;

    private void OnDisable()
    {
        if (_currentKnife == null) return;
        _currentKnife.IsStuck -= SpawnKnife;
        _currentKnife.IsBounced -= OnKnifeBounced;
    }
    
    private void OnKnifeBounced()
    {
        IsLose?.Invoke();
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
        
        _currentKnife.Throw();
        _currentKnife = null;
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
