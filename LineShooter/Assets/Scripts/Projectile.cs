using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private Transform _spriteObject;

    private Vector3 _initialPosition;
    private Vector3 _moveDirection;
    private float _speed;
    private float _maxDistance;

    private bool _isDestroyed;

    public Action<Projectile> ProjectileDestroyed;
    public Action<Enemy> ProjectileHitEnemy;

    public void SetUp(Vector3 position, Vector3 moveDirection, float speed, float maxDistance)
    {
        _initialPosition = position;
        transform.position = position;

        _moveDirection = moveDirection;
        _speed = speed;
        _maxDistance = maxDistance;

        RotateTowardsMovement(moveDirection);

        _isDestroyed = false;
    }

    private void RotateTowardsMovement(Vector3 moveDirection)
    {
        var angle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
        _spriteObject.rotation = Quaternion.Euler(0, 0, -angle);
    }

    public void Move()
    {
        if (Vector3.Magnitude(transform.position - _initialPosition) >= _maxDistance)
        {
            ProjectileDestroyed?.Invoke(this);
        }

        transform.position += _moveDirection * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isDestroyed)
        {
            return;
        }

        var hitEnemy = collision.GetComponent<Enemy>();

        if (hitEnemy != null)
        {
            _isDestroyed = true;

            ProjectileHitEnemy?.Invoke(hitEnemy);
            ProjectileDestroyed?.Invoke(this);
        }
    }
}
