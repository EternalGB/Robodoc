using UnityEngine;
using System.Collections;

public class BallCollectContinuousSpawn : GoalTutorialEvent
{



	public GameObject ball;
	public BallMachine ballMachine;
	ObjectPool ballPool;
	public float spawnInterval;
	PlayerBall playerBall;
	
	protected override void InitEvent ()
	{
		playerBall = GameObject.Find ("PlayerBall").GetComponent<PlayerBall>();
		playerBall.BallCollect += BallCollectionHandler;
		ballPool = PoolManager.Instance.GetPoolByRepresentative(ball);
	}
	
	void BallCollectionHandler(GameObject player, GameObject playerPart, GameObject ball)
	{
		SetCompleted();
	}
	
	public override void Activate()
	{
		Invoke("Spawn",0);
	}
	
	void Spawn()
	{
		ballMachine.SpawnBall(ballPool.GetPooled());
		Invoke("Spawn",spawnInterval);
	}
	
	public override void Deactivate ()
	{
		CancelInvoke("Spawn");
		playerBall.BallCollect -= BallCollectionHandler;
	}

}

