using System;
using UnityEngine;

[Serializable]
public class PlayerSettings
{
    public int Hp;
    public int Damage;
    public int Speed;
    public float ShootDelay;
    public float ShootDistance;
    public float ShootSpeed;

    public GameObject Prefab;
    public GameObject ProjectilePrefab;
}
