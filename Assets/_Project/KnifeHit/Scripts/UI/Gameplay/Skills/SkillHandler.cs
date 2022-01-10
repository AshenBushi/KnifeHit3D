using System.Collections;
using UnityEngine;

public class SkillHandler : MonoBehaviour
{
    [SerializeField] private SkillSlowMode _slowMode;
    [SerializeField] private SkillSkipTargetAd _skipTarget;
    [SerializeField] private SkillLevelPass _levelPass;
    [SerializeField] private SkillSecondLife _secondChance;

    private bool _canEnableSkills = true;

    private void OnEnable()
    {
        SessionHandler.Instance.IsSessionStarted += OnSessionStarted;
        SessionHandler.Instance.IsSessionRestarted += OnSessionRestarted;
    }

    private void OnDisable()
    {
        SessionHandler.Instance.IsSessionStarted -= OnSessionStarted;
        SessionHandler.Instance.IsSessionRestarted -= OnSessionRestarted;
    }

    private void OnSessionStarted()
    {
        if (!_canEnableSkills) return;

        StartCoroutine(TryEnableButtons());
    }

    private void OnSessionRestarted()
    {
        _skipTarget.gameObject.SetActive(false);
    }

    private IEnumerator TryEnableButtons()
    {
        yield return new WaitForSeconds(2f);

        _skipTarget.Show();
        _slowMode.Show();
        _secondChance.Show();
        _levelPass.Show();
    }

    public void AllowSkills()
    {
        _canEnableSkills = true;
    }

    public void DisallowSkills()
    {
        _canEnableSkills = false;
    }
}
