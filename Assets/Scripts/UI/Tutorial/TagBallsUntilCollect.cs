using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class TagBallsUntilCollect : TagBalls
{

	public PlayerBall pb;
	bool ballCollected = false;

	protected override bool CheckCompleted ()
	{
		return ballCollected;
	}

	public override void Activate ()
	{
		pb.BallCollect += HandleBallCollect;
		base.Activate ();
	}

	void HandleBallCollect (GameObject player, GameObject playerPart, GameObject ball)
	{
		ballCollected = true;
	}
}

