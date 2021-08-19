using UnityEngine;

public class KnifeHitManager : Singleton<KnifeHitManager>
{
    [SerializeField] private CameraMover _cameraMover;

    private int _currentMod => GamemodManager.Instance.KnifeHitMod;

    private void OnEnable()
    {
        SessionHandler.Instance.IsSessionStarted += OnSessionStarted;
    }

    private void OnDisable()
    {
        SessionHandler.Instance.IsSessionStarted -= OnSessionStarted;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (_currentMod == 6)
        {
            _cameraMover.TryMoveCamera(0);
            TargetHandler.Instance.CleanTargets();
            LotteryHandler.Instance.StartLottery();
            return;
        }

        var targetIndex = _currentMod % 3;
        var cameraPositionIndex = _currentMod / 3;

        TargetHandler.Instance.SpawnLevel(targetIndex);
        _cameraMover.TryMoveCamera(cameraPositionIndex);
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