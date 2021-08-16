using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class LoseScreen : UIScreen
{
    [SerializeField] private GameObject _noThanks;

    public event UnityAction<bool> IsScreenDisabled;

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    private IEnumerator LoseAnimation()
    {
        Enable();
        
        SoundManager.Instance.PlaySound(SoundName.Lose);

        yield return new WaitForSeconds(1f);
        
        _noThanks.SetActive(true);
    }

    private void EndGame()
    {
        IsScreenDisabled?.Invoke(false);
        DataManager.Instance.GameData.CurrentGamemod = -1;
    }

    public void Lose()
    {
        StartCoroutine(LoseAnimation());
    }

    public void SkipAd()
    {
        EndGame();
    }
}
