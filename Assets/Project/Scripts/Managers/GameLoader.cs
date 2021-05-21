using System;
using System.Collections;
using GoogleMobileAds.Api;
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
        _operation = SceneManager.LoadSceneAsync(1);
        _operation.allowSceneActivation = false;

        yield return new WaitForSeconds(0.5f);
        
        while (_loadProgress < 1f)
        {
            if (AdManager.Interstitial.IsLoaded())
            {
                _mobileAdProgress = .33f;
            }

            if (DataManager.Loaded())
            {
                _saveProgress = 33f;
            }
            
            _loadProgress = (_operation.progress / 0.9f * 0.34f)  + _saveProgress + _mobileAdProgress;
            yield return null;
        }

        if (AdManager.Interstitial.IsLoaded())
        {
            AdManager.Interstitial.OnAdClosed += HandleOnAdClosed;
            AdManager.ShowInterstitial();
        }
        else
        {
            _operation.allowSceneActivation = true;
        }
    }

    private void HandleOnAdClosed(object sender, EventArgs args)
    {
        AdManager.Interstitial.OnAdClosed -= HandleOnAdClosed;
        _operation.allowSceneActivation = true;
    }

}
