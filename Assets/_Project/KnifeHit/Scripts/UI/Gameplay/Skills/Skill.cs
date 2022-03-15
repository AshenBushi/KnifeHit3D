using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class Skill : MonoBehaviour
{
    protected Button _button;

    protected virtual void Awake()
    {
        _button = GetComponent<Button>();
    }

    protected virtual void OnEnable()
    {
        _button.onClick.AddListener(ActivateSkill);
    }

    protected virtual void OnDisable()
    {
        _button.onClick.RemoveListener(ActivateSkill);
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    protected virtual void ChangeButtonSprite() { }

    protected virtual void SetNotificationInfo()
    {
        NotificationScreen.Instance.SetNotifyForSkills("There are no active boosts, you can get them in the lottery");
    }

    protected abstract void ActivateSkill();
}
