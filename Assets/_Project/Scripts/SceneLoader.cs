using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    [SerializeField] private List<string> _sceneNames;

    private AsyncOperation _operation;
    private int _lastLoadSceneIndex = -1;

    public void TryLoadGameplayScene(int sceneIndex)
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

    public void PrepareScene(int sceneIndex)
    {
        _operation = SceneManager.LoadSceneAsync(sceneIndex);
        _operation.allowSceneActivation = false;
    }

    public void LoadPreparedScene()
    {
        _operation.allowSceneActivation = true;
    }
}
