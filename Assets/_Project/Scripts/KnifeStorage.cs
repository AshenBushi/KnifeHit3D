using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnifeStorage : Singleton<KnifeStorage>
{
    [SerializeField] private List<Knife> _knives;
    [SerializeField] private List<GameObject> _knifePreviews;

    public List<Knife> Knives => _knives;
    public List<GameObject> KnifePreviews => _knifePreviews;
    
    public event UnityAction IsKnifeChanged;
    
    public void ChangeKnife(int index)
    {
        DataManager.Instance.GameData.ShopData.CurrentKnifeIndex = index;
        DataManager.Instance.Save();
        IsKnifeChanged?.Invoke();
    }
    
    public void AddKnife(int index)
    {   
        DataManager.Instance.GameData.ShopData.OpenedKnives.Add(index);
        ChangeKnife(index);
    }
}
