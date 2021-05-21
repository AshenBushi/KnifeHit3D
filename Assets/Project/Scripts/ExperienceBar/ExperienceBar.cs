using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class ExperienceBar : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private CanvasGroup _canvasGroup;
    private Slider _slider;

    private Tween _tween;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _slider = GetComponent<Slider>();
    }

    private void Start()
    {
        _slider.value = DataManager.GameData.PlayerData.Experience;
    }

    private IEnumerator AddExpAnimation(int addedValue)
    {
        _tween = _canvasGroup.DOFade(1f, 1f);
        
        _tween.OnComplete(() =>
        {
            _tween = _canvasGroup.DOFade(0f, 1f);
        });
        
        _text.text = "+" + addedValue.ToString();
        
        while ((int) _slider.value != DataManager.GameData.PlayerData.Experience)
        {
            _slider.value++;

            if (_slider.value >= 100)
            {
                _slider.value -= 100;
            }
            
            yield return new WaitForSeconds(1f / addedValue);
        }
    }

    public void ShowExpBar(int addedValue)
    {
        StartCoroutine(AddExpAnimation(addedValue));
    }
}
