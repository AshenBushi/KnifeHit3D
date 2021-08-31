using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnifeStorage : Singleton<KnifeStorage>
{
    [SerializeField] private List<Knife> _knives;
    [SerializeField] private List<GameObject> _knifePreviews;
    [SerializeField] private List<GameObject> _stackKnives;

    public List<Knife> Knives => _knives;
    public List<GameObject> KnifePreviews => _knifePreviews;
    public List<GameObject> StackKnives => _stackKnives;
    
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
    }

    public GameObject GetSimpleKnife()
    {
        return _stackKnives[DataManager.Instance.GameData.ShopData.CurrentKnifeIndex];
    }
}
