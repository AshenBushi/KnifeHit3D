using System;
using System.Collections;
using System.Collections.Generic;
using Project.Scripts.Handlers;
using UnityEngine;

public class KnifeHitManager : Singleton<KnifeHitManager>
{
    [SerializeField] private TargetHandler _targetHandler;
    [SerializeField] private CameraMover _cameraMover;

    private void OnEnable()
    {
        GamemodManager.Instance.IsButtonIndexChanged += OnButtonIndexChanged;
        SessionHandler.Instance.IsSessionStarted += OnSessionStarted;
    }

    private void OnDisable()
    {
        GamemodManager.Instance.IsButtonIndexChanged -= OnButtonIndexChanged;
    }

    private void Start()
    {
        OnButtonIndexChanged();
    }

    private void OnButtonIndexChanged()
    {
        var targetIndex = GamemodManager.Instance.LastPressedButtonIndex % 3;
        var cameraPositionIndex = GamemodManager.Instance.LastPressedButtonIndex / 3;
        
        _targetHandler.SpawnLevel(targetIndex);
        _cameraMover.TryMoveCamera(cameraPositionIndex);
    }
    
    private void OnSessionStarted()
    {
        switch (_targetHandler.CurrentSpawnerIndex)
        {
            case 0:
                MetricaManager.SendEvent("target_lvl_start_(" + DataManager.Instance.GameData.ProgressData.CurrentMarkLevel + ")");
                break;
            case 1:
                MetricaManager.SendEvent("cube_lvl_start_(" + DataManager.Instance.GameData.ProgressData.CurrentCubeLevel + ")");
                break;
            case 2:
                MetricaManager.SendEvent("flat_lvl_start_(" + DataManager.Instance.GameData.ProgressData.CurrentFlatLevel + ")");
                break;
        }
    }
}