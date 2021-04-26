using System;
using UnityEngine;
using UnityEngine.Events;

public class ShopScreen : MonoBehaviour
{
    [SerializeField] private Shop _shop;

    private int _startKnifeIndex;
    
    public event UnityAction IsKnifeChanged;

    private void Start()
    {
        _startKnifeIndex = DataManager.GameData.ShopData.CurrentKnifeIndex;
    }

    public void EnableScreen()
    {
        gameObject.SetActive(true);
    }

    public void DisableScreen()
    {
        if(DataManager.GameData.ShopData.CurrentKnifeIndex != _startKnifeIndex)
            IsKnifeChanged?.Invoke();
        
        gameObject.SetActive(false);
    }
}
