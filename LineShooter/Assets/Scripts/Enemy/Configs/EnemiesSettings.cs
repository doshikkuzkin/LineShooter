using System;

[Serializable]
public class EnemiesSettings
{
    public float MinSpawnDelay;
    public float MaxSpawnDelay;
    public int MinEnemiesCount;
    public int MaxEnemiesCount;
    public float MinSpeed;
    public float MaxSpeed;
    public int Hp;

    public EnemyGraphicsData[] EnemiesGraphics;
}
