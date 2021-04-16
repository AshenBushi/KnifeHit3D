using UnityEngine;

public class SessionManager : MonoBehaviour
{
    [SerializeField] private KnifeSpawner _knifeSpawner;
    [SerializeField] private TargetSpawner _targetSpawner;
    [SerializeField] private LoseScreen _loseScreen;
    [SerializeField] private WinScreen _winScreen;

    private readonly Level _currentLevel = new Level() {TargetCount = 5};
    
    private void OnEnable()
    {
        _targetSpawner.IsWin += OnWin;
        _knifeSpawner.IsLose += OnLose;
    }

    private void OnDisable()
    {
        _targetSpawner.IsWin -= OnWin;
        _knifeSpawner.IsLose -= OnLose;
    }

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        _knifeSpawner.SpawnKnife();
        _targetSpawner.SpawnLevel(_currentLevel);
    }

    private void OnWin()
    {
        _winScreen.Win();
    }
    
    private void OnLose()
    {
        _loseScreen.Lose();
    }
}
