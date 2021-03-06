using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Knife _obstacle;
    
    public void Initialize(Knife obstacleTemplate)
    {
        if(_obstacle != null)
            Destroy(_obstacle.gameObject);
            
        _obstacle = Instantiate(obstacleTemplate, transform.position, Quaternion.identity);
        _obstacle.MakeDefaultObstacle(transform);
    }
}
