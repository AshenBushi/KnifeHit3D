using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class TargetSpawner : MonoBehaviour
{
    protected const float SpawnZ = 10;
    protected const float SpawnStep = 25;

    public List<Target> Targets { get; protected set; } = new List<Target>();
    
    [SerializeField] protected Target _template;
    [SerializeField] protected float _spawnY;

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