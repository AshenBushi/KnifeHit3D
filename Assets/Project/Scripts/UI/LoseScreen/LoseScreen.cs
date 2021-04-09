using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(CanvasGroup))]
public class LoseScreen : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private IEnumerator LoseAnimation()
    {
        _canvasGroup.blocksRaycasts = true;
        yield return new WaitForSeconds(1f);

        Time.timeScale = 0;
        
        _canvasGroup.interactable = true;
        _canvasGroup.alpha = 1;
    }

    public void Lose()
    {
        StartCoroutine(LoseAnimation());
    }
    
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneBuildIndex: 0);
    }
}
