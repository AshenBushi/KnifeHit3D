using KnifeFest;
using UnityEngine;

public class KnifeFestManager : MonoBehaviour
{
    [SerializeField] private PathFollower _pathFollower;

    private void OnEnable()
    {
        SessionHandler.Instance.IsSessionStarted += OnSessionStarted;
    }

    private void OnDisable()
    {
        SessionHandler.Instance.IsSessionStarted -= OnSessionStarted;
    }

    private void OnSessionStarted()
    {
        PlayerInput.Instance.Disable();
        _pathFollower.AllowMove();
    }
}
