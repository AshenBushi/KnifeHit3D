using UnityEngine;
using UnityEngine.Events;

public class GamemodManager : Singleton<GamemodManager>
{
    [SerializeField] private Skills _skills;
    
    public int LastPressedButtonIndex { get; private set; }
    public int CurrentModIndex { get; private set; }
    
    public event UnityAction IsButtonIndexChanged;
    public event UnityAction IsLotterySelected;

    private void Start()
    {
        StartSession();
    }

    private void StartSession()
    {
        if (DataManager.Instance.GameData.CurrentGamemod == -1)
        {
            SelectRandomMod();
        }
        else
        {
            SelectMod(DataManager.Instance.GameData.CurrentGamemod);
            
            if(DataManager.Instance.GameData.CurrentGamemod == 1) return;
            
            SetButtonIndex(DataManager.Instance.GameData.CurrentTargetType);
        }
    }

    private void SelectRandomMod()
    {
        var modIndex = Random.Range(0, 2);
        SelectMod(modIndex);
        
        if(modIndex == 1) return;
        
        var randomType = Random.Range(0, 6);
        SetButtonIndex(randomType);
    }

    public void SelectMod(int index)
    {
        CurrentModIndex = index;
        
        if(CurrentModIndex == 1)
            _skills.DisallowSkills();
        
        SceneLoader.Instance.TryLoadGameplayScene(index);
        DataManager.Instance.GameData.CurrentGamemod = CurrentModIndex;
    }

    public void SetButtonIndex(int index)
    {
        _skills.AllowSkills();
        
        if (LastPressedButtonIndex == index) return;
        
        SoundManager.Instance.PlaySound(SoundName.ButtonClick);
        
        LastPressedButtonIndex = index;
        IsButtonIndexChanged?.Invoke();
        DataManager.Instance.GameData.CurrentTargetType = LastPressedButtonIndex;
    }

    public void SelectLottery()
    {
        LastPressedButtonIndex = -1;
        IsLotterySelected?.Invoke();
        _skills.DisallowSkills();
    }
}
