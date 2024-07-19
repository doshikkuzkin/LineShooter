using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private CircleCollider2D _circleCollider;

    private PlayerSettings _playerSettings;

    private Vector2 _bottomLeftBoundary;
    private Vector2 _topRightBoundary;

    private GameObjectPool<Projectile> _projectilesPool;

    private List<Projectile> _projectiles = new ();
    private List<Transform> _visibleEnemies = new ();

    private float _nextShootTime;

    public void SetUp(PlayerSettings playerSettings, Vector3 spawnPosition, Vector2 bottomLeftBoundary, Vector2 topRightBoundary)
    {
        _playerSettings = playerSettings;
        _bottomLeftBoundary = bottomLeftBoundary;
        _topRightBoundary = topRightBoundary;

        transform.position = spawnPosition;

        _circleCollider.radius = _playerSettings.ShootDistance;
        _projectilesPool = new GameObjectPool<Projectile>(_playerSettings.ProjectilePrefab);
    }

    public void Move()
    {
        var moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveDirection.y += 1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveDirection.y -= 1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveDirection.x -= 1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveDirection.x += 1f;
        }

        var newPosition = transform.position + _playerSettings.Speed * moveDirection * Time.deltaTime;

        var xPosition = Mathf.Clamp(newPosition.x, _bottomLeftBoundary.x, _topRightBoundary.x);
        var yPosition = Mathf.Clamp(newPosition.y, _bottomLeftBoundary.y, _topRightBoundary.y);

        transform.position = new Vector3(xPosition, yPosition);
    }

    public void MoveProjectiles()
    {
        foreach (var projectile in _projectiles.ToArray())
        {
            projectile.Move();
        }
    }

    public void Shoot()
    {
        if (_nextShootTime > Time.time)
        {
            return;
        }

        if (TryGetCloserEnemyInShootDistance(out var enemyTransform))
        {
            CreateProjectile(enemyTransform.position);

            _nextShootTime = Time.time + _playerSettings.ShootDelay;
        }
    }

    private bool TryGetCloserEnemyInShootDistance(out Transform enemyTransform)
    {
        enemyTransform = null;

        if (!_visibleEnemies.Any())
        {
            return false;
        }

        float? minDistance = null;

        foreach (var enemy in _visibleEnemies)
        {
            var distanceToEnemy = Vector3.Distance(enemy.transform.position, transform.position);

            if (minDistance == null || distanceToEnemy < minDistance.Value)
            {
                minDistance = distanceToEnemy;
                enemyTransform = enemy;
            }
        }

        return true;
    }

    private void CreateProjectile(Vector3 targetPosition)
    {
        var moveDirection = targetPosition - transform.position;
        moveDirection.Normalize();

        var projectile = _projectilesPool.Get();
        projectile.SetUp(transform.position, moveDirection, _playerSettings.ShootSpeed, _playerSettings.ShootDistance);

        projectile.ProjectileDestroyed = OnProjectileDestroyed;
        projectile.ProjectileHitEnemy = OnProjectileHitEnemy;

        _projectiles.Add(projectile);
    }

    private void OnProjectileDestroyed(Projectile projectile)
    {
        _projectilesPool.Return(projectile);
        _projectiles.Remove(projectile);
    }

    private void OnProjectileHitEnemy(Enemy enemy)
    {
        enemy.DecreaseHp(_playerSettings.Damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            _visibleEnemies.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            _visibleEnemies.Remove(collision.transform);
        }
    }

    public void Reset()
    {
        foreach (var projectile in _projectiles)
        {
            _projectilesPool.Return(projectile);
        }

        _projectiles.Clear();
    }
}