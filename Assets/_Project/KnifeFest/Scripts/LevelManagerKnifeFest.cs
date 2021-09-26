using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace KnifeFest
{
    public class LevelManagerKnifeFest : Singleton<LevelManagerKnifeFest>
    {
        [SerializeField] private KnifeFestLevels _knifeFestLevels;

        private string _wallParametersUrl = "https://drive.google.com/uc?export=download&id=1AQdc3pdWy3MSG3z4yfU5iCoSsWbbqx-m";

        public Level CurrentLevel { get; private set; }


        protected override void Awake()
        {
            base.Awake();
            StartCoroutine(LoadDataWalls(_wallParametersUrl, "KnifeFestLevels.json", _knifeFestLevels));
        }

        private IEnumerator LoadDataWalls(string url, string fileName, KnifeFestLevels knifeFestLevels)
        {
            KnifeFestManager.IsLoadingData?.Invoke();

#if UNITY_ANDROID && !UNITY_EDITOR
        var path = Path.Combine(Application.persistentDataPath, fileName);
#else
            var path = Path.Combine(Application.dataPath, fileName);
#endif

            var request = UnityWebRequest.Get(url);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ProtocolError)
            {
                if (File.Exists(path))
                {
                    knifeFestLevels = JsonUtility.FromJson<KnifeFestLevels>(File.ReadAllText(path));
                }
            }
            else
            {
                knifeFestLevels = JsonUtility.FromJson<KnifeFestLevels>(request.downloadHandler.text);
                File.WriteAllText(path, JsonUtility.ToJson(knifeFestLevels));
            }

            _knifeFestLevels = knifeFestLevels;
            CurrentLevel = _knifeFestLevels.Levels[0];

            request.Dispose();

            WallSpawner.Instance.SpawnWalls(CurrentLevel);
        }
    }

    [Serializable]
    public class KnifeFestLevels
    {
        public List<Level> Levels;
    }
}