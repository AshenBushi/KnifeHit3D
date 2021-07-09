using UnityEngine;

namespace Watermelon
{
    [SetupTab("Audio", texture = "icon_audio")]
    [CreateAssetMenu(fileName = "Audio Settings", menuName = "Settings/Audio Settings")]
    public class AudioSettings : ScriptableObject
    {
        [System.Serializable]
        public class Sounds
        {
            [Tooltip("Platform destruct sound")]
            public AudioClip platformDestructAudioClip;
            [Tooltip("Platform bounce sound")]
            public AudioClip platformBounceAudioClip;

            [Space]
            [Tooltip("Player hit sound")]
            public AudioClip playerHitAudioClip;

            [Space]
            [Tooltip("Game over sound")]
            public AudioClip gameOverAudioClip;
            [Tooltip("Game win sound")]
            public AudioClip gameWinAudioClip;
        }

        [System.Serializable]
        public class Vibrations
        {
            public int shortVibration;
            public int longVibration;
        }
        
        public bool isMusicEnabled = true;
        public bool isAudioEnabled = true;
        public bool isVibrationEnabled = true;

        public AudioClip[] musicAudioClips;
        
        public Sounds sounds;
        public Vibrations vibrations;
    }
}

// -----------------
// Audio Controller v 0.3
// -----------------