using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLoader : MonoBehaviour
{
    private AsyncOperation _operation;

    private float _loadProgress = 0f, _mobileAdProgress = 0f, _saveProgress = 0f;

    private void Start()
    {
        StartCoroutine(LoadAsync());
    }
    
    private IEnumerator LoadAsync()
    {
        yield return new WaitForSeconds(1f);
        
        _operation = SceneManager.LoadSceneAsync(1);
        _operation.allowSceneActivation = false;

        while (_loadProgress < 1f)
        {
            /*if (AdManager.Interstitial.IsLoaded())
            {
                _mobileAdProgress = .33f;
            }*/

            if (DataManager.Loaded())
            {
                _saveProgress = .5f;
            }
            
            _loadProgress = (_operation.progress / 0.9f * 0.5f)  + _saveProgress;
            yield return null;
        }

        _operation.allowSceneActivation = true;
    }
}
