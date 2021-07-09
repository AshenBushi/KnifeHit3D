#pragma warning disable 0649

using UnityEngine;

namespace Watermelon
{
    public class PrefabInitModule : InitModule
    {
        [SerializeField] GameObject[] prefabs;

        public override void CreateComponent(Initialiser Initialiser)
        {
            for(int i = 0; i < prefabs.Length; i++)
            {
                if(prefabs[i])
                {
                    GameObject tempPrefab = Instantiate(prefabs[i]);
                    tempPrefab.transform.SetParent(Initialiser.transform);
                }
                else
                {
                    Debug.LogError("[Initialiser]: Custom prefab can't be null!");
                }
            }
        }

        public PrefabInitModule()
        {
            moduleName = "Custom Prefab Initialization";
        }
    }
}

// -----------------
// Initialiser v 0.3
// -----------------