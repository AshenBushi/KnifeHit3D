using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class NotificationScreen : Singleton<NotificationScreen>
{
    [SerializeField] private TMP_Text _textInfo;
    [SerializeField] private TMP_Text _textLotteryInfo;
    private CanvasGroup _canvasGroup;

    protected override void Awake()
    {
        base.Awake();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Enable()
    {
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
        _canvasGroup.DOFade(1f, 0.5f).OnComplete(Disable).SetLink(gameObject);
    }

    private void Disable()
    {
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
        _canvasGroup.DOFade(0f, 0.5f).SetDelay(1f).SetLink(gameObject);
    }

    public void SetNotify(string text)
    {
        _textInfo.text = text;
        Enable();
    }

    public void SetNotifyForSkills(string text)
    {
        _textInfo.text = text;
        _textLotteryInfo.text = DataManager.Instance.GameData.IsLotteryEnable == true ? "Lottery is available!" : "The lottery is not available yet!";
        Enable();
    }
}
