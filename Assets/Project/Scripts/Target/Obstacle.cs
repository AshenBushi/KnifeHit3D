using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Knife _knife;
    
    public void Initialize(Knife obstacleTemplate)
    {
        _knife = Instantiate(obstacleTemplate, transform.position, Quaternion.identity, transform);
        _knife.MakeObstacle();
    }
}
