using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{

	public List<GameObject> obstacles;
	float maxWait = 2f;

	// Use this for initialization
	void Start ()
	{
		foreach(GameObject obstacle in obstacles) {
			StartCoroutine(ActivateObstacle(obstacle));
		}
	}

	System.Collections.IEnumerator ActivateObstacle(GameObject obs)
	{

		yield return new WaitForSeconds(Random.Range(0,maxWait));
		obs.SetActive(true);
	}

	

}

