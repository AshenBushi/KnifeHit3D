﻿using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerInput : Singleton<PlayerInput>, IPointerDownHandler
{
    private bool _canTap = true;
    private bool _isSessionStart = false;
    
    public event UnityAction IsTapped;
    public event UnityAction IsSessionStart;

    private void OnEnable()
    {
        SessionHandler.Instance.IsSessionRestarted += OnSessionRestarted;
    }

    private void OnDisable()
    {
        SessionHandler.Instance.IsSessionRestarted -= OnSessionRestarted;
    }
    
    private void OnSessionRestarted()
    {
        _isSessionStart = false;
        AllowTap();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_canTap) return;
        
        if (!_isSessionStart)
        {
            IsSessionStart?.Invoke();
            _isSessionStart = true;
        }
        
        IsTapped?.Invoke();
    }

    public void AllowTap()
    {
        _canTap = true;
    }

    public void DisallowTap()
    {
        _canTap = false;
    }
}
