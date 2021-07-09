#pragma warning disable 0649

using UnityEngine;

namespace Watermelon
{
    [SetupTab("Init Settings", priority = 1, texture = "icon_puzzle")]
    [CreateAssetMenu(fileName = "Project Init Settings", menuName = "Settings/Project Init Settings")]
    public class ProjectInitSettings : ScriptableObject
    {
        [SerializeField] InitModule[] initModules;

        private Initialiser Initialiser;

        private bool isInitialized = false;
        public bool IsInititalized
        {
            get { return isInitialized; }
        }

        public void Init(Initialiser Initialiser)
        {
            if (isInitialized)
            {
                Debug.LogError("[Initialiser]: Project is already initialized!");

                return;
            }

            this.Initialiser = Initialiser;

            for(int i = 0; i < initModules.Length; i++)
            {
                initModules[i].CreateComponent(Initialiser);
            }

            isInitialized = true;
        }

        public void Destroy()
        {
            isInitialized = false;
        }
    }
}

// -----------------
// Initialiser v 0.3
// -----------------