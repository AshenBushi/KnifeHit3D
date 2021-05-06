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

    public void EnableShop()
    {
        gameObject.SetActive(true);
    }

    public void DisableScreen()
    {
        SoundManager.PlaySound(SoundNames.ButtonClick);
        if(DataManager.GameData.ShopData.CurrentKnifeIndex != _startKnifeIndex)
            IsKnifeChanged?.Invoke();
        
        gameObject.SetActive(false);
    }
}
