using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameObjectPool<T> where T : UnityEngine.Component
{
	private Queue<T> _poolItems;
	private GameObject _prefab;

	public GameObjectPool(GameObject prefab)
	{
		_prefab = prefab;
		_poolItems = new Queue<T>();
	}

	public T Get(bool activate = true)
	{
        if (!_poolItems.TryDequeue(out T view))
        {
            view = InstantiateObject();
        }

        view.gameObject.SetActive(activate);

		return view;
	}

	public void Return(T view)
	{
		_poolItems.Enqueue(view);

		view.gameObject.SetActive(false);
	}

	public void Clear()
	{
		foreach (var item in _poolItems)
		{
			Object.Destroy(item);
		}

		_poolItems.Clear();
	}

	private T InstantiateObject()
	{
		var instantiatedObject = Object.Instantiate(_prefab);

		return instantiatedObject.GetComponent<T>();
	}
}
