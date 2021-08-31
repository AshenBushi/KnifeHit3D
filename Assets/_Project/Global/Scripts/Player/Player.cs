using UnityEngine;
using UnityEngine.Events;

public class Player : Singleton<Player>
{
    public int Money { get; private set; }

    public event UnityAction IsMoneyChanged;

    private void Start()
    {
        Money = DataManager.Instance.GameData.PlayerData.Money;
    }

    public void DepositMoney(int count)
    {
        Money += count;
        DataManager.Instance.GameData.PlayerData.Money = Money;
        DataManager.Instance.Save();
        IsMoneyChanged?.Invoke();
    }

    public void WithdrawMoney(int count)
    {
        Money -= count;
        DataManager.Instance.GameData.PlayerData.Money = Money;
        DataManager.Instance.Save();
        IsMoneyChanged?.Invoke();
    }
}
