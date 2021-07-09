using System.Collections.Generic;
using UnityEngine;
using Watermelon;

[CreateAssetMenu(fileName = "Level Database", menuName = "Content/Level Database")]
public class LevelDatabase : ScriptableObject, IInitialized
{
    public Level[] levels;

    public LevelPlatformType[] levelPlatformTypes;

    public LevelColorPreset[] levelColorPresets;

    public void Init()
    {
        for(int i = 0; i < levels.Length; i++)
        {
            levels[i].levelPlatformType = levelPlatformTypes[levels[i].levelPlatformTypeID];
        }
    }

    public Level GetLevel(int id)
    {
        if(id >= levels.Length)
        {
            return null;
        }

        levels[id].levelColorPreset = levelColorPresets.GetRandomItem();

        return levels[id];
    }
}