using UnityEngine;
using Watermelon;

public class OpenScreenButton : SettingsButtonBase
{
    [SerializeField] private UIScreen _screen;
    
    public override bool IsActive()
    {
        return true;
    }

    public override void OnClick()
    {
        SoundManager.Instance.PlaySound(SoundName.ButtonClick);
        
        _screen.Enable();
    }
}
