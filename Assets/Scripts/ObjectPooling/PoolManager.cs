using UnityEngine;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour
{

	public GameObject poolPrefab;
	private Dictionary<string, ObjectPool> currentPools;

	public static PoolManager Instance
	{
		get; private set;
	}
	
	void Awake()
	{
		if(Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		
		Instance = this;

		currentPools = new Dictionary<string, ObjectPool>();
	}

	public ObjectPool GetPoolByName(string name)
	{
		return currentPools[name];
	}
	
	public ObjectPool GetPoolByRepresentative(GameObject rep)
	{	
		ObjectPool pool = null;
		if(!currentPools.TryGetValue(rep.gameObject.name + "Pool", out pool)) {
			pool = ((GameObject)GameObject.Instantiate(poolPrefab))
				.GetComponent<ObjectPool>();
			pool.Init(rep,10,true);
			pool.gameObject.name = rep.gameObject.name + "Pool";
			currentPools.Add(pool.gameObject.name, pool);
		}
		return pool;
	}

	public ObjectPool GetPoolByResourcePath(string path)
	{
		GameObject obj = Resources.Load<GameObject>(path);
		if(obj != null) {
			return GetPoolByRepresentative(obj);
		} else {
			return null;
		}
	}

	public ObjectPool GetNewUntrackedPool(GameObject pooledObject, int pooledAmount, bool growable)
	{
		ObjectPool pool = ((GameObject)GameObject.Instantiate(poolPrefab))
			.GetComponent<ObjectPool>();
		pool.Init(pooledObject,pooledAmount,growable);
		pool.gameObject.name = pooledObject.gameObject.name + "Pool";
		return pool;
	}

}

