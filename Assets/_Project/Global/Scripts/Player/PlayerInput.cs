using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInput : Singleton<PlayerInput>, IPointerDownHandler, IPointerUpHandler
{
    private bool _canTap = true;
    private bool _isSessionStart = false;

    public event UnityAction IsTapped;
    public event UnityAction IsUntapped;
    public event UnityAction IsSessionStart;

    private void OnEnable()
    {
        SessionHandler.Instance.IsSessionRestarted += OnSessionRestarted;
    }

    private void OnDisable()
    {
        SessionHandler.Instance.IsSessionRestarted -= OnSessionRestarted;
    }

    private void OnSessionRestarted()
    {
        _isSessionStart = false;
        AllowTap();
    }

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

    public void OnClick()
    {
        if (!_canTap) return;

        if (!_isSessionStart)
        {
            IsSessionStart?.Invoke();
            _isSessionStart = true;
        }
    }

    public void AllowUsing()
    {
        GetComponent<Image>().raycastTarget = true;
    }

    public void DisallowUsing()
    {
        GetComponent<Image>().raycastTarget = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsUntapped?.Invoke();
    }

    public void AllowTap()
    {
        _canTap = true;
    }

    public void DisallowTap()
    {
        _canTap = false;
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
