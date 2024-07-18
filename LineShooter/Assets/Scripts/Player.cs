using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 _bottomLeftBoundary;
    private Vector2 _topRightBoundary;

    private float _speed;

    public void SetUp(float speed, Vector3 spawnPosition, Vector2 bottomLeftBoundary, Vector2 topRightBoundary)
    {
        _speed = speed;

        transform.position = spawnPosition;

        _bottomLeftBoundary = bottomLeftBoundary;
        _topRightBoundary = topRightBoundary;
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

        var newPosition = transform.position + _speed * moveDirection * Time.deltaTime;

        var xPosition = Mathf.Clamp(newPosition.x, _bottomLeftBoundary.x, _topRightBoundary.x);
        var yPosition = Mathf.Clamp(newPosition.y, _bottomLeftBoundary.y, _topRightBoundary.y);

        transform.position = new Vector3(xPosition, yPosition);
    }
}