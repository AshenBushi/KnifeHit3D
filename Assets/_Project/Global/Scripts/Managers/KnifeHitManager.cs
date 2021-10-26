using UnityEngine;

public class KnifeHitManager : Singleton<KnifeHitManager>
{
    [SerializeField] private CameraMover _cameraMover;

    public int CurrentKnifeHitMod => GamemodManager.Instance.KnifeHitMod;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (CurrentKnifeHitMod == 6)
        {
            _cameraMover.TryMoveCamera(0);
            ColorManager.Instance.RandomColorPreset();
            TargetHandler.Instance.CleanTargets();
            LotteryHandler.Instance.StartLottery();
            return;
        }

        var targetIndex = CurrentKnifeHitMod % 3;
        var cameraPositionIndex = CurrentKnifeHitMod / 3;

        TargetHandler.Instance.SpawnLevel(targetIndex);
        _cameraMover.TryMoveCamera(cameraPositionIndex);
    }
}