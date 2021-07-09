using System.Collections;
using System.Collections.Generic;
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
        _screen.Enable();
    }
}
