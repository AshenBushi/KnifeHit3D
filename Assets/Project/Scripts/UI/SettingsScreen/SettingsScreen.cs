using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScreen : MonoBehaviour
{
    [SerializeField] private Button _sound;
    [SerializeField] private Button _music;
    [SerializeField] private AudioSource _soundPlayer;
    [SerializeField] private AudioSource _musicPlayer;
    [SerializeField] private Sprite _buttonOn;
    [SerializeField] private Sprite _buttonOff;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
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
        _soundPlayer.volume = DataManager.GameData.SettingsData.SoundVolume;
        _musicPlayer.volume = DataManager.GameData.SettingsData.MusicVolume;
        UpdateButtonImages();
    }

    private void UpdateButtonImages()
    {
        _sound.image.sprite = _soundPlayer.volume > 0 ? _buttonOn : _buttonOff;
        _music.image.sprite = _musicPlayer.volume > 0 ? _buttonOn : _buttonOff;
    }
    
    private void ChangeSoundVolume()
    {
        _soundPlayer.volume = _soundPlayer.volume > 0 ? 0 : 1;
        SoundManager.PlaySound(SoundNames.ButtonClick);
        DataManager.GameData.SettingsData.SoundVolume = _soundPlayer.volume;
        DataManager.Save();
        UpdateButtonImages();
    }

    private void ChangeMusicVolume()
    {
        _musicPlayer.volume = _musicPlayer.volume > 0 ? 0 : 1;
        SoundManager.PlaySound(SoundNames.ButtonClick);
        DataManager.GameData.SettingsData.MusicVolume = _musicPlayer.volume;
        DataManager.Save();
        UpdateButtonImages();
    }

    public void EnableSettings()
    {
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
        _canvasGroup.alpha = 1;
    }
    
    public void DisableSettings()
    {
        SoundManager.PlaySound(SoundNames.ButtonClick);
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
        _canvasGroup.alpha = 0;
    }
}
