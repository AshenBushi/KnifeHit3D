using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SoundNames
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

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _soundPlayer;
    [SerializeField] private AudioSource _musicPlayer;

    [SerializeField] private List<AudioClip> _audioClips;

    private static List<AudioClip> _aClips;
    private static AudioSource _soundP;
    private static AudioSource _musicP;

    public static AudioSource SoundPlayer => _soundP;
    public static AudioSource MusicPlayer => _musicP;

    private void Awake()
    {
        _soundP = _soundPlayer;
        _musicP = _musicPlayer;
        _aClips = _audioClips;
    }

    public static void PlaySound(SoundNames soundName)
    {
        _soundP.PlayOneShot(_aClips[(int) soundName]);
    }
}
