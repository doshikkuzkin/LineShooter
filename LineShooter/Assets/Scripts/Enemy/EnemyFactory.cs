using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory
{
	private readonly Dictionary<EnemyType, GameObjectPool<Enemy>> _enemyMap;

    public EnemyFactory(Dictionary<EnemyType, GameObjectPool<Enemy>> enemyMap)
	{
		_enemyMap = enemyMap;
	}

	public Enemy Create(EnemyType enemyType, Vector3 spawnPosition, float speed, int hp)
	{
		return CreateInternal(_enemyMap[enemyType].Get(), spawnPosition, speed, hp);
	}

	private Enemy CreateInternal(Enemy enemyObject, Vector3 spawnPosition, float speed, int hp)
	{
		enemyObject.SetUp(spawnPosition, speed, hp);

		return enemyObject;
	}
}
	
