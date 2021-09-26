using KnifeFest;
using UnityEngine;
using UnityEngine.Events;

public class KnifeFestManager : MonoBehaviour
{
    [SerializeField] private PathFollower _pathFollower;
    [SerializeField] private GameObject _loadingScreen;

    public static UnityAction IsLoadingData, IsLoadingDataComplete;

    private void OnEnable()
    {
        IsLoadingData += ActivationLoadingScreen;
        IsLoadingDataComplete += DeactivationLoadingScreen;
        SessionHandler.Instance.IsSessionStarted += OnSessionStarted;
    }

    private void OnDisable()
    {
        IsLoadingData -= ActivationLoadingScreen;
        IsLoadingDataComplete -= DeactivationLoadingScreen;
        SessionHandler.Instance.IsSessionStarted -= OnSessionStarted;
    }

    private void OnSessionStarted()
    {
        PlayerInput.Instance.Disable();
        _pathFollower.AllowMove();
    }

    private void ActivationLoadingScreen()
    {
        _loadingScreen.SetActive(true);
    }

    private void DeactivationLoadingScreen()
    {
        _loadingScreen.SetActive(false);
    }
}
