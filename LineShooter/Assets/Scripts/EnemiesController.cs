using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController
{
    private readonly EnemiesSettings _enemiesSettings;

    private readonly EnemyFactory _enemyFactory;
    private readonly Dictionary<EnemyType, GameObjectPool<Enemy>> _enemyMap = new ();
    private readonly Transform[] _enemiesSpawnPoints;

    private readonly Vector3 _finishLinePosition;

    private int _enemiesToSpawnLeft;
    private float _nextSpawnTime;

    private List<Enemy> _activeEnemies = new ();
    private List<Enemy> _enemiesToDestroy = new ();

    public bool HasEnemiesLeft => _enemiesToSpawnLeft > 0 || _activeEnemies.Count > 0;

    public event Action EnemyHitFinishLine;

    public EnemiesController(EnemiesSettings enemiesSettings, Transform[] enemiesSpawnPoints, Vector3 finishLinePosition)
    {
        _enemiesSettings = enemiesSettings;
        _enemiesSpawnPoints = enemiesSpawnPoints;
        _finishLinePosition = finishLinePosition;

        foreach (var enemyGraphics in enemiesSettings.EnemiesGraphics)
        {
            _enemyMap.Add(enemyGraphics.Type, new GameObjectPool<Enemy>(enemyGraphics.Prefab));
        }

        _enemyFactory = new EnemyFactory(_enemyMap);
    }

    public void Reset()
    {
        foreach (var enemy in _activeEnemies)
        {
            _enemyMap[enemy.Type].Return(enemy);
        }

        _activeEnemies.Clear();
    }

    public void Start()
    {
        _enemiesToSpawnLeft = RandomHelper.GetRandomInt(_enemiesSettings.MinEnemiesCount, _enemiesSettings.MaxEnemiesCount);
    }

    public void Update()
    {
        if (!HasEnemiesLeft)
        {
            return;
        }

        TryDestroyEnemies();
        MoveEnemies();
        TrySpawnEnemy();
    }

    private void TrySpawnEnemy()
    {
        if (_nextSpawnTime < Time.time && _enemiesToSpawnLeft > 0)
        {
            SpawnEnemy();

            _nextSpawnTime = Time.time + RandomHelper.GetRandomFloat(_enemiesSettings.MinSpawnDelay, _enemiesSettings.MaxSpawnDelay);
            _enemiesToSpawnLeft--;
        }
    }

    private void SpawnEnemy()
    {
        var spawnPosition = GetRandomSpawnPoint().position;
        var type = RandomHelper.GetRandomEnum<EnemyType>();
        var speed = RandomHelper.GetRandomFloat(_enemiesSettings.MinSpeed, _enemiesSettings.MaxSpeed);
        var enemy = _enemyFactory.Create(type, spawnPosition, speed, _enemiesSettings.Hp);

        _activeEnemies.Add(enemy);
    }

    private Transform GetRandomSpawnPoint()
    {
        var spawnPointIndex = RandomHelper.GetRandomInt(0, _enemiesSpawnPoints.Length);

        return _enemiesSpawnPoints[spawnPointIndex];
    }

    private void TryDestroyEnemies()
    {
        foreach (var enemy in _activeEnemies)
        {
            if (enemy.IsDestoyed)
            {
                _enemiesToDestroy.Add(enemy);
            }
            else if (IsReachedFinishLine(enemy.Position))
            {
                EnemyHitFinishLine?.Invoke();

                _enemiesToDestroy.Add(enemy);
            }
        }

        foreach (var enemy in _enemiesToDestroy)
        {
            _activeEnemies.Remove(enemy);
            _enemyMap[enemy.Type].Return(enemy);
        }

        _enemiesToDestroy.Clear();
    }

    private bool IsReachedFinishLine(Vector3 position)
    {
        return position.y <= _finishLinePosition.y;
    }

    private void MoveEnemies()
    {
        foreach (var enemy in _activeEnemies)
        {
            enemy.Move();
        }
    }
}
