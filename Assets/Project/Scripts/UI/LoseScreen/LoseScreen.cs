using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class LoseScreen : UIScreen
{
    [SerializeField] private GameObject _adPanel;
    [SerializeField] private GameObject _noThanks;
    [SerializeField] private GameObject _cupPanel;
    [SerializeField] private GameObject _cup;

    private List<Rigidbody> _cupPieces;
    
    public event UnityAction<bool> IsScreenDisabled;

    private void Awake()
    {
        _cupPieces = _cup.GetComponentsInChildren<Rigidbody>().ToList();
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    private IEnumerator LoseAnimation()
    {
        Enable();

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

    private void EndGame()
    {
        Disable();
        IsScreenDisabled?.Invoke(false);
    }

    public IEnumerator LotteryLose()
    {
        yield return new WaitForSeconds(1f);
        
        Enable();
        
        _adPanel.SetActive(false);
        _cupPanel.SetActive(true);
        
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

    public void Lose()
    {
        StartCoroutine(LoseAnimation());
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
        EndGame();
    }

    
}
