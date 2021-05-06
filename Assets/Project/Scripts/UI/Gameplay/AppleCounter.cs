using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AppleCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private GameObject _container;

    private List<AppleSpawner> _spawners;

    public int AppleCount { get; private set; } = 0;

    private void OnEnable()
    {
        _spawners = _container.GetComponentsInChildren<AppleSpawner>().ToList();

        foreach (var spawner in _spawners.Where(spawner => spawner.IsAppleSpawn))
        {
            spawner.IsAppleSliced += OnAppleSliced;
        }
    }

    private void OnAppleSliced()
    {
        AppleCount++;
        _countText.text = AppleCount.ToString();
    }
}
