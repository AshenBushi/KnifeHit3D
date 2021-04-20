using System;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] private InputField _inputField;
    [SerializeField] private KnifeSpawner _knifeSpawner;

    private int _money = 50;
    private bool _canThrow = true;

    public int Money => _money;

    public event UnityAction IsMoneyChanged;

    private void OnEnable()
    {
        _inputField.InPlayerTap += ThrowKnife;
    }

    private void OnDisable()
    {
        _inputField.InPlayerTap -= ThrowKnife;
    }
    
    private void ThrowKnife()
    {
        if (!_canThrow) return;
        _knifeSpawner.CurrentKnife.Throw();
        _canThrow = false;
    }

    public void AddMoney(int money)
    {
        _money += money;
        IsMoneyChanged?.Invoke();
    }
    
    public void AllowThrow()
    {
        _canThrow = true;
    }
}
