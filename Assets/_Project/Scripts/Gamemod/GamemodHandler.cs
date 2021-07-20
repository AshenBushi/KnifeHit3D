using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum GamemodName
{
    Mark,
    Cube,
    Flat,
    StackKnife,
    Lottery
}

public class GamemodHandler : Singleton<GamemodHandler>
{
    [SerializeField] private CameraMover _cameraMover;
    [SerializeField] private GamemodButtons _gamemodButtons;

    public GamemodName CurrentGamemod { get; private set; }

    public event UnityAction IsModChanged;

    private void Start()
    {
        SelectRandomMod();
    }

    private void SelectRandomMod()
    {
        var randomIndex = Random.Range(0, 3);

        var randomMod = (GamemodName)randomIndex;
        
        SelectMod(randomMod, 0f);
    }

    private void SelectAndLoadScene()
    {
        
    }
    
    private void SelectMod(GamemodName gamemodName, float duration)
    {
        CurrentGamemod = gamemodName;

        _gamemodButtons.ChangeButtonSize(gamemodName, _cameraMover.CurrentPointIndex, duration);
        
        IsModChanged?.Invoke();
    }
    
    public void SelectMod(int gamemodIndex)
    {
        SoundManager.PlaySound(SoundName.ButtonClick);

        CurrentGamemod = (GamemodName)gamemodIndex;

        _gamemodButtons.ChangeButtonSize(CurrentGamemod, _cameraMover.CurrentPointIndex);
        
        IsModChanged?.Invoke();
    }
}
