using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSizeChanger : MonoBehaviour
{
    [SerializeField] private float _duration;
    
    [Header("Sizes")]
    [SerializeField] private Vector3 _selected;
    [SerializeField] private Vector3 _unselected;
    
    private List<Button> _buttons;
    private Tween _tween;

    private void Awake()
    {
        _buttons = GetComponentsInChildren<Button>().ToList();
    }

    private void OnEnable()
    {
        foreach (var button in _buttons)
        {
            button.onClick.AddListener(() => ChangeButtonsSize(button));
        }
    }

    private void OnDisable()
    {
        foreach (var button in _buttons)
        {
            button.onClick.RemoveListener(() => ChangeButtonsSize(button));
        }
    }

    private void ChangeButtonsSize(Button selectedButton)
    {
        foreach (var button in _buttons)
        {
            _tween = button.transform.DOScale(button == selectedButton ? _selected : _unselected, _duration);
        }
    }
}
