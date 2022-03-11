using System.Collections.Generic;
using UnityEngine;

public abstract class TargetSpawner : MonoBehaviour
{
    [SerializeField] protected Target _template;
    [SerializeField] protected float _spawnY;

    public List<Target> Targets { get; protected set; } = new List<Target>();

    protected const float SpawnZ = 10;
    protected const float SpawnStep = 25;

    public void TryCleanTargets()
    {
        if (Targets.Count < 1) return;
        foreach (var target in Targets)
        {
            Destroy(target.gameObject);
        }

        Targets.Clear();
        Targets = new List<Target>();
    }

    public void RemoveTarget(Target target)
    {
        Targets.Remove(target);
    }
    public abstract void SpawnLevel();
}