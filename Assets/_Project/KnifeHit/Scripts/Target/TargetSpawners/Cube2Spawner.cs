using UnityEngine;

public class Cube2Spawner : TargetSpawner
{
    public override void SpawnLevel()
    {
        var level = LevelManager.Instance.CurrentCube2Level;
        var colorPreset = ColorManager.Instance.CurrentColorPreset;

        TryCleanTargets();

        Targets.Add(Instantiate(_template, new Vector3(0f, _spawnY, SpawnZ), Quaternion.identity, transform));
        Targets[0].SetupTarget(colorPreset.startColor, null, level);
    }
}
