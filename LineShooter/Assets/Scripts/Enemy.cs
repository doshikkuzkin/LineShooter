using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _speed;
    private Transform _transform;

    public Vector3 Position => _transform.position;

    private void Awake()
    {
        _transform = transform;
    }

    public void SetUp(Vector3 position, float speed)
    {
        transform.position = position;
        _speed = speed;
    }

    public void Move()
    {
        _transform.position = new Vector3(_transform.position.x, _transform.position.y - _speed * Time.deltaTime);
    }
}
