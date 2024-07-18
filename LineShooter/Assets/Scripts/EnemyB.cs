using UnityEngine;

public class EnemyB : Enemy
{
    [SerializeField]
    private float _frequency;

    [SerializeField]
    private float _amplitude;

    private Vector3 _initialPosition;

    public override EnemyType Type => EnemyType.EnemyB;

    public override void SetUp(Vector3 position, float speed, int hp)
    {
        base.SetUp(position, speed, hp);

        _initialPosition = position;
    }

    public override void Move()
    {
        var yPosition = transform.position.y - Speed * Time.deltaTime;
        var xPosition = _initialPosition.x + Mathf.Sin(yPosition * _frequency) * _amplitude;

        Transform.position = new Vector3(xPosition, yPosition);
    }
}
