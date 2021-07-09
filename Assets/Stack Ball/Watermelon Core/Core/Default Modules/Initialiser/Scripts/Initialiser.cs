#pragma warning disable 0649

using UnityEngine;

namespace Watermelon
{
    [DefaultExecutionOrder(-999)]
    public class Initialiser : MonoBehaviour
    {
        [SerializeField] ProjectInitSettings initSettings;
        [SerializeField] Canvas systemCanvas;

        public static Canvas SystemCanvas;
        public static GameObject InitialiserGameObject;

        private void Awake()
        {
            if(!initSettings.IsInititalized)
            {
                SystemCanvas = systemCanvas;
                InitialiserGameObject = gameObject;

                DontDestroyOnLoad(gameObject);

                initSettings.Init(this); 
                
                Destroy(this); 
            }
            else
            {
                Debug.Log("[Initialiser]: Game is already initialized!");

                Destroy(gameObject);
            }
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoad]
        private static class InitCallbackHandler
        {
            public static ProjectInitSettings initSettings;

            static InitCallbackHandler()
            {
                UnityEditor.EditorApplication.playModeStateChanged += ModeChanged;
            }

            [UnityEditor.Callbacks.DidReloadScripts]
            private static void CreateAssetWhenReady() 
            {
                if ((UnityEditor.EditorApplication.isCompiling || UnityEditor.EditorApplication.isUpdating) && UnityEditor.EditorApplication.isPlaying)
                {
                    UnityEditor.EditorApplication.delayCall += delegate
                    {
                        initSettings = RuntimeEditorUtils.GetAssetByName<ProjectInitSettings>("Project Init Settings");
                          
                        if (initSettings != null)
                            initSettings.Destroy();
                    };

                    return;
                }
            }

            public static void ModeChanged(UnityEditor.PlayModeStateChange playModeState)
            {
                if(playModeState == UnityEditor.PlayModeStateChange.ExitingPlayMode)
                {
                    initSettings = RuntimeEditorUtils.GetAssetByName<ProjectInitSettings>("Project Init Settings");

                    if (initSettings != null)
                        initSettings.Destroy();
                }
            }
        }
#endif
    }
}

// -----------------
// Initialiser v 0.3
// -----------------

// Changelog
// v 0.3
// • Initializer renamed to Initialiser
// • Fixed problem with recompilation
// v 0.2
// • Added sorting feature
// • Initialiser MonoBehaviour will destroy after initialization
// v 0.1
// • Added basic version
