using UnityEngine;
using System.Collections;

public class ScoreContinuousSpawn : GoalTutorialEvent
{

	public GameObject ball;
	public BallMachine ballMachine;
	ObjectPool ballPool;
	public float spawnInterval;

	protected override void InitEvent ()
	{
		ballPool = PoolManager.Instance.GetPoolByRepresentative(ball);
	}
	
	public override void Activate ()
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
	
	void HandleScoreEvent(float score)
	{
		SetCompleted();
	}
}

