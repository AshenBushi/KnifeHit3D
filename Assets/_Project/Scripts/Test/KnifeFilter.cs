using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KnifeFilter : MonoBehaviour
{
    private List<Transform> _knives;
    
    private void Start()
    {
        _knives = GetComponentsInChildren<Transform>().ToList();

        for (var i = 0; i < _knives.Count; i++)
        {
            _knives[i].position = new Vector3(i, 0f, 0f);
        }
    }
}
