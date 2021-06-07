using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RewardScreen : UIScreen
{
    public event UnityAction IsScreenDisabled;
    
    public override void Enable()
    {
        base.Enable();
    }

    public override void Disable()
    {
        base.Disable();
        IsScreenDisabled?.Invoke();
    }
}
