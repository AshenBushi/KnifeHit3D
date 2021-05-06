using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasGroup))]
public class WinScreen : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private IEnumerator WinAnimation()
    {
        _canvasGroup.blocksRaycasts = true;
        yield return new WaitForSeconds(1f);

        Time.timeScale = 0;
        
        _canvasGroup.interactable = true;
        _canvasGroup.alpha = 1;
    }

    public void Win()
    {
        StartCoroutine(WinAnimation());
    }
    
    public void Restart()
    {
        SoundManager.PlaySound(SoundNames.ButtonClick);
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneBuildIndex: 0);
    }
}
