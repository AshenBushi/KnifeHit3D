using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLoader : MonoBehaviour
{
    [SerializeField] private Slider _sliderProgressBar;
    [SerializeField] private TextMeshProUGUI _textProgress;

    private AsyncOperation _operation;

    private void Start()
    {
        _operation = SceneManager.LoadSceneAsync(1);
    }

    private void Update()
    {
        float progressValue = Mathf.Clamp01(_operation.progress / 0.9f);
        _sliderProgressBar.value = progressValue * 100;
        _textProgress.text = Mathf.Round(progressValue * 100) + "%";
    }

    /*private IEnumerator LoadAsync()
    {
        _operation = SceneManager.LoadSceneAsync(1);
        _operation.allowSceneActivation = false;

        yield return new WaitForSeconds(4f);
        
        if (AdManager.Instance.ShowInterstitial())
        {
            AdManager.Instance.Interstitial.OnAdClosed += HandleOnAdClosed;
            yield break;
        }

        _operation.allowSceneActivation = true;
    }

    private void HandleOnAdClosed(object sender, EventArgs args)
    {
        AdManager.Instance.Interstitial.OnAdClosed -= HandleOnAdClosed;
        _operation.allowSceneActivation = true;
    }*/
}
