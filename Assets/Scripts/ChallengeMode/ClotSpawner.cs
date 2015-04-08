using UnityEngine;
using System.Collections;

public class ClotSpawner : MonoBehaviour
{

	int tryLimit = 10;

	public Collider2D spawnArea;
	public float spawnInterval;
	public GameObject clotPrefab;



	void Start()
	{
		InvokeRepeating("SpawnClot",spawnInterval,spawnInterval);
	}

	void SpawnClot()
	{
		GameObject clot = (GameObject)GameObject.Instantiate(clotPrefab);
		Vector3 pos;
		float radius = 35;
		int tries = 0;
		do {
			pos = Util.RandomPointInside(spawnArea);
			tries++;
			if(tries >= tryLimit)
				return;
		} while(Util.ExistsWithinSphere(pos,radius,"Clot"));
		clot.transform.position = pos;
		clot.transform.rotation = Util.RandomRotation(Vector3.forward,0,360);
	}

}

