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
    Lottery
}

public class GamemodHandler : MonoBehaviour
{
    [SerializeField] private List<Button> _buttons;
    [SerializeField] private float _duration;
    [SerializeField] private Animator _cameraAnimator;
    
    [Header("Sizes")]
    [SerializeField] private Vector3 _selected;
    [SerializeField] private Vector3 _unselected;

    private Tween _tween;
    private GamemodName _currentGamemod;
    private int _cameraViewIndex = 0;

    public static GamemodName CurrentGamemod;

    public event UnityAction IsModChanged;

    private void Awake()
    {
        SelectRandomMod();
    }

    private void SelectRandomMod()
    {
        var randomIndex = Random.Range(0, 3);

        var randomMod = (GamemodName)randomIndex;
        
        SelectMod(randomMod, 0f);
    }

    private void ChangeButtonSize(float duration)
    {
        if (_currentGamemod == GamemodName.Lottery)
        {
            foreach (var button in _buttons)
            {
                _tween = button.transform.DOScale(_unselected, duration);
            }

            return;
        }
        
        for (var i = 0; i < _buttons.Count; i++)
        {
            _tween = _buttons[i].transform.DOScale(i == (int) _currentGamemod + 3 * _cameraViewIndex ? _selected : _unselected, duration);
        }
    }
    
    private void SelectMod(GamemodName gamemodName, float duration)
    {
        _currentGamemod = gamemodName;
        CurrentGamemod = _currentGamemod;

        ChangeButtonSize(duration);
    }
    
    public void SelectMod(int gamemodIndex)
    {
        SoundManager.PlaySound(SoundName.ButtonClick);

        _currentGamemod = (GamemodName)gamemodIndex;
        CurrentGamemod = _currentGamemod;
        
        ChangeButtonSize(_duration);
        
        IsModChanged?.Invoke();
    }
    
    public void TryChangeCameraView(int cameraViewIndex)
    {
        if (_cameraViewIndex == cameraViewIndex) return;

        switch (cameraViewIndex)
        {
            case 0:
                _cameraAnimator.SetTrigger("EnableFirstView");
                break;
            case 1:
                _cameraAnimator.SetTrigger("EnableSecondView");
                break;
        }

        _cameraViewIndex = cameraViewIndex;
    }
}
