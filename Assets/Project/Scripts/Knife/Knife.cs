﻿using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Knife : MonoBehaviour
{
    [SerializeField] private float _throwForce;
    [SerializeField] private float _bounceForce;

    private Rigidbody _rigidbody;

    public Rigidbody Rigidbody => _rigidbody;

    public event UnityAction IsStuck;
    public event UnityAction IsBounced;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Knife knife))
        {
            Bounced();
        }
    }

    private void Bounced()
    {
        if (_rigidbody.isKinematic) return;
        Destroy(GetComponent<Collider>());
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.useGravity = true;
        _rigidbody.AddForce(Vector3.back * _bounceForce, ForceMode.Impulse);
        IsBounced?.Invoke();
    }

    public void Stuck(Transform parent)
    {
        _rigidbody.isKinematic = true;
        transform.SetParent(parent);
        IsStuck?.Invoke();
    }
    
    public void Throw()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(Vector3.forward * _throwForce, ForceMode.Impulse);
    }
}
