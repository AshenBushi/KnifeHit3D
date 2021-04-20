using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PageIndicator : MonoBehaviour
{
    [SerializeField] private Sprite _enable;
    [SerializeField] private Sprite _disable;
    [SerializeField] private Image _image;
    
    private Tween _tween;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void EnablePoint()
    {
        _image.sprite = _enable;
    }

    public void DisablePoint()
    {
        _image.sprite = _disable;
    }
}