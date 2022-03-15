using UnityEngine;

public class SkillSecondLife : Skill
{
    //[SerializeField] private ContinueScreen _continueScreen;

    protected override void OnEnable()
    {
        base.OnEnable();
        ChangeButtonSprite();
    }

    protected override void ChangeButtonSprite()
    {
        if (DataManager.Instance.GameData.PlayerData.SecondLife <= 0)
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
        if (DataManager.Instance.GameData.PlayerData.SecondLifeEnabled) return;

        if (DataManager.Instance.GameData.PlayerData.SecondLife > 0)
        {
            DataManager.Instance.GameData.PlayerData.SecondLife--;
            DataManager.Instance.GameData.PlayerData.SecondLifeEnabled = true;
            DataManager.Instance.Save();
            ChangeButtonSprite();
        }
        else
            SetNotificationInfo();
    }
}
