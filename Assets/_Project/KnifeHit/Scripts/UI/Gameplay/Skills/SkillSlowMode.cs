using System.Collections;
using TMPro;
using UnityEngine;

public class SkillSlowMode : Skill
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _timer;

    protected override void OnEnable()
    {
        base.OnEnable();
        ChangeButtonSprite();
    }

    protected override void ChangeButtonSprite()
    {
        if (DataManager.Instance.GameData.PlayerData.SlowMode <= 0)
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
        if (DataManager.Instance.GameData.PlayerData.SlowMode > 0)
        {
            DataManager.Instance.GameData.PlayerData.SlowMode--;
            DataManager.Instance.Save();
            StartCoroutine(SlowGame());
        }
        else
            SetNotificationInfo();
    }

    private IEnumerator SlowGame()
    {
        _timer.gameObject.SetActive(true);
        TargetHandler.Instance.EnableSlowMode();

        var timer = 5f;
        _text.text = "";

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            _timer.text = timer.ToString("#.##");
            yield return null;
        }

        TargetHandler.Instance.DisableSlowMode();
        ChangeButtonSprite();
    }

}
