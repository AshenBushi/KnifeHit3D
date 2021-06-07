using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LotterySpawner : MonoBehaviour
{
    [SerializeField] private Lottery _template;

    private readonly Vector3 _lotteryPosition = new Vector3(0f, 2.1f, 8f);
    
    public Lottery SpawnLottery()
    {
        var lottery = Instantiate(_template, _lotteryPosition, Quaternion.identity, transform);

        return lottery;
    }
}
