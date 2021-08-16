using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    [SerializeField] private List<string> _sceneNames;

    private AsyncOperation _operation;
    private int _lastLoadSceneIndex = -1;
    private bool _isScenePrepearing = false;

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
        /*if (_isScenePrepearing) return;
        _operation = SceneManager.LoadSceneAsync(sceneIndex);
        _operation.allowSceneActivation = false;
        _isScenePrepearing = true;*/
    }

    public void LoadPreparedScene()
    {
        SceneManager.LoadScene(1);
        
        /*_isScenePrepearing = false;
        _operation.allowSceneActivation = true;*/
    }
}
