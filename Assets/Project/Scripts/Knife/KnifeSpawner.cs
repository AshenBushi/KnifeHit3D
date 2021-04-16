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

    private void OnDisable()
    {
        _currentKnife.IsStuck -= SpawnKnife;
        _currentKnife.IsBounced -= OnKnifeBounced;
    }
    
    private void OnKnifeBounced()
    {
        IsLose?.Invoke();
    }
    
    public void SpawnKnife()
    {
        if (_currentKnife != null)
        {
            _currentKnife.IsStuck -= SpawnKnife;
            _currentKnife.IsBounced -= OnKnifeBounced;
        }
        _currentKnife = Instantiate(_knives[_knifeIndex], _player.transform);
        _currentKnife.IsStuck += SpawnKnife;
        _currentKnife.IsBounced += OnKnifeBounced;
    }
}
