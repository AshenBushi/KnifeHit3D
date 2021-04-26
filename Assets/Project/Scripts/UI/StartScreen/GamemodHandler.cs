using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GamemodHandler : MonoBehaviour
{
    [SerializeField] private List<Button> _buttons;
    [SerializeField] private float _duration;
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
        for (var i = 0; i < _buttons.Count; i++)
        {
            _tween = _buttons[i].transform.DOScale(i == index ? _selected : _unselected, duration);
        }
    }
    
    public void SelectMod(int index)
    {
        DataManager.GameData.ProgressData.CurrentGamemod = index;
        DataManager.Save();
        
        for (var i = 0; i < _buttons.Count; i++)
        {
            _tween = _buttons[i].transform.DOScale(i == index ? _selected : _unselected, _duration);
        }
        
        IsModChanged?.Invoke();
    }
}
