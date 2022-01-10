using UnityEngine;

public class SkillSecondLife : Skill
{
    [SerializeField] private ContinueScreen _continueScreen;

    protected override void OnEnable()
    {
        base.OnEnable();
        ChangeButtonSprite();
    }

    protected override void ChangeButtonSprite()
    {
        if (DataManager.Instance.GameData.PlayerData.SecondLife <= 0)
        {
            _button.image.sprite = _button.spriteState.disabledSprite;
            _button.spriteState = _disableState;
        }
        else
        {
            _button.image.sprite = _normalSprite;
            _button.spriteState = _defaultState;
        }
    }

    protected override void ActivateSkill()
    {
        if (DataManager.Instance.GameData.PlayerData.SecondLife > 0)
        {
            DataManager.Instance.GameData.PlayerData.SecondLife--;
            ChangeButtonSprite();
            DataManager.Instance.Save();
            _continueScreen.Disable();
            KnifeHandler.Instance.SecondLife();
        }
        else
            SetNotificationInfo();
    }
}
