using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public Camera Camera { get; private set; }

    private void Awake()
    {
        Camera = GetComponent<Camera>();
    }
}
