using System;
using System.Collections;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLoader : MonoBehaviour
{
    private AsyncOperation _operation;
    
    private float _passedTime = 0f;

    private void Start()
    {
        StartCoroutine(LoadAsync());
    }
    
    private IEnumerator LoadAsync()
    {
        _operation = SceneManager.LoadSceneAsync(1);
        _operation.allowSceneActivation = false;

        yield return new WaitForSeconds(0.5f);
        
        while (_passedTime <= 5f)
        {
            _passedTime += Time.deltaTime;

            if (AdManager.Interstitial.IsLoaded())
            {
                AdManager.Interstitial.OnAdClosed += HandleOnAdClosed;
                AdManager.ShowInterstitial();
                yield break;
            }
            
            yield return null;
        }
        
        _operation.allowSceneActivation = true;
    }

    private void HandleOnAdClosed(object sender, EventArgs args)
    {
        AdManager.Interstitial.OnAdClosed -= HandleOnAdClosed;
        _operation.allowSceneActivation = true;
    }

}
