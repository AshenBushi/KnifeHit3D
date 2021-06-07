using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public int Money { get; private set; }

    public event UnityAction IsMoneyChanged;

    private void Start()
    {
        Money = DataManager.GameData.PlayerData.Money;
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
}
