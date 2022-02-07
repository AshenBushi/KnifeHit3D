using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SkillSkipTargetAd : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(Skip);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(Skip);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Skip()
    {
        AdManager.Instance.ShowRewardVideo();

        TargetHandler.Instance.CurrentTarget.BreakTarget();
        Hide();
    }
}
