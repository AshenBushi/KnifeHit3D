using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GamemodManager : Singleton<GamemodManager>
{
    [SerializeField] private SceneLoader _sceneLoader;
    
    public int LastPressedButtonIndex { get; private set; }
    
    public event UnityAction IsButtonIndexChanged;

    private void Start()
    {
        SelectMod(0);
        SetButtonIndex(Random.Range(0, 6));
    }

    public void SelectMod(int index)
    {
        _sceneLoader.TryLoadScene(index);
    }

    public void SetButtonIndex(int index)
    {
        if (LastPressedButtonIndex == index) return;
        
        LastPressedButtonIndex = index;
        IsButtonIndexChanged?.Invoke();
    }
}

public enum Gamemod
{
    KnifeHit,
    StackKnife
}
