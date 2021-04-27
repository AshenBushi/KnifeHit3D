using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartScreen : MonoBehaviour
{
    [SerializeField] private ShopScreen _shopScreen;

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
