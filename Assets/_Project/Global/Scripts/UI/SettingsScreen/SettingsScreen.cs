using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScreen : UIScreen
{
    [SerializeField] private Button _sound;
    [SerializeField] private Button _music;
    [SerializeField] private Sprite _buttonOn;
    [SerializeField] private Sprite _buttonOff;

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        _sound.onClick.AddListener(ChangeSoundVolume);
        _music.onClick.AddListener(ChangeMusicVolume);
    }

    private void OnDisable()
    {
        _sound.onClick.RemoveListener(ChangeSoundVolume);
        _music.onClick.RemoveListener(ChangeMusicVolume);
    }

    private void Start()
    {
        SoundManager.Instance.SoundPlayer.volume = DataManager.Instance.GameData.SettingsData.SoundVolume;
        SoundManager.Instance.MusicPlayer.volume = DataManager.Instance.GameData.SettingsData.MusicVolume;
        UpdateButtonImages();
    }

    private void UpdateButtonImages()
    {
        _sound.image.sprite = SoundManager.Instance.SoundPlayer.volume > 0 ? _buttonOn : _buttonOff;
        _music.image.sprite = SoundManager.Instance.MusicPlayer.volume > 0 ? _buttonOn : _buttonOff;
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
