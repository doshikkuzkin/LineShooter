using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory
{
	private GameObjectPool<Enemy> _enemyObectsPool;

	public EnemyFactory(GameObjectPool<Enemy> enemyObectsPool)
	{
		_enemyObectsPool = enemyObectsPool;
	}

	public Enemy Create(Vector3 spawnPosition, float speed)
	{
		return SpawnEnemy(spawnPosition, speed);
	}

	private Enemy SpawnEnemy(Vector3 spawnPosition, float speed)
	{
		var enemyObject = _enemyObectsPool.Get();
		enemyObject.SetUp(spawnPosition, speed);

		return enemyObject;
	}
}
	
