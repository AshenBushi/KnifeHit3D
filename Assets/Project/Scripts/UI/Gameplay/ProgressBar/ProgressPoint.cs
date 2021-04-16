using UnityEngine;
using UnityEngine.UI;

public class ProgressPoint : MonoBehaviour
{
    [SerializeField] private Sprite _pointOn;

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void TurnOn()
    {
        _image.sprite = _pointOn;
    }
}
