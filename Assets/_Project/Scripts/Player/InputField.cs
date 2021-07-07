﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputField : MonoBehaviour, IPointerDownHandler
{
    private bool _isSessionStart = false;
    
    public event UnityAction IsTapped;
    public event UnityAction IsSessionStart;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_isSessionStart)
        {
            IsSessionStart?.Invoke();
            _isSessionStart = true;
        }
        
        IsTapped?.Invoke();
    }
}