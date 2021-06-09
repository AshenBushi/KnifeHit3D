using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopHandler : MonoBehaviour
{
    public event UnityAction IsKnifeChanged;
    
    public void ChangeKnife(int index)
    {
        DataManager.GameData.ShopData.CurrentKnifeIndex = index;
        DataManager.Save();
        IsKnifeChanged?.Invoke();
    }
    
    public void AddKnife(int index)
    {   
        DataManager.GameData.ShopData.OpenedKnives.Add(index);
        ChangeKnife(index);
    }
}
