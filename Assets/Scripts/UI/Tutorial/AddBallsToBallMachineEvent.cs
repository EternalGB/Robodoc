using UnityEngine;
using System.Collections.Generic;

public class AddBallsToBallMachineEvent : TutorialEvent
{

	public BallMachine ballMachine;
	public List<GameObject> balls;
	public bool goodBalls;
	bool completed = false;

	protected override void InitEvent ()
	{

	}

	public override void Activate ()
	{
		if(goodBalls)
			foreach(GameObject ball in balls)
				ballMachine.AddGoodBall(ball);
		else
			foreach(GameObject ball in balls)
				ballMachine.AddBadBall(ball);
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

