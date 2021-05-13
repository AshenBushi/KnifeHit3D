using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Knife : MonoBehaviour
{
    private const float ThrowForce = 40f;
    private const float BounceForce = 10f;
    
    [SerializeField] private Vector3 _obstacleRotation;
    [SerializeField] private GameObject _stuckEffect;

    private Rigidbody _rigidbody;

    private bool _isBounced = false;
    private bool _isStuck = false;

    public event UnityAction IsStuck;
    public event UnityAction IsBounced;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.TryGetComponent(out Knife knife)) return;
        
        Bounced(knife.transform.position);
            
        foreach (var point in other.contacts)
        {
            Instantiate(_stuckEffect, point.point, Quaternion.identity);
        }
    }

    private void Bounced(Vector3 position)
    {
        if (_isStuck) return;
        SoundManager.PlaySound(SoundNames.ObstacleHit);
        _isBounced = true;
        Destroy(GetComponent<Collider>());
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.useGravity = true;
        _rigidbody.AddExplosionForce(BounceForce, position, 5, 0, ForceMode.Impulse);
        IsBounced?.Invoke();
    }

    public void Stuck(Transform parent)
    {
        if (_isBounced || _isStuck) return;
        _isStuck = true;
        _rigidbody.isKinematic = true;
        transform.SetParent(parent);
        gameObject.layer = LayerMask.NameToLayer("Obstacle");
        IsStuck?.Invoke();
    }
    
    public void Throw()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(Vector3.forward * ThrowForce, ForceMode.Impulse);
        SoundManager.PlaySound(SoundNames.KnifeThrow);
    }

    public void MakeObstacle()
    {
        _rigidbody.isKinematic = true;
        gameObject.layer = LayerMask.NameToLayer("Obstacle");
        transform.localRotation = Quaternion.Euler(_obstacleRotation);
    }
}
