using UnityEngine;
using UnityEngine.Events;

public class Knife : MonoBehaviour
{
    private const float ThrowForce = 40f;
    private const float BounceForce = 10f;
    
    [SerializeField] private GameObject _stuckEffect;
    [SerializeField] private Vector3 _obstacleRotation;

    private Rigidbody _rigidbody;
    private bool _isCollided = false;

    public event UnityAction IsStuck;
    public event UnityAction IsBounced;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_isCollided) return;

        if (other.gameObject.TryGetComponent(out Target target))
        {
            Stuck(target.Base.transform);
            target.TakeHit();
            
            _isCollided = true;
        }
        
        if (other.gameObject.TryGetComponent(out LotterySection section))
        {
            Stuck(section.transform);
            section.TakeReward();
            
            _isCollided = true;
        }

        if (other.gameObject.TryGetComponent(out Knife knife))
        {
            Bounced(knife.transform.position);
            
            foreach (var point in other.contacts)
            {
                Instantiate(_stuckEffect, point.point, Quaternion.identity);
            }
            
            _isCollided = true;
        }
    }

    public void MakeDefaultObstacle(Transform parent)
    {
        gameObject.layer = LayerMask.NameToLayer("Obstacle");
        _rigidbody.isKinematic = true;
        transform.SetParent(parent);
        transform.localRotation = Quaternion.Euler(_obstacleRotation);
    }

    public void Throw()
    {
        SoundManager.Instance.PlaySound(SoundName.KnifeThrow);
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(Vector3.forward * ThrowForce, ForceMode.Impulse);
    }

    private void MakeBounced(Vector3 position)
    {
        gameObject.layer = LayerMask.NameToLayer("Bounced");
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.AddExplosionForce(BounceForce, position, 5, 0, ForceMode.Impulse);
    }
    
    private void Bounced(Vector3 position)
    {
        if (gameObject.layer == LayerMask.NameToLayer("Bounced")) return;
        
        SoundManager.Instance.PlaySound(SoundName.ObstacleHit);
        MakeBounced(position);
        IsBounced?.Invoke();
    }

    private void Stuck(Transform parent)
    {
        SoundManager.Instance.PlaySound(SoundName.TargetHit);
        MakeObstacle(parent);
        IsStuck?.Invoke();
    }
    
    private void MakeObstacle(Transform parent)
    {
        gameObject.layer = LayerMask.NameToLayer("Obstacle");
        _rigidbody.isKinematic = true;
        transform.SetParent(parent);
    }
}
