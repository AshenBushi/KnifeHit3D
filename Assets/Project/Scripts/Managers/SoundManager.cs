using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private List<AudioClip> _audioClips;

    private static List<AudioClip> _aClips;
    private static AudioSource _soundP;

    private void Awake()
    {
        _soundP = _soundPlayer;
        _aClips = _audioClips;
    }

    public static void PlaySound(SoundNames soundName)
    {
        _soundP.PlayOneShot(_aClips[(int) soundName]);
    }
}
