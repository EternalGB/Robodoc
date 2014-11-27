using UnityEngine;
using System.Collections;

public class SingleBallSpawnEvent: TutorialEvent
{

	public GameObject ball;
	public BallMachine ballMachine;
	GameObject spawnedBall;
	bool completed = false;

	protected override void InitEvent ()
	{

	}

	public override void Activate ()
	{
		spawnedBall = ballMachine.SpawnBall(PoolManager.Instance.GetPoolByRepresentative(ball).GetPooled());
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

