using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnifeStorage : MonoBehaviour
{
    [SerializeField] private List<Knife> _knives;
    [SerializeField] private List<GameObject> _knifePreviews;

    public static List<Knife> Knives { get; private set; }
    public static List<GameObject> KnifePreviews { get; private set; }
    
    public static event UnityAction IsKnifeChanged;

    private void Awake()
    {
        Knives = _knives;
        KnifePreviews = _knifePreviews;
    }
    public static void ChangeKnife(int index)
    {
        DataManager.GameData.ShopData.CurrentKnifeIndex = index;
        DataManager.Save();
        IsKnifeChanged?.Invoke();
    }
    
    public static void AddKnife(int index)
    {   
        DataManager.GameData.ShopData.OpenedKnives.Add(index);
        ChangeKnife(index);
    }
}
