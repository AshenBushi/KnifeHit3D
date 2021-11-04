using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private static bool _isInitialized = false;
    private void Awake()
    {
        if (!_isInitialized)
        {
            _isInitialized = true;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
