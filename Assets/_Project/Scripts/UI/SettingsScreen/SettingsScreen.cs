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
        SoundManager.SoundPlayer.volume = DataManager.GameData.SettingsData.SoundVolume;
        SoundManager.MusicPlayer.volume = DataManager.GameData.SettingsData.MusicVolume;
        UpdateButtonImages();
    }

    private void UpdateButtonImages()
    {
        _sound.image.sprite = SoundManager.SoundPlayer.volume > 0 ? _buttonOn : _buttonOff;
        _music.image.sprite = SoundManager.MusicPlayer.volume > 0 ? _buttonOn : _buttonOff;
    }
    
    private void ChangeSoundVolume()
    {
        SoundManager.SoundPlayer.volume = SoundManager.SoundPlayer.volume > 0 ? 0 : 1;
        SoundManager.PlaySound(SoundName.ButtonClick);
        DataManager.GameData.SettingsData.SoundVolume = SoundManager.SoundPlayer.volume;
        DataManager.Save();
        UpdateButtonImages();
    }

    private void ChangeMusicVolume()
    {
        SoundManager.MusicPlayer.volume = SoundManager.MusicPlayer.volume > 0 ? 0 : 1;
        SoundManager.PlaySound(SoundName.ButtonClick);
        DataManager.GameData.SettingsData.MusicVolume = SoundManager.MusicPlayer.volume;
        DataManager.Save();
        UpdateButtonImages();
    }

    public override void Disable()
    {
        base.Disable();
        SoundManager.PlaySound(SoundName.ButtonClick);
    }
}
