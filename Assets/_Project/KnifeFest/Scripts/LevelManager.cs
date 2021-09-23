using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KnifeFest
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private KnifeFestLevels _knifeFestLevels;

        public Level CurrentLevel { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            CurrentLevel = _knifeFestLevels.Levels[0];
            
            var path = Path.Combine(Application.dataPath, "KnifeFestLevels.json");

            File.WriteAllText(path, JsonUtility.ToJson(_knifeFestLevels));
        }
    }

    [Serializable]
    public class KnifeFestLevels
    {
        public List<Level> Levels;
    }
}