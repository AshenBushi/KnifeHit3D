using DG.Tweening;
using System.Collections;
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
        _slider.value = DataManager.Instance.GameData.PlayerData.Experience;
    }

    private IEnumerator AddExpAnimation(int addedValue)
    {
        _tween = _canvasGroup.DOFade(1f, 1f).SetLink(gameObject);
        
        _tween.OnComplete(() =>
        {
            _tween = _canvasGroup.DOFade(0f, 1f).SetLink(gameObject);
        });
        
        _text.text = "+" + addedValue.ToString();
        
        while ((int) _slider.value != DataManager.Instance.GameData.PlayerData.Experience)
        {
            _slider.value++;

            if (_slider.value >= 200)
            {
                _slider.value -= 200;
            }
            
            yield return new WaitForSeconds(1f / addedValue);
        }
    }

    public void ShowExpBar(int addedValue)
    {
        StartCoroutine(AddExpAnimation(addedValue));
    }
}
