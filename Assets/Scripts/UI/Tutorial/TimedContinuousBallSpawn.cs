using UnityEngine;
using System.Collections;

public class TimedContinuousBallSpawn : TimedTutorialEvent
{

	public GameObject ball;
	public BallMachine ballMachine;
	ObjectPool ballPool;
	public float spawnInterval;

	protected override void InitEvent ()
	{
		ballPool = PoolManager.Instance.GetPoolByRepresentative(ball);
	}

	protected override void StartEvent ()
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
	}
}

