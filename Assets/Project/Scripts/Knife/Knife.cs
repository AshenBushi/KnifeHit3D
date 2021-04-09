using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Knife : MonoBehaviour
{
    [SerializeField] private float _throwForce;
    [SerializeField] private float _bounceForce;

    private Rigidbody _rigidbody;

    public event UnityAction<bool> IsStuck;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Target target))
        {
            Stuck(target.transform);
        }

        if (other.gameObject.TryGetComponent(out Knife knife))
        {
            Stuck();
        }
    }

    private void Stuck()
    {
        if (_rigidbody.isKinematic) return;
        Destroy(GetComponent<Collider>());
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.useGravity = true;
        _rigidbody.AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), -1f) * _bounceForce, ForceMode.Impulse);
        IsStuck?.Invoke(false);
    }    
    
    private void Stuck(Transform target)
    {
        _rigidbody.isKinematic = true;
        transform.SetParent(target);
        IsStuck?.Invoke(true);
    }
    
    public void AllowMove()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(Vector3.forward * _throwForce, ForceMode.Impulse);
    }
}
