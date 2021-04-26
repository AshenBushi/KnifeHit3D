using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObstacle : MonoBehaviour
{
    private Knife _knife;
    
    public void Initialize(Knife obstacleTemplate)
    {
        if(_knife != null)
            Destroy(_knife.gameObject);
            
        _knife = Instantiate(obstacleTemplate, transform.position, Quaternion.identity, transform);
        _knife.MakeObstacle();
    }
}
