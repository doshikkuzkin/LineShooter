using UnityEngine;

public class EnemyA : Enemy
{
    public override EnemyType Type => EnemyType.EnemyA;

    public override void Move()
    {
        Transform.position = new Vector3(Transform.position.x, Transform.position.y - Speed * Time.deltaTime);
    }
}
