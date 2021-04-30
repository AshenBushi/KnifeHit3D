using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    [SerializeField] private ShopScreen _shopScreen;
    [SerializeField] private Slider _levelSelector;

    private GamemodHandler _gamemodHandler;
    private CanvasGroup _canvasGroup;

    public event UnityAction IsModChanged;

    private void Awake()
    {
        _gamemodHandler = GetComponentInChildren<GamemodHandler>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        _gamemodHandler.IsModChanged += OnModChanged;
    }

    private void OnDisable()
    {
        _gamemodHandler.IsModChanged -= OnModChanged;
    }

    private void OnModChanged()
    {
        IsModChanged?.Invoke();
    }

    public void EnableShopScreen()
    {
        _shopScreen.EnableScreen();
    }
    
    public void StartSession()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }
}
