using System.Collections.Generic;
using UnityEngine;

public enum SoundName
{
    TargetHit = 0,
    KnifeThrow,
    AppleHit,
    GiftHit,
    ObstacleHit,
    TargetBreak,
    Lose,
    Win,
    RandomBuy,
    GiftOpen,
    ButtonClick
}

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource _soundPlayer;
    [SerializeField] private AudioSource _musicPlayer;
    [SerializeField] private List<AudioClip> _audioClips;
    

    public AudioSource SoundPlayer => _soundPlayer;
    public AudioSource MusicPlayer => _musicPlayer;

    public void PlaySound(SoundName soundName)
    {
        _soundPlayer.PlayOneShot(_audioClips[(int) soundName]);
    }
}
