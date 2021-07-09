#pragma warning disable 0649

using UnityEngine;

namespace Watermelon
{
    public class AudioControllerInitModule : InitModule
    {
        [SerializeField] AudioSettings audioSettings;

        public override void CreateComponent(Initialiser Initialiser)
        {
            AudioController audioController = new AudioController();
            audioController.Init(audioSettings, Initialiser.gameObject);
        }

        public AudioControllerInitModule()
        {
            moduleName = "Audio Controller";
        }
    }
}

// -----------------
// Audio Controller v 0.3
// -----------------