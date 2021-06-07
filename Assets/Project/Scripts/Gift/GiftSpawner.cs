using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GiftSpawner : MonoBehaviour
{
    [SerializeField] private Gift _template;
    public Gift SpawnGift()
    {
        return Instantiate(_template, transform);
    }
}
