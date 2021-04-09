using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    private int _score = 0;

    public void AddScore()
    {
        _score++;
    }
}
