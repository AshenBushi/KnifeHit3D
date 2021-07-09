using UnityEngine;
using UnityEngine.Events;

public class Flat : Target
{
    private readonly float _explosionForce = 700f;
    private readonly Vector3 _explosionPosition = new Vector3(0f, .65f, 10f);
    
    public override event UnityAction<int> IsTargetBreak;
    
    private void Awake()
    {
        Rotator = GetComponent<TargetRotator>();
    }

    public override void SetupTarget(Color color, MarkConfig markConfig = null, CubeLevel level = new CubeLevel(), FlatConfig flatConfig = null)
    {
        if (flatConfig is null) return;
        Base = GetComponentInChildren<TargetBase>();
        Base.SetColor(color);
        HitToBreak = flatConfig.HitToBreak;
        ObstacleCount[0] = flatConfig.ObstacleCount;
        ExpReward = flatConfig.Experience;
        Rotator.StartRotate(flatConfig.RotateDefinitions);
    }

    public override void BreakTarget()
    {
        SoundManager.PlaySound(SoundName.TargetBreak);
        var targetBase = Instantiate(Base, Base.transform.position, Base.transform.rotation);
        targetBase.Detonate(_explosionPosition, _explosionForce);
        IsTargetBreak?.Invoke(ExpReward);
        Destroy(gameObject);
    }
}
