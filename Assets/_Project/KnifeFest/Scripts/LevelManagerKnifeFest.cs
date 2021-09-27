using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace KnifeFest
{
    [Serializable]
    public class KnifeFestLevels
    {
        public List<Level> Levels;
    }

    public class LevelManagerKnifeFest : Singleton<LevelManagerKnifeFest>
    {
        [SerializeField] private KnifeFestLevels _knifeFestLevels;

        private string _wallParametersUrl = "https://drive.google.com/uc?export=download&id=1AQdc3pdWy3MSG3z4yfU5iCoSsWbbqx-m";
        private string _path;

        public Level CurrentLevel { get; private set; }


        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);

#if UNITY_ANDROID && !UNITY_EDITOR
        _path = Path.Combine(Application.persistentDataPath, "KnifeFestLevels.json");
#else
            _path = Path.Combine(Application.dataPath, "KnifeFestLevels.json");
#endif

            if (File.Exists(_path))
            {
                _knifeFestLevels = JsonUtility.FromJson<KnifeFestLevels>(File.ReadAllText(_path));
                CurrentLevel = _knifeFestLevels.Levels[0];

                WallSpawner.Instance.SpawnWalls(CurrentLevel);
            }
            else
            {
                StartCoroutine(LoadDataWalls(_wallParametersUrl, "KnifeFestLevels.json", _knifeFestLevels));
            }
        }

        private IEnumerator LoadDataWalls(string url, string fileName, KnifeFestLevels knifeFestLevels)
        {
            var request = UnityWebRequest.Get(url);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ProtocolError)
            {
                if (File.Exists(_path))
                {
                    knifeFestLevels = JsonUtility.FromJson<KnifeFestLevels>(File.ReadAllText(_path));
                }
            }
            else
            {
                knifeFestLevels = JsonUtility.FromJson<KnifeFestLevels>(request.downloadHandler.text);
                File.WriteAllText(_path, JsonUtility.ToJson(knifeFestLevels));
            }

            _knifeFestLevels = knifeFestLevels;
            CurrentLevel = _knifeFestLevels.Levels[0];

            request.Dispose();

            WallSpawner.Instance.SpawnWalls(CurrentLevel);
        }
    }
}