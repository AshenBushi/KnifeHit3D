using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Apple : MonoBehaviour
{
    [SerializeField] private GameObject _slicedApple;
    [SerializeField] private GameObject _sliceEffect;

    public event UnityAction IsSliced;
    
    private void Update()
    {
        var position = transform.position;
        transform.LookAt(new Vector3(position.x, position.y, position.z + 1f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Knife knife)) return;
        SoundManager.PlaySound(SoundNames.AppleHit);
        StartCoroutine(Sliced());
    }
    
    private IEnumerator Sliced()
    {
        var slicedApple = Instantiate(_slicedApple, transform.position, transform.rotation);

        Instantiate(_sliceEffect, transform.position, Quaternion.identity);
        
        IsSliced?.Invoke();
        
        Destroy(gameObject);

        yield return new WaitForSeconds(2f);
        
        Destroy(slicedApple);
    }
}
