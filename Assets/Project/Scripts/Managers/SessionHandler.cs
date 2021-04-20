using UnityEngine;

public class SessionHandler : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private KnifeSpawner _knifeSpawner;
    [SerializeField] private TargetSpawner _targetSpawner;
    [SerializeField] private StartScreen _startScreen;
    [SerializeField] private LoseScreen _loseScreen;
    [SerializeField] private WinScreen _winScreen;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private InputField _inputField;

    private void OnEnable()
    {
        _inputField.IsSessionStart += StartGame;
        _targetSpawner.IsWin += OnWin;
        _knifeSpawner.IsLose += OnLose;
    }

    private void OnDisable()
    {
        _inputField.IsSessionStart -= StartGame;
        _targetSpawner.IsWin -= OnWin;
        _knifeSpawner.IsLose -= OnLose;
    }

    private void Start()
    {
        _targetSpawner.SpawnLevel(_levelManager.CurrentLevel, _knifeSpawner.CurrentTemplate);
        _knifeSpawner.SpawnKnife();
    }

    private void StartGame()
    {
        _targetSpawner.SetCurrentTarget();
        _startScreen.StartSession();
    }

    private void OnWin()
    {
        _player.AddMoney(50);
        _winScreen.Win();
    }
    
    private void OnLose()
    {
        _loseScreen.Lose();
    }
}
