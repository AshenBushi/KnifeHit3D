using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AppleSpawner : MonoBehaviour
{
    [SerializeField] private Apple _template;
    
    public Apple SpawnApple()
    {
        return Instantiate(_template, transform);
    }
}
