using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class Background : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particlesBackground;
    private RawImage _image;

    private void Awake()
    {
        _image = GetComponent<RawImage>();
        _particlesBackground.Play();
    }

    private void OnEnable()
    {
        ColorManager.Instance.IsPresetChanged += OnPresetChanged;
    }

    private void OnDisable()
    {
        ColorManager.Instance.IsPresetChanged -= OnPresetChanged;
    }

    private void OnPresetChanged()
    {
        ColorManager.Instance.ColorizeImage(_image);
    }
}
