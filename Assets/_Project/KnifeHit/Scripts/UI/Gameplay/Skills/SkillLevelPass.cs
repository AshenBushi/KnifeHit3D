using UnityEngine;

public class SkillLevelPass : Skill
{
    protected override void OnEnable()
    {
        base.OnEnable();
        ChangeButtonSprite();
    }

    protected override void ChangeButtonSprite()
    {
        if (DataManager.Instance.GameData.PlayerData.LevelPass <= 0)
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
        if (DataManager.Instance.GameData.PlayerData.LevelPass > 0)
        {
            DataManager.Instance.GameData.PlayerData.LevelPass--;
            ChangeButtonSprite();
            DataManager.Instance.Save();
            TargetHandler.Instance.CompleteLevel();
        }
        else
            SetNotificationInfo();
    }

}
