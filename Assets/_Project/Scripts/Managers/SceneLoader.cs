using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    private AsyncOperation _operation;

    protected override void Awake()
    {
        if (Instance != null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator LoadScene(int sceneIndex)
    {
        _operation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

        yield return new WaitUntil(() => _operation.isDone);
    }

    public IEnumerator UnloadScene(int sceneIndex)
    {
        _operation = SceneManager.UnloadSceneAsync(sceneIndex);
        
        yield return new WaitUntil(() => _operation.isDone);
    }
}
