using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PageIndicator : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Color _enabled;
    [SerializeField] private Color _disabled;
    

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void EnablePoint()
    {
        _image.color = _enabled;
    }

    public void DisablePoint()
    {
        _image.color = _disabled;
    }
}