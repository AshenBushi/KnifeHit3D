using System.Collections;
using UnityEngine;

public class ContinueScreen : UIScreen
{
    [SerializeField] private LoseScreen _loseScreen;
    [SerializeField] private GameObject _continue;
    [SerializeField] private GameObject _noThanks;

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    public override void Enable()
    {
        base.Enable();
        StartCoroutine(LoseAnimation());
    }

    public void FailLevel()
    {
        _loseScreen.Lose();
        Disable();
    }

    private IEnumerator LoseAnimation()
    {
        _noThanks.SetActive(false);

        yield return new WaitForSeconds(1f);

        _noThanks.SetActive(true);
    }
}
