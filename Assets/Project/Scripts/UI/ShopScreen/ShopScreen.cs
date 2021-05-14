using System;
using UnityEngine;
using UnityEngine.Events;

public class ShopScreen : UIScreen
{
    [SerializeField] private Shop _shop;

    private int _startKnifeIndex;
    
    public event UnityAction IsKnifeChanged;

    private void Start()
    {
        _startKnifeIndex = DataManager.GameData.ShopData.CurrentKnifeIndex;
    }

    public override void Enable()
    {
        gameObject.SetActive(true);
    }

    public override void Disable()
    {
        SoundManager.PlaySound(SoundNames.ButtonClick);
        if(DataManager.GameData.ShopData.CurrentKnifeIndex != _startKnifeIndex)
            IsKnifeChanged?.Invoke();
        
        gameObject.SetActive(false);
    }
}
