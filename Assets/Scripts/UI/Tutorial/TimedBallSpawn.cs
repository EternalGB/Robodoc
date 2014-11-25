using UnityEngine;
using System.Collections;

public class TimedBallSpawn : TimedTutorialEvent
{

	public GameObject ball;
	public BallMachine ballMachine;
	GameObject spawnedBall;

	protected override void InitEvent ()
	{

	}

	protected override void StartEvent ()
	{
		bool wasInactive = !ballMachine.gameObject.activeInHierarchy;
		if(wasInactive)
			ballMachine.gameObject.SetActive(true);
		spawnedBall = ballMachine.SpawnBall(PoolManager.Instance.GetPoolByRepresentative(ball).GetPooled());
		if(wasInactive)
			ballMachine.gameObject.SetActive(false);
	}

	public override void Deactivate ()
	{
		spawnedBall.SendMessage("Destroy");
	}
}

