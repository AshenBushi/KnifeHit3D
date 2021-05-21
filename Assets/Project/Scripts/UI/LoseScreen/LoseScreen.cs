using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoseScreen : UIScreen
{
    [SerializeField] private GameObject _adPanel;
    [SerializeField] private GameObject _noThanks;
    [SerializeField] private GameObject _cupPanel;
    [SerializeField] private GameObject _cup;

    private List<Rigidbody> _cupPieces;

    private void Awake()
    {
        _cupPieces = _cup.GetComponentsInChildren<Rigidbody>().ToList();
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    private IEnumerator LoseAnimation(float delay)
    {
        CanvasGroup.blocksRaycasts = true;
        yield return new WaitForSeconds(delay);

        CanvasGroup.interactable = true;
        CanvasGroup.alpha = 1;

        yield return new WaitForSeconds(1f);
        _noThanks.SetActive(true);
    }

    private IEnumerator CupBreakAnimation()
    {
        SoundManager.PlaySound(SoundNames.Lose);
        
        yield return new WaitForSeconds(.5f);

        foreach (var piece in _cupPieces)
        {
            piece.isKinematic = false;
        }
    }

    public override void Disable()
    {
        base.Disable();
        _cupPanel.SetActive(false);
    }

    public void Lose(float delay)
    {
        StartCoroutine(LoseAnimation(delay));
    }

    public void SkipAd()
    {
        _adPanel.SetActive(false);
        _cupPanel.SetActive(true);
        StartCoroutine(CupBreakAnimation());
    }

    public void Restart()
    {
        SoundManager.PlaySound(SoundNames.ButtonClick);
        
        if (AdManager.Interstitial.IsLoaded())
        {
            AdManager.Interstitial.OnAdClosed += HandleOnAdClosed;
            AdManager.ShowInterstitial();
        }
        else
        {
            SceneManager.LoadScene(sceneBuildIndex: 1);
        }
    }

    private void HandleOnAdClosed(object sender, EventArgs e)
    {
        AdManager.Interstitial.OnAdClosed -= HandleOnAdClosed;
        SceneManager.LoadScene(sceneBuildIndex: 1);
    }
}
