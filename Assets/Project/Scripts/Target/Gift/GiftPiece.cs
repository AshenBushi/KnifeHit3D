using System;
using UnityEngine;
using UnityEngine.Events;

public class GiftPiece : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Collider _collider;

    public event UnityAction IsSliced;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Knife knife)) return;
        SoundManager.PlaySound(SoundNames.GiftHit);
        Slice();
    }

    private void Slice()
    {
        _rigidbody.isKinematic = false;
        _collider.isTrigger = false;
        IsSliced?.Invoke();
    }
}
