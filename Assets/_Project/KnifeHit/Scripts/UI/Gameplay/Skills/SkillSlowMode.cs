using System.Collections;
using TMPro;
using UnityEngine;

public class SkillSlowMode : Skill
{
    [SerializeField] private TMP_Text _textNumCount;
    [SerializeField] private TMP_Text _timer;

    protected override void OnEnable()
    {
        base.OnEnable();
        ChangeButtonSprite();
    }

    protected override void ChangeButtonSprite()
    {
        _textNumCount.text = DataManager.Instance.GameData.PlayerData.SlowMode.ToString();

        if (DataManager.Instance.GameData.PlayerData.SlowMode <= 0)
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
        if (DataManager.Instance.GameData.PlayerData.SlowMode > 0)
        {
            DataManager.Instance.GameData.PlayerData.SlowMode--;
            DataManager.Instance.Save();
            _button.interactable = false;
            _button.image.color = Color.grey;
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
        _timer.text = "";

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                timer = 0f;
                _timer.text = timer.ToString("#.##");
                break;
            }

            _timer.text = timer.ToString("#.##");
            yield return null;
        }

        _button.interactable = true;
        _button.image.color = Color.white;
        TargetHandler.Instance.DisableSlowMode();
        ChangeButtonSprite();
    }

}
