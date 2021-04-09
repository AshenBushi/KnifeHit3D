using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputField _inputField;
    [SerializeField] private KnifeSpawner _knifeSpawner;

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
        _knifeSpawner.CurrentKnife.AllowMove();
    }
}
