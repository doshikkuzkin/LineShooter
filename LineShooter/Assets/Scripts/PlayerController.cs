using System;
using UnityEngine;

public class PlayerController
{
    private readonly PlayerSettings _settings;
    private readonly Transform _playerSpawnPoint;
    private readonly Transform _finishLine;

    private GameObjectPool<Player> _playerPool;
    private Player _player;

    private Vector2 _bottomLeftBoundary;
    private Vector2 _topRightBoundary;

    private int _currentHp;

    public event Action<int> PlayerHpChanged;
    public event Action PlayerLost;

    public PlayerController(PlayerSettings settings, Transform playerSpawnPoint, Transform finishLine)
    {
        _settings = settings;
        _playerSpawnPoint = playerSpawnPoint;
        _finishLine = finishLine;

        _playerPool = new GameObjectPool<Player>(settings.Prefab);
        _player = _playerPool.Get(false);

        Vector2 leftCorner = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 rightCorner = Camera.main.ViewportToWorldPoint(new Vector2(1, 0));

        _bottomLeftBoundary = new Vector2(leftCorner.x, leftCorner.y);
        _topRightBoundary = new Vector2(rightCorner.x, _finishLine.position.y);
    }

    public void Start()
    {
        _currentHp = _settings.Hp;

        _player.SetUp(_settings, _playerSpawnPoint.position, _bottomLeftBoundary, _topRightBoundary);
        _player.gameObject.SetActive(true);
    }

    public void Reset()
    {
        _player.Reset();
    }

    public void Update()
    {
        _player.Move();
        _player.MoveProjectiles();
        _player.Shoot();
    }

    public void DecreaseHp()
    {
        _currentHp--;

        PlayerHpChanged?.Invoke(_currentHp);

        if (_currentHp == 0)
        {
            PlayerLost?.Invoke();
        }
    }
}
