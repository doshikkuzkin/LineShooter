using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    private int _hp;

    protected float Speed;
    protected Transform Transform;

    public Vector3 Position => Transform.position;
    public bool IsDestoyed => _hp <= 0;
    public abstract EnemyType Type { get; }

    private void Awake()
    {
        Transform = transform;
    }

    public virtual void SetUp(Vector3 position, float speed, int hp)
    {
        Transform.position = position;
        Speed = speed;
        _hp = hp;
    }

    public void DecreaseHp(int damage)
    {
        _hp -= damage;
    }

    public abstract void Move();
}
