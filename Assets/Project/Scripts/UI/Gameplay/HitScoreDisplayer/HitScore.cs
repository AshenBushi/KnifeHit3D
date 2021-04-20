using UnityEngine;
using UnityEngine.UI;

public class HitScore : MonoBehaviour
{
    [SerializeField] private Sprite _scoreOn;

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void TurnOn()
    {
        _image.sprite = _scoreOn;
    }
}
