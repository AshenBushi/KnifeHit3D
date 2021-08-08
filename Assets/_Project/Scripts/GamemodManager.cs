using UnityEngine;
using UnityEngine.Events;

public class GamemodManager : Singleton<GamemodManager>
{
    [SerializeField] private Skills _skills;
    
    public int LastPressedButtonIndex { get; private set; }
    
    public event UnityAction IsButtonIndexChanged;
    public event UnityAction IsLotterySelected;

    private void Start()
    {
        StartSession();
    }

    private void StartSession()
    {
        SelectMod(0);
        SetButtonIndex(0);
    }

    public void SelectMod(int index)
    {
        SceneLoader.Instance.TryLoadGameplayScene(index);
        _skills.DisallowSkills();
    }

    public void SetButtonIndex(int index)
    {
        if (LastPressedButtonIndex == index) return;
        
        SoundManager.Instance.PlaySound(SoundName.ButtonClick);
        
        LastPressedButtonIndex = index;
        IsButtonIndexChanged?.Invoke();
        _skills.AllowSkills();
    }

    public void SelectLottery()
    {
        LastPressedButtonIndex = -1;
        IsLotterySelected?.Invoke();
        _skills.DisallowSkills();
    }
}
