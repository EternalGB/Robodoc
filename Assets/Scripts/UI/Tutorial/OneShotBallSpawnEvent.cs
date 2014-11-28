using UnityEngine;
using System.Collections;

public class OneShotBallSpawnEvent: TutorialEvent
{

	public GameObject ball;
	public BallMachine ballMachine;
	public int numBalls;
	ObjectPool ballPool;
	bool completed = false;

	protected override void InitEvent ()
	{
		ballPool = PoolManager.Instance.GetPoolByRepresentative(ball);
	}

	public override void Activate ()
	{
		for(int i = 0; i < numBalls; i++)
			ballMachine.SpawnBall(ballPool.GetPooled());
		completed = true;
	}

	public override void Deactivate ()
	{
		//spawnedBall.SendMessage("Destroy");
	}

	public override bool Completed ()
	{
		return completed;
	}
}

