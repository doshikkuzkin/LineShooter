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

	public Enemy Create(Vector3 spawnPosition, float speed, int hp)
	{
		return SpawnEnemy(spawnPosition, speed, hp);
	}

	private Enemy SpawnEnemy(Vector3 spawnPosition, float speed, int hp)
	{
		var enemyObject = _enemyObectsPool.Get();
		enemyObject.SetUp(spawnPosition, speed, hp);

		return enemyObject;
	}
}
	
