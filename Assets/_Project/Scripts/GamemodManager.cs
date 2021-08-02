using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GamemodManager : Singleton<GamemodManager>
{
    public int LastPressedButtonIndex { get; private set; }
    
    public event UnityAction IsButtonIndexChanged;

    private void Start()
    {
        StartSession();
    }

    public void StartSession()
    {
        SelectMod(0);
        SetButtonIndex(Random.Range(0, 6));
    }

    public void SelectMod(int index)
    {
        SceneLoader.Instance.TryLoadGameplayScene(index);
    }

    public void SetButtonIndex(int index)
    {
        if (LastPressedButtonIndex == index) return;
        
        SoundManager.Instance.PlaySound(SoundName.ButtonClick);
        
        LastPressedButtonIndex = index;
        IsButtonIndexChanged?.Invoke();
    }
}

public enum Gamemod
{
    KnifeHit,
    StackKnife
}
