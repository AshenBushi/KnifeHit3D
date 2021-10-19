using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum WinLotterySectionType
{
    x2,
    x3
}

public class WinLotterySection : MonoBehaviour
{
    [SerializeField] private WinLotterySectionType _type;

    public WinLotterySectionType Type => _type;
}
