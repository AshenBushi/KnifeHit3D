using UnityEngine;
using UnityEngine.UI;

public class PageIndicator : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _enabled;
    [SerializeField] private Sprite _disabled;
    

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void EnablePoint()
    {
        _image.sprite = _enabled;
    }

    public void DisablePoint()
    {
        _image.sprite = _disabled;
    }
}