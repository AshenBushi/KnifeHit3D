using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
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
