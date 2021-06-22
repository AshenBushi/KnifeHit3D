using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GamemodHandler : MonoBehaviour
{
    [SerializeField] private List<Button> _buttons;
    [SerializeField] private float _duration;
    [SerializeField] private Animator _cameraAnimator;

    [Header("Background Settings")] 
    [SerializeField] private Image _background;
    [SerializeField] private List<Sprite> _backgroundSprites;
    [Header("Sizes")]
    [SerializeField] private Vector3 _selected;
    [SerializeField] private Vector3 _unselected;

    private Tween _tween;
    private int _cameraViewIndex = 1;

    public event UnityAction IsModChanged;

    private void Start()
    {
        SelectMod(DataManager.GameData.ProgressData.CurrentGamemod, 0f);
    }

    private void SelectBackground(int index)
    {
        _background.sprite = _backgroundSprites[index];
    }

    private void TryChangeCameraView(int index)
    {
        if (index < 3)
        {
            if (_cameraViewIndex == 1) return;
            _cameraAnimator.SetTrigger("EnableFirstView");
            _cameraViewIndex = 1;
        }
        else
        {
            if (_cameraViewIndex == 2) return;
            _cameraAnimator.SetTrigger("EnableSecondView");
            _cameraViewIndex = 2;
        }
    }
    
    private void SelectMod(int index, float duration)
    {
        SelectBackground(index % 3);
        TryChangeCameraView(index);

        for (var i = 0; i < _buttons.Count; i++)
        {
            _tween = _buttons[i].transform.DOScale(i == index ? _selected : _unselected, duration);
        }
    }

    public void SelectMod(int index)
    {
        SoundManager.PlaySound(SoundNames.ButtonClick);
        DataManager.GameData.ProgressData.CurrentGamemod = index % 3;
        DataManager.Save();
        
        SelectBackground(index % 3);
        TryChangeCameraView(index);
        
        for (var i = 0; i < _buttons.Count; i++)
        {
            _tween = _buttons[i].transform.DOScale(i == index ? _selected : _unselected, _duration);
        }
        
        IsModChanged?.Invoke();
    }
}
