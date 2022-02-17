using UnityEngine;

public class Mark2Spawner : TargetSpawner
{
    public override void SpawnLevel()
    {
        var level = LevelManager.Instance.CurrentMark2Level;
        var colorPreset = ColorManager.Instance.CurrentColorPreset;

        TryCleanTargets();

        for (var i = 0; i < level.Marks.Count; i++)
        {
            Targets.Add(Instantiate(_template, new Vector3(0f, _spawnY, SpawnZ + SpawnStep * i), Quaternion.identity, transform));
            Targets[i].SetupTarget(Color.Lerp(colorPreset.endColor, colorPreset.startColor, (float)i / level.Marks.Count), level.Marks[i]);
        }
    }
}
