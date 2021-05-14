using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoseScreen : UIScreen
{
    [SerializeField] private GameObject _adPanel;
    [SerializeField] private GameObject _cupPanel;
    [SerializeField] private GameObject _cup;

    private List<Rigidbody> _cupPieces;

    private void Awake()
    {
        _cupPieces = _cup.GetComponentsInChildren<Rigidbody>().ToList();
    }

    private IEnumerator LoseAnimation()
    {
        CanvasGroup.blocksRaycasts = true;
        yield return new WaitForSeconds(1f);

        CanvasGroup.interactable = true;
        CanvasGroup.alpha = 1;
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
        SceneManager.LoadScene(sceneBuildIndex: 0);
    }
}
