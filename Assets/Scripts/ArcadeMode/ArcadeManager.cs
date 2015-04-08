using UnityEngine;
using System.Collections.Generic;

public class ArcadeManager : MonoBehaviour
{

	public BallMachine bm;
	public ArcadeProgression progression;

	public Material baseMat;
	public List<GameObject> goodBallPrefabs;
	public List<GameObject> badBalls;
	public int initBadBalls = 2;

	public GameObject clotPrefab;
	float  clotSpawnInterval;
	int tryLimit = 10;


	void Awake()
	{
		progression.ProcessUnlocked();
		while(goodBallPrefabs.Count > 0) {
			AddColorBall();
		}
		for(int i = 0; i < initBadBalls; i++) {
			AddBadBall();
		}

		InvokeRepeating("SpawnClot", clotSpawnInterval, clotSpawnInterval);
	}
	
	void AddBadBall()
	{
		if(badBalls.Count > 0) {
			int badBallIndex = Random.Range(0,badBalls.Count);
			bm.AddBadBall(badBalls[badBallIndex]);
			badBalls.RemoveAt(badBallIndex);
		}
	}

	void AddColorBall()
	{
		int ballIndex = Random.Range (0,goodBallPrefabs.Count);
		bm.AddColourBall(goodBallPrefabs[ballIndex]);
		goodBallPrefabs.RemoveAt(ballIndex);
	}

	public void SetClotSpawnInterval(float rate)
	{
		clotSpawnInterval = rate;
	}

	void SpawnClot()
	{
		GameObject clot = (GameObject)GameObject.Instantiate(clotPrefab);
		Vector3 pos;
		float radius = 35;
		int tries = 0;
		do {
			pos = Util.RandomPointInside(GameObject.Find ("PlayArea").GetComponent<Collider2D>());
			tries++;
			if(tries >= tryLimit)
				return;
		} while(Util.ExistsWithinSphere(pos,radius,"Clot"));
		clot.transform.position = pos;
		clot.transform.rotation = Util.RandomRotation(Vector3.forward,0,360);
	}
	
}

