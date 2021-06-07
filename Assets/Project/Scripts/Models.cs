using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Models : MonoBehaviour
{
    [SerializeField] private List<Knife> _knives;
    [SerializeField] private List<GameObject> _knifePreviews;

    public static List<Knife> Knives { get; private set; }
    public static List<GameObject> KnifePreviews { get; private set; }

    private void Awake()
    {
        Knives = _knives;
        KnifePreviews = _knifePreviews;
    }
}
