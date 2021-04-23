using System;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] private InputField _inputField;
    [SerializeField] private KnifeSpawner _knifeSpawner;
    
    private bool _canThrow = true;

    public int Money { get; private set; }

    public event UnityAction IsMoneyChanged;

    private void OnEnable()
    {
        _inputField.InPlayerTap += ThrowKnife;
    }

    private void OnDisable()
    {
        _inputField.InPlayerTap -= ThrowKnife;
    }

    private void Start()
    {
        Money = DataManager.GameData.PlayerData.Money;
    }

    private void ThrowKnife()
    {
        if (!_canThrow) return;
        _knifeSpawner.CurrentKnife.Throw();
        _canThrow = false;
    }

    public void DepositMoney(int count)
    {
        Money += count;
        DataManager.GameData.PlayerData.Money = Money;
        DataManager.Save();
        IsMoneyChanged?.Invoke();
    }

    public void WithdrawMoney(int count)
    {
        Money -= count;
        DataManager.GameData.PlayerData.Money = Money;
        DataManager.Save();
        IsMoneyChanged?.Invoke();
    }
    
    public void AllowThrow()
    {
        _canThrow = true;
    }
}
