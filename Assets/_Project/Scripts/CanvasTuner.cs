using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasTuner : MonoBehaviour
{
    private Canvas _canvas;
    
    private void Start()
    {
        _canvas = GetComponent<Canvas>();

        _canvas.worldCamera = FindObjectOfType<CameraMover>().Camera;
    }
}
