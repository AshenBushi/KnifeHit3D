using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GamemodHandler : MonoBehaviour
{
    [SerializeField] private List<Button> _buttons;
    [SerializeField] private float _duration;

    [Header("Background Settings")] 
    [SerializeField] private Image _background;
    [SerializeField] private List<Sprite> _backgroundSprites;
    [Header("Sizes")]
    [SerializeField] private Vector3 _selected;
    [SerializeField] private Vector3 _unselected;

    private Tween _tween;

    public event UnityAction IsModChanged;

    private void Start()
    {
        SelectMod(DataManager.GameData.ProgressData.CurrentGamemod, 0f);
    }

    private void SelectMod(int index, float duration)
    {
        SelectBackground(index);
        
        for (var i = 0; i < _buttons.Count; i++)
        {
            _tween = _buttons[i].transform.DOScale(i == index ? _selected : _unselected, duration);
        }
    }

    private void SelectBackground(int index)
    {
        _background.sprite = _backgroundSprites[index];
    }
    
    public void SelectMod(int index)
    {
        SoundManager.PlaySound(SoundNames.ButtonClick);
        DataManager.GameData.ProgressData.CurrentGamemod = index;
        DataManager.Save();
        
        SelectBackground(index);
        
        for (var i = 0; i < _buttons.Count; i++)
        {
            _tween = _buttons[i].transform.DOScale(i == index ? _selected : _unselected, _duration);
        }
        
        IsModChanged?.Invoke();
    }
}
