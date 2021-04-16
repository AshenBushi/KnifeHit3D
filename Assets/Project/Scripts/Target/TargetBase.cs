using UnityEngine;

public class TargetBase : MonoBehaviour
{
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _radius;
    [SerializeField] private float _upForce;

    public void Detonate()
    {
        foreach (var item in GetComponentsInChildren<Collider>())
        {
            item.isTrigger = false;
        }

        foreach (var item in GetComponentsInChildren<Rigidbody>())
        {
            item.isKinematic = false;
            item.AddExplosionForce(_explosionForce, transform.position, _radius, _upForce);
        }
    }
    
}
