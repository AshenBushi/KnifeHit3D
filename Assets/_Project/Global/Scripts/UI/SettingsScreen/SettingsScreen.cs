using UnityEngine;
using UnityEngine.UI;

public class SettingsScreen : UIScreen
{
    [SerializeField] private Button _buttonSound;
    [SerializeField] private Button _buttonMusic;

    [SerializeField] private Sprite _soundActiveIcon;
    [SerializeField] private Sprite _soundInactiveIcon;
    [SerializeField] private Sprite _musicActiveIcon;
    [SerializeField] private Sprite _musicInactiveIcon;
    [SerializeField] private Sprite _turnOn;
    [SerializeField] private Sprite _turnOff;

    private Image _soundIcon;
    private Image _musicIcon;

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        _soundIcon = _buttonSound.transform.GetChild(0).GetComponent<Image>();
        _musicIcon = _buttonMusic.transform.GetChild(0).GetComponent<Image>();
    }

    private void OnEnable()
    {
        _buttonSound.onClick.AddListener(ChangeSoundVolume);
        _buttonMusic.onClick.AddListener(ChangeMusicVolume);
    }

    private void OnDisable()
    {
        _buttonSound.onClick.RemoveListener(ChangeSoundVolume);
        _buttonMusic.onClick.RemoveListener(ChangeMusicVolume);
    }

    private void Start()
    {
        SoundManager.Instance.SoundPlayer.volume = DataManager.Instance.GameData.SettingsData.SoundVolume;
        SoundManager.Instance.MusicPlayer.volume = DataManager.Instance.GameData.SettingsData.MusicVolume;
        
        UpdateButtonImages();
    }

    private void UpdateButtonImages()
    {
        _buttonSound.image.sprite = SoundManager.Instance.SoundPlayer.volume > 0 ? _turnOn : _turnOff;
        _soundIcon.sprite = SoundManager.Instance.SoundPlayer.volume > 0 ? _soundActiveIcon : _soundInactiveIcon;

        _buttonMusic.image.sprite = SoundManager.Instance.MusicPlayer.volume > 0 ? _turnOn : _turnOff;
        _musicIcon.sprite = SoundManager.Instance.MusicPlayer.volume > 0 ? _musicActiveIcon : _musicInactiveIcon;
    }

    private void ChangeSoundVolume()
    {
        SoundManager.Instance.SoundPlayer.volume = SoundManager.Instance.SoundPlayer.volume > 0 ? 0 : 1;
        SoundManager.Instance.PlaySound(SoundName.ButtonClick);
        DataManager.Instance.GameData.SettingsData.SoundVolume = SoundManager.Instance.SoundPlayer.volume;
        DataManager.Instance.Save();
        UpdateButtonImages();
    }

    private void ChangeMusicVolume()
    {
        SoundManager.Instance.MusicPlayer.volume = SoundManager.Instance.MusicPlayer.volume > 0 ? 0 : 1;
        SoundManager.Instance.PlaySound(SoundName.ButtonClick);
        DataManager.Instance.GameData.SettingsData.MusicVolume = SoundManager.Instance.MusicPlayer.volume;
        DataManager.Instance.Save();
        UpdateButtonImages();
    }

    public override void Disable()
    {
        base.Disable();
        SoundManager.Instance.PlaySound(SoundName.ButtonClick);
    }
}
