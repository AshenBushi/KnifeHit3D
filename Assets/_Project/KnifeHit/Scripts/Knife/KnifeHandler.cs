using System.Collections.Generic;
using UnityEngine;

public class KnifeHandler : Singleton<KnifeHandler>
{
    [SerializeField] private HitScoreDisplayer _hitScoreDisplayer;
    [SerializeField] private KnifeSpawner _knifeSpawner;

    private Knife _currentKnife;
    private List<Knife> _thrownKnives = new List<Knife>();
    private int _knifeAmount;
    private bool _canThrowKnife = true;

    public Knife CurrentKnifeTemplate => KnifeStorage.Instance.Knives[DataManager.Instance.GameData.ShopData.CurrentKnifeIndex];

    private void OnEnable()
    {
        PlayerInput.Instance.IsTapped += OnTapped;
        KnifeStorage.Instance.IsKnifeChanged += RespawnKnife;
    }

    private void OnDisable()
    {
        PlayerInput.Instance.IsTapped -= OnTapped;
        KnifeStorage.Instance.IsKnifeChanged -= RespawnKnife;

        if (_currentKnife == null) return;
        _currentKnife.IsStuck -= OnKnifeStuck;
        _currentKnife.IsBounced -= OnKnifeBounced;
    }

    private void Start()
    {
        InitKnife();
    }

    private void OnTapped()
    {
        if (_knifeAmount == 0 || !_canThrowKnife) return;

        if (_currentKnife == null)
        {
            InitKnife();
        }

        _currentKnife.Throw();
        _knifeAmount--;
        _thrownKnives.Add(_currentKnife);
        _currentKnife = null;
    }

    private void OnKnifeBounced()
    {
        InitKnife();

        if (DataManager.Instance.GameData.PlayerData.SecondLifeEnabled)
        {
            DataManager.Instance.GameData.PlayerData.SecondLifeEnabled = false;
            DataManager.Instance.Save();
            _knifeAmount++;
            return;
        }
        else
        {
            FailLevel();
        }
    }

    private void OnKnifeStuck()
    {
        InitKnife();
        _hitScoreDisplayer.SubmitHit();
    }

    private void InitKnife()
    {
        if (_currentKnife != null) return;
        _currentKnife = _knifeSpawner.SpawnKnife(CurrentKnifeTemplate);
        _currentKnife.IsStuck += OnKnifeStuck;
        _currentKnife.IsBounced += OnKnifeBounced;
    }

    public void SetKnifeAmount(int knifeAmount)
    {
        _knifeAmount = knifeAmount;
    }

    public void SecondLife()
    {
        PlayerInput.Instance.AllowTap();
        SetKnifeAmount(TargetHandler.Instance.CurrentTarget.HitToBreak);
    }

    private void RespawnKnife()
    {
        Destroy(_currentKnife.gameObject);
        _currentKnife = _knifeSpawner.SpawnKnife(CurrentKnifeTemplate);
        _currentKnife.IsStuck += OnKnifeStuck;
        _currentKnife.IsBounced += OnKnifeBounced;
    }

    private void FailLevel()
    {
        if ((KnifeHitManager.Instance.CurrentKnifeHitMod % 3) <= 2)
        {
            switch (TargetHandler.Instance.CurrentSpawnerIndex)
            {
                case 0:
                    MetricaManager.SendEvent("target_fail_(" + DataManager.Instance.GameData.ProgressData.CurrentMarkLevel + ")");
                    break;
                case 1:
                    MetricaManager.SendEvent("cube_fail_(" + DataManager.Instance.GameData.ProgressData.CurrentCubeLevel + ")");
                    break;
                case 2:
                    MetricaManager.SendEvent("disk_fail_(" + DataManager.Instance.GameData.ProgressData.CurrentFlatLevel + ")");
                    break;
            }
        }
        else
        {
            switch (TargetHandler.Instance.CurrentSpawnerIndex)
            {
                case 0:
                    MetricaManager.SendEvent("target2_fail_(" + DataManager.Instance.GameData.ProgressData.CurrentMarkLevel + ")");
                    break;
                case 1:
                    MetricaManager.SendEvent("cube2_fail_(" + DataManager.Instance.GameData.ProgressData.CurrentCubeLevel + ")");
                    break;
                case 2:
                    MetricaManager.SendEvent("disk2_fail_(" + DataManager.Instance.GameData.ProgressData.CurrentFlatLevel + ")");
                    break;
            }
        }

        SessionHandler.Instance.FailLevel();
    }
}