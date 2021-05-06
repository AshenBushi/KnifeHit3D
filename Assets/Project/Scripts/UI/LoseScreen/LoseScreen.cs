using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(CanvasGroup))]
public class LoseScreen : MonoBehaviour
{
    [SerializeField] private GameObject _adPanel;
    [SerializeField] private GameObject _cupPanel;
    [SerializeField] private GameObject _cup;

    private List<Rigidbody> _cupPieces;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _cupPieces = _cup.GetComponentsInChildren<Rigidbody>().ToList();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private IEnumerator LoseAnimation()
    {
        _canvasGroup.blocksRaycasts = true;
        yield return new WaitForSeconds(1f);

        _canvasGroup.interactable = true;
        _canvasGroup.alpha = 1;
    }

    private IEnumerator CupBreakAnimation()
    {
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
        SceneManager.LoadScene(sceneBuildIndex: 0);
    }
}
