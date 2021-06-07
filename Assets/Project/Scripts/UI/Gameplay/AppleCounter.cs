using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AppleCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _countText;

    public void SetCount(int count)
    {
        _countText.text = count.ToString();
    }
}
