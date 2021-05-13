using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AppleSpawner : MonoBehaviour
{
    private Apple _apple;

    public bool AppleSpawn { get; private set; } = false;
    public event UnityAction IsAppleSliced;
    
    private void OnDisable()
    {
        if (_apple == null) return;
        _apple.IsSliced -= OnAppleSliced;
    }

    private void OnAppleSliced()
    {
        IsAppleSliced?.Invoke();
    }
    
    public void SpawnApple(Apple appleTemplate)
    {
        if (_apple != null)
        {
            _apple.IsSliced -= OnAppleSliced;
            Destroy(_apple.gameObject);
        }

        _apple = Instantiate(appleTemplate, transform);
        _apple.IsSliced += OnAppleSliced;
        AppleSpawn = true;
    }
}
