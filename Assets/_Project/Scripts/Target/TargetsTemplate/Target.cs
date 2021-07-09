using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TargetRotator))]
public abstract class Target : MonoBehaviour
{
    protected TargetRotator Rotator;
    protected int EdgeCount;
    protected int ExpReward;

    public int HitToBreak { get; protected set; }

    public int[] ObstacleCount { get; protected set; } = new int[6];
    public TargetBase Base { get; protected set; }
    
    public virtual event UnityAction<int> IsTargetBreak;
    public virtual event UnityAction<int, int> IsEdgePass;

    public abstract void BreakTarget();
    public abstract void SetupTarget(Color color, MarkConfig markConfig = null, CubeLevel level = new CubeLevel(), FlatConfig flatConfig = null);
    
    public void TakeHit()
    {
        HitToBreak--;

        Base.SpringBack();
        
        if (HitToBreak <= 0)
        {
            BreakTarget();
        }
    }
    
}
