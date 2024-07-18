using System;
using UnityEngine;

[Serializable]
public class EnemiesSettings
{
    public float MinSpawnDelay;
    public float MaxSpawnDelay;
    public int EnemiesToSpawn;
    public float MinSpeed;
    public float MaxSpeed;
    public int Hp;

    public GameObject Prefab;
}
