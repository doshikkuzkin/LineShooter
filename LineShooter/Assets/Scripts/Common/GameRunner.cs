using UnityEngine;

public class GameRunner : MonoBehaviour
{
	[SerializeField]
	private EnemiesSettingsScriptableObject _enemiesSettings;

    [SerializeField]
    private PlayerSettingsScriptableObject _playerSettings;

    [SerializeField]
    private Transform _playerSpawnPoint;

    [SerializeField]
	private Transform[] _enemiesSpawnPoints;

    [SerializeField]
    private Transform _finishLine;

    [SerializeField]
    private InGameUIView _gameUIView;

    [SerializeField]
    private StartGameUIView _startGameUIView;

    [SerializeField]
    private RestartGameUIView _restartGameUIView;

    private EnemiesController _enemiesController;
    private PlayerController _playerController;

    private bool _isGameRunning;

    private void Awake()
    {
		_enemiesController = new EnemiesController(_enemiesSettings.EnemiesSettings, _enemiesSpawnPoints, _finishLine.position);
        _playerController = new PlayerController(_playerSettings.PlayerSettings, _playerSpawnPoint, _finishLine);

        _startGameUIView.gameObject.SetActive(true);
        _restartGameUIView.gameObject.SetActive(false);

        _startGameUIView.StartButtonClicked += OnStartButtonClicked;
        _restartGameUIView.StartButtonClicked += OnRestartButtonClicked;

        UpdateHpText();
    }

    private void UpdateHpText()
    {
        _gameUIView.SetHpText(_playerSettings.PlayerSettings.Hp);
    }

    private void OnStartButtonClicked()
    {
       _startGameUIView.gameObject.SetActive(false);

        StartGame();
    }

    private void OnRestartButtonClicked()
    {
        _restartGameUIView.gameObject.SetActive(false);

        _enemiesController.Reset();
        _playerController.Reset();

        StartGame();
    }

    private void StartGame()
	{
		_isGameRunning = true;

        _playerController.Start();
        _enemiesController.Start();

        UpdateHpText();

        Subscribe();
    }

    private void Subscribe()
    {
        _enemiesController.EnemyHitFinishLine += OnEnemyHitFinishLine;
        _playerController.PlayerHpChanged += OnPlayerHpChanged;
        _playerController.PlayerLost += OnPlayerLost;
    }

    private void Unsubscribe()
    {
        _enemiesController.EnemyHitFinishLine -= OnEnemyHitFinishLine;
        _playerController.PlayerHpChanged -= OnPlayerHpChanged;
        _playerController.PlayerLost -= OnPlayerLost;
    }

    private void OnEnemyHitFinishLine()
    {
        _playerController.DecreaseHp();
    }

    private void OnPlayerHpChanged(int currentHp)
    {
        _gameUIView.SetHpText(currentHp);
    }

    private void OnPlayerLost()
    {
        OnGameEnded(false);
    }

    private void OnGameEnded(bool isWon)
    {
        _isGameRunning = false;

        _restartGameUIView.SetupText(isWon);
        _restartGameUIView.gameObject.SetActive(true);

        Unsubscribe();
    }

    private void Update()
    {
        if (!_isGameRunning)
        {
            return;
        }

        if (!_enemiesController.HasEnemiesLeft)
        {
            OnGameEnded(true);

            return;
        }

        _playerController.Update();
        _enemiesController.Update();
    }
}
