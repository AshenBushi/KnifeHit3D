#pragma warning disable 0649

using System.Collections.Generic;
using UnityEngine;
using Watermelon;

public class LevelController : MonoBehaviour
{
    public const float PLATFORM_ROTATE_VALUE = 5;
    public const float PLATFORM_POSITION_VALUE = 0.5f;

    [SerializeField] ProjectData projectData;

    [Space]
    [SerializeField] Transform platformsContainer;
    [SerializeField] Transform pillarTransform;
        
    private PoolSimple platformPool;
    private Level level;

    private float currentScaleValue = 1.0f;
    private bool isScaleGrow = true;

    private int lastSpawnedPlatformIndex;

    private List<Platform> platformsQueue = new List<Platform>();
    public Platform NextPlatform
    {
        get
        {
            if (platformsQueue.Count > 0)
                return platformsQueue[0];

            return null;
        }
    }

    public int PlatformsQueueCount => platformsQueue.Count;

    public void Init(PoolSimple platformPool, Level level)
    {
        this.platformPool = platformPool;
        this.level = level;

        ResetLevel();
    }

    public void ResetLevel()
    {
        // Return all game objects to pool
        platformPool.ReturnToPoolEverything();

        // Reset spawn queue
        platformsQueue = new List<Platform>();

        // Reset pillar position
        int levelPlatforms = level.levelPlatformsData.Length;
        pillarTransform.localScale = new Vector3(1, levelPlatforms, 1);
        pillarTransform.localPosition = new Vector3(0, levelPlatforms - 1, 0);

        currentScaleValue = (projectData.maxScaleValue + projectData.minScaleValue) / 2;
        isScaleGrow = true;
    }

    public void UnloadLevel()
    {
        int pooledObjectsCount = platformPool.pooledObjects.Count;
        for(int i = 0; i < pooledObjectsCount; i++)
        {
            Destroy(platformPool.pooledObjects[i].gameObject);
        }

        platformPool = null;
    }
        
    public void SpawnPlatforms(int count)
    {
        if (count > level.levelPlatformsData.Length)
        {
            lastSpawnedPlatformIndex = level.levelPlatformsData.Length - 1;

            for (int i = lastSpawnedPlatformIndex; i >= 0; i--)
            {
                SpawnAndInitializePlatform(i);
            }
        }
        else
        {
            lastSpawnedPlatformIndex = level.levelPlatformsData.Length - 1;
            int finishPlatform = level.levelPlatformsData.Length - count;
            for (int i = lastSpawnedPlatformIndex; i > finishPlatform; i--)
            {
                SpawnAndInitializePlatform(i);
            }
        }
    }

    private void SpawnAndInitializePlatform(int index)
    {
        GameObject platformGameObject = platformPool.GetPooledObject();
        platformGameObject.transform.SetParent(platformsContainer);
        platformGameObject.transform.position = new Vector3(0, index * PLATFORM_POSITION_VALUE, 0);
        platformGameObject.transform.localEulerAngles = new Vector3(0, index * PLATFORM_ROTATE_VALUE, 0);
        platformGameObject.SetActive(true);
        platformGameObject.name = index.ToString();

        float lerpValue = (float)index / level.levelPlatformsData.Length;

        if(isScaleGrow)
        {
            currentScaleValue += projectData.scaleStep;
            if (currentScaleValue >= projectData.maxScaleValue)
                isScaleGrow = false;
        }
        else
        {
            currentScaleValue -= projectData.scaleStep;
            if (currentScaleValue <= projectData.minScaleValue)
                isScaleGrow = true;
        }

        Platform platform = platformGameObject.GetComponent<Platform>();
        platform.Init(index, level.levelPlatformsData[index].data, Color.Lerp(level.levelColorPreset.endColor, level.levelColorPreset.startColor, lerpValue), lerpValue, currentScaleValue, GameController.DefaultPlatformMaterial, GameController.ObstaclePlatformMaterial);

        platformsQueue.Add(platform);

        lastSpawnedPlatformIndex--;
    }

    public void DestructPlatform(Platform platform)
    {
        platform.DestructObject();
        platformsQueue.RemoveAt(0);

        if(lastSpawnedPlatformIndex >= 0)
        {
            SpawnAndInitializePlatform(lastSpawnedPlatformIndex);
        }
    }

    private void FixedUpdate()
    {
        platformsContainer.Rotate(0, Time.fixedDeltaTime * projectData.platformsRotationSpeed, 0);
    }
}
