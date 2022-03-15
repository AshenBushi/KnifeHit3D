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
            _button.interactable = false;
            _button.image.color = Color.grey;
        }
        else
        {
            _button.interactable = true;
            _button.image.color = Color.white;
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
