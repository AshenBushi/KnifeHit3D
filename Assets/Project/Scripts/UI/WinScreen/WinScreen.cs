using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasGroup))]


public class WinScreen : MonoBehaviour
{
    [SerializeField] private GameObject _cup;
    
    private CanvasGroup _canvasGroup;
    private bool _isALottery = false;

    public event UnityAction IsCanStartLottery;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private IEnumerator WinAnimation()
    {
        yield return new WaitForSeconds(1f);
        
        EnableScreen();
    }

    private void EnableScreen()
    {
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
        _canvasGroup.alpha = 1;
        _cup.SetActive(true);
    }
    
    private void DisableScreen()
    {
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
        _canvasGroup.alpha = 0;
        _cup.SetActive(false);
    }

    public void Win(bool isALottery)
    {
        StartCoroutine(WinAnimation());
        _isALottery = isALottery;
    }

    public void Continue()
    {
        SoundManager.PlaySound(SoundNames.ButtonClick);
        
        if (_isALottery)
        {
            DisableScreen();
            IsCanStartLottery?.Invoke();
        }
        else
        {
            SceneManager.LoadScene(sceneBuildIndex: 0);
        }
    }
}
