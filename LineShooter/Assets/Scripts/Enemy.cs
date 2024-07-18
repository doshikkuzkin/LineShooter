using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _speed;
    private int _hp;
    private Transform _transform;

    public Vector3 Position => _transform.position;
    public bool IsDestoyed => _hp <= 0;

    private void Awake()
    {
        _transform = transform;
    }

    public void SetUp(Vector3 position, float speed, int hp)
    {
        transform.position = position;
        _speed = speed;
        _hp = hp;
    }

    public void Move()
    {
        _transform.position = new Vector3(_transform.position.x, _transform.position.y - _speed * Time.deltaTime);
    }

    public void DecreaseHp(int damage)
    {
        _hp -= damage;
    }
}
