using UnityEngine;
using UnityEngine.Events;

public class LoseScreen : UIScreen
{
    [SerializeField] private GameObject _continue;

    public event UnityAction<bool> IsScreenDisabled;

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    public override void Disable()
    {
        base.Disable();
        IsScreenDisabled?.Invoke(false);
    }

    public void Lose()
    {
        Enable();
    }
}
