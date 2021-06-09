using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Mark : Target
{
    private readonly float _explosionForce = 1500f;
    private readonly Vector3 _explosionPosition = new Vector3(0f, 2.5f, 11f);
    
    public override event UnityAction<int> IsTargetBreak;
    
    private void Awake()
    {
        Rotator = GetComponent<TargetRotator>();
    }

    public override void SpawnTargetBase(MarkConfig markConfig = null, CubeLevel level = new CubeLevel(), FlatConfig flatConfig = null)
    {
        if (markConfig is null) return;
        Base = Instantiate(markConfig.Base, transform.position, Quaternion.Euler(0f, 180f, 0f), transform);
        HitToBreak = markConfig.HitToBreak;
        ObstacleCount[0] = markConfig.ObstacleCount;
        ExpReward = markConfig.Experience;
        Rotator.StartRotate(markConfig.RotateDefinitions);
    }

    public override void BreakTarget()
    {
        SoundManager.PlaySound(SoundNames.TargetBreak);
        var targetBase = Instantiate(Base, Base.transform.position, Base.transform.rotation);
        targetBase.Detonate(_explosionPosition, _explosionForce);
        IsTargetBreak?.Invoke(ExpReward);
        Destroy(gameObject);
    }
}