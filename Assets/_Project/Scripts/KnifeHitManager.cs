using UnityEngine;

public class KnifeHitManager : Singleton<KnifeHitManager>
{
    [SerializeField] private CameraMover _cameraMover;

    private bool _isLotterySpawned = false;

    private void OnEnable()
    {
        GamemodManager.Instance.IsButtonIndexChanged += OnButtonIndexChanged;
        GamemodManager.Instance.IsLotterySelected += OnLotterySelected;
        SessionHandler.Instance.IsSessionStarted += OnSessionStarted;
    }

    private void OnDisable()
    {
        GamemodManager.Instance.IsButtonIndexChanged -= OnButtonIndexChanged;
        GamemodManager.Instance.IsLotterySelected -= OnLotterySelected;
        SessionHandler.Instance.IsSessionStarted -= OnSessionStarted;
    }

    private void Start()
    {
        OnButtonIndexChanged();
    }

    private void OnButtonIndexChanged()
    {
        if (_isLotterySpawned)
        {
            LotteryHandler.Instance.CleanLottery();
            _isLotterySpawned = false;
        }
        
        var targetIndex = GamemodManager.Instance.LastPressedButtonIndex % 3;
        var cameraPositionIndex = GamemodManager.Instance.LastPressedButtonIndex / 3;
        
        TargetHandler.Instance.SpawnLevel(targetIndex);
        _cameraMover.TryMoveCamera(cameraPositionIndex);
    }
    
    private void OnLotterySelected()
    {
        if (_isLotterySpawned) return;
        _cameraMover.TryMoveCamera(0);
        TargetHandler.Instance.CleanTargets();
        LotteryHandler.Instance.StartLottery();
        _isLotterySpawned = true;
    }
    
    private void OnSessionStarted()
    {
        switch (TargetHandler.Instance.CurrentSpawnerIndex)
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