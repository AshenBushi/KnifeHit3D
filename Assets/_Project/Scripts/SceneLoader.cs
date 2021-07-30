using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private List<string> _sceneNames;
    
    private int _lastLoadSceneIndex = -1;

    public void TryLoadScene(int sceneIndex)
    {
        if (_lastLoadSceneIndex == sceneIndex) return;

        if(_lastLoadSceneIndex == -1)
        {
            SceneManager.LoadSceneAsync(_sceneNames[sceneIndex], LoadSceneMode.Additive);
        }
        else
        {
            for (var i = 0; i < _sceneNames.Count; i++)
            {
                if (i == sceneIndex)
                {
                    SceneManager.LoadSceneAsync(_sceneNames[i], LoadSceneMode.Additive);
                }
                else
                {
                    SceneManager.UnloadSceneAsync(_sceneNames[i]);
                }
            }
        }
        
        _lastLoadSceneIndex = sceneIndex;
    }
}
