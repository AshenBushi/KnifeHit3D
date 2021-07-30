using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerInput : Singleton<PlayerInput>, IPointerDownHandler
{
    private bool _canTap = true;
    private bool _isSessionStart = false;
    
    public event UnityAction IsTapped;
    public event UnityAction IsSessionStart;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_canTap) return;
        
        if (!_isSessionStart)
        {
            IsSessionStart?.Invoke();
            _isSessionStart = true;
        }
        
        IsTapped?.Invoke();
    }

    public void AllowTap()
    {
        _canTap = true;
    }

    public void DisallowTap()
    {
        _canTap = false;
    }
}
