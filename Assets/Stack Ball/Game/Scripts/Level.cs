using System.Collections.Generic;

[System.Serializable]
public class Level
{
    public int levelPlatformTypeID;

    public LevelBytes[] levelPlatformsData;

    [System.NonSerialized]
    public LevelPlatformType levelPlatformType;

    [System.NonSerialized]
    public LevelColorPreset levelColorPreset;

    public Level() { }

    public Level(Level level)
    {
        levelPlatformTypeID = level.levelPlatformTypeID;
        levelPlatformsData = level.levelPlatformsData;
    }

    [System.Serializable]
    public class LevelBytes
    {
        public byte[] data;

        public LevelBytes(byte[] data)
        {
            this.data = data;
        }
    }
}
