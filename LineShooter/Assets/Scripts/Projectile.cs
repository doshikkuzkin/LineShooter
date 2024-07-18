using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 _initialPosition;
    private Vector3 _moveDirection;
    private float _speed;
    private float _maxDistance;

    public Action<Projectile> ProjectileDestroyed;
    public Action<Enemy> ProjectileHitEnemy;

    public void SetUp(Vector3 position, Vector3 moveDirection, float speed, float maxDistance)
    {
        _initialPosition = position;
        transform.position = position;

        _moveDirection = moveDirection;
        _speed = speed;
        _maxDistance = maxDistance;
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
        var hitEnemy = collision.GetComponent<Enemy>();

        if (hitEnemy != null)
        {
            ProjectileHitEnemy?.Invoke(hitEnemy);
            ProjectileDestroyed?.Invoke(this);
        }
    }
}
