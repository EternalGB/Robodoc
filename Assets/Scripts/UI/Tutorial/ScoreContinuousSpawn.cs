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
		ScoreCalculator.PlayerScored += HandleScoreEvent;
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
		ScoreCalculator.PlayerScored -= HandleScoreEvent;
	}
	
	void HandleScoreEvent(float score)
	{
		Debug.Log (name + " receiving score event");
		if(score > 0)
			SetCompleted();
	}
}

