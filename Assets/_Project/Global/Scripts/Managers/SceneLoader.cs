using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    [SerializeField] private List<string> _sceneNames;

    private AsyncOperation _operation;
    private int _lastLoadSceneIndex = -1;

    public void LoadGamemodScene(int sceneIndex)
    {
        if(_lastLoadSceneIndex == -1)
        {
            SceneManager.LoadSceneAsync(_sceneNames[sceneIndex], LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.UnloadSceneAsync(_sceneNames[_lastLoadSceneIndex]);
            SceneManager.LoadSceneAsync(_sceneNames[sceneIndex], LoadSceneMode.Additive);
        }
        
        _lastLoadSceneIndex = sceneIndex;
    }

    public void LoadPreparedScene()
    {
        SceneManager.LoadScene(1);
        
        /*_isScenePrepearing = false;
        _operation.allowSceneActivation = true;*/
    }
}
