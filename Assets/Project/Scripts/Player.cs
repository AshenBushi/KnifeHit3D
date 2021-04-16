using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputField _inputField;
    [SerializeField] private KnifeSpawner _knifeSpawner;

    private bool _canThrow = true;

    private void OnEnable()
    {
        _inputField.OnPlayerTap += ThrowKnife;
    }

    private void OnDisable()
    {
        _inputField.OnPlayerTap -= ThrowKnife;
    }

    private void ThrowKnife()
    {
        if (!_canThrow) return;
        _knifeSpawner.CurrentKnife.Throw();
        _canThrow = false;
    }

    public void AllowThrow()
    {
        _canThrow = true;
    }
}
