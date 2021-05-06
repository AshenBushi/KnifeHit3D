using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasGroup))]


public class WinScreen : MonoBehaviour

{
    [SerializeField] private GameObject _cup;
    
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private IEnumerator WinAnimation()
    {
        _canvasGroup.blocksRaycasts = true;
        yield return new WaitForSeconds(1f);
        
        _canvasGroup.interactable = true;
        _canvasGroup.alpha = 1;
        
        _cup.SetActive(true);
    }

    public void Win()
    {
        StartCoroutine(WinAnimation());
    }

    public void Restart()
    {
        SceneManager.LoadScene(sceneBuildIndex: 0);
    }
}
