using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Knife : MonoBehaviour
{
    [SerializeField] private float _throwForce;
    [SerializeField] private float _bounceForce;
    [SerializeField] private Vector3 _obstacleRotation;

    private Rigidbody _rigidbody;

    private bool _isBounced = false;

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
            Bounced(knife.transform.position);
        }
    }

    private void Bounced(Vector3 position)
    {
        if (_rigidbody.isKinematic) return;
        SoundManager.PlaySound(SoundNames.ObstacleHit);
        _isBounced = true;
        Destroy(GetComponent<Collider>());
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.useGravity = true;
        _rigidbody.AddExplosionForce(_bounceForce, position, 5, 0, ForceMode.Impulse);
        IsBounced?.Invoke();
    }

    public void Stuck(Transform parent)
    {
        if (_isBounced) return;
        _rigidbody.isKinematic = true;
        transform.SetParent(parent);
        gameObject.layer = LayerMask.NameToLayer("Obstacle");
        IsStuck?.Invoke();
    }
    
    public void Throw()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(Vector3.forward * _throwForce, ForceMode.Impulse);
        SoundManager.PlaySound(SoundNames.KnifeThrow);
    }

    public void MakeObstacle()
    {
        _rigidbody.isKinematic = true;
        gameObject.layer = LayerMask.NameToLayer("Obstacle");
        transform.localRotation = Quaternion.Euler(_obstacleRotation);
    }
}
