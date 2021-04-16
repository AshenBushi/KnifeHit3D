using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Target : MonoBehaviour
{
    private TargetBase _targetBase;
    private TargetRotator _rotator;
    private Collider _collider;
    private int _health;
    
    public int Health => _health;

    public event UnityAction IsTakeHit;
    public event UnityAction<TargetBase> IsBreak;

    private void Awake()
    {
        _rotator = GetComponent<TargetRotator>();
        _collider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.TryGetComponent(out Knife knife)) return;
        knife.Stuck(_targetBase.transform);
        TakeHit();
    }

    private void TakeHit()
    {
        _health--;
        
        if(_health <= 0)
            Break();
        else
            IsTakeHit?.Invoke();
    }

    private void Break()
    {
        IsBreak?.Invoke(Instantiate(_targetBase, _targetBase.transform.position, _targetBase.transform.rotation));
        Destroy(gameObject);
    }
    
    public void SpawnAndSetup(TargetBase target, int health)
    {
        _targetBase = Instantiate(target, transform.position, Quaternion.Euler(0f, 180f, 0f), transform);
        _health = health;
    }
}
