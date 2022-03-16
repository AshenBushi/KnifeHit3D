using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContinueScreen : UIScreen
{
    [SerializeField] private LoseScreen _loseScreen;
    [SerializeField] private GameObject _noThanks;
    [SerializeField] private List<TMP_Text> _textLose = new List<TMP_Text>();

    public bool IsEnable { get; private set; }

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    public override void Enable()
    {
        base.Enable();
        IsEnable = true;
        StartCoroutine(SetLoseTextEnable());
        StartCoroutine(LoseAnimation());
    }

    public override void Disable()
    {
        base.Disable();
        IsEnable = false;
    }

    public void FailLevel()
    {
        _loseScreen.Lose();
        Disable();
    }

    private IEnumerator LoseAnimation()
    {
        _noThanks.SetActive(false);

        yield return new WaitForSeconds(4f);

        _noThanks.SetActive(true);
    }

    private IEnumerator SetLoseTextEnable()
    {
        for (int i = 0; i < _textLose.Count; i++)
        {
            _textLose[i].DOFade(1f, 1.5f).SetLink(gameObject);
            yield return new WaitForSeconds(1.5f);
        }
    }
}
