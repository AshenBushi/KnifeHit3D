using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LotterySpawner : MonoBehaviour
{
    [SerializeField] private Lottery _template;

    private readonly Vector3 _lotteryPosition = new Vector3(0f, -2.25f, 8f);
    
    public Lottery SpawnLottery()
    {
        var lottery = Instantiate(_template, _lotteryPosition, Quaternion.Euler(0f, 180f, 0f), transform);

        return lottery;
    }
}
