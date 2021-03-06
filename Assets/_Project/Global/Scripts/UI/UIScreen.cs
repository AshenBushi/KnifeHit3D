using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreen : MonoBehaviour
{
    protected CanvasGroup CanvasGroup;

    public virtual void Enable()
    {
        CanvasGroup.blocksRaycasts = true;
        CanvasGroup.interactable = true;
        CanvasGroup.alpha = 1;
    }

    public virtual void Disable()
    {
        CanvasGroup.blocksRaycasts = false;
        CanvasGroup.interactable = false;
        CanvasGroup.alpha = 0;
    }
}
