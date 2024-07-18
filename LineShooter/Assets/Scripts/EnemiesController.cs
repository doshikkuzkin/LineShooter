using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController
{
    private readonly EnemiesSettings _enemiesSettings;

    private readonly EnemyFactory _enemyFactory;
    private readonly GameObjectPool<Enemy> _enemiesPool;
    private readonly Transform[] _enemiesSpawnPoints;

    private readonly Vector3 _finishLinePosition;

    private int _enemiesToSpawnLeft;
    private float _nextSpawnTime;

    private List<Enemy> _activeEnemies = new ();
    private List<Enemy> _enemiesToDestroy = new ();
    private System.Random _random;

    public bool HasEnemiesLeft => _enemiesToSpawnLeft > 0 || _activeEnemies.Count > 0;

    public event Action EnemyHitFinishLine;

    public EnemiesController(EnemiesSettings enemiesSettings, Transform[] enemiesSpawnPoints, Vector3 finishLinePosition)
    {
        _enemiesSettings = enemiesSettings;
        _enemiesToSpawnLeft = enemiesSettings.EnemiesToSpawn;
        _enemiesSpawnPoints = enemiesSpawnPoints;
        _finishLinePosition = finishLinePosition;

        _enemiesPool = new GameObjectPool<Enemy>(enemiesSettings.Prefab);
        _enemyFactory = new EnemyFactory(_enemiesPool);

        _random = new System.Random(Guid.NewGuid().GetHashCode());
    }

    public void Reset()
    {
        _enemiesToSpawnLeft = _enemiesSettings.EnemiesToSpawn;

        foreach (var enemy in _activeEnemies)
        {
            _enemiesPool.Return(enemy);
        }

        _activeEnemies.Clear();
    }

    public void Update()
    {
        if (!HasEnemiesLeft)
        {
            return;
        }

        TryDestroyEnemies();
        TryMoveEnemies();
        TrySpawnEnemie();
    }

    private void TrySpawnEnemie()
    {
        if (_nextSpawnTime < Time.time && _enemiesToSpawnLeft > 0)
        {
            SpawnEnemie();

            _nextSpawnTime = Time.time + GetRandomFloat(_enemiesSettings.MinSpawnDelay, _enemiesSettings.MaxSpawnDelay);
            _enemiesToSpawnLeft--;
        }
    }

    private void SpawnEnemie()
    {
        var spawnPosition = GetRandomSpawnPoint().position;
        var speed = GetRandomFloat(_enemiesSettings.MinSpeed, _enemiesSettings.MaxSpeed);
        var enemy = _enemyFactory.Create(spawnPosition, speed, _enemiesSettings.Hp);

        _activeEnemies.Add(enemy);
    }

    private Transform GetRandomSpawnPoint()
    {
        var spawnPointIndex = _random.Next(0, _enemiesSpawnPoints.Length);

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
            _enemiesPool.Return(enemy);
        }

        _enemiesToDestroy.Clear();
    }

    private bool IsReachedFinishLine(Vector3 position)
    {
        return position.y <= _finishLinePosition.y;
    }

    private void TryMoveEnemies()
    {
        foreach (var enemy in _activeEnemies)
        {
            enemy.Move();
        }
    }

    private float GetRandomFloat(float minValue, float maxValue)
    {
        var range = maxValue - minValue;

        var sample = _random.NextDouble();
        var scaled = (sample * range) + minValue;

        return (float)scaled;
    }
}
