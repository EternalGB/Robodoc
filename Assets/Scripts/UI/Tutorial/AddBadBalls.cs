using UnityEngine;
using System.Collections.Generic;

public class AddBadBalls : TutorialEvent
{

	public List<GameObject> badBalls;
	public BallMachine ballMachine;

	bool completed = false;

	protected override void InitEvent ()
	{

	}

	public override void Activate ()
	{
		foreach(GameObject badBall in badBalls)
			ballMachine.AddBadBall(badBall);
		completed = true;
	}

	public override void Deactivate ()
	{

	}

	public override bool Completed ()
	{
		return completed;
	}

}

