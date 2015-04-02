using UnityEngine;
using System.Collections;

public class BadBallUnlocker : Unlockable
{

	public ArcadeManager manager;
	public GameObject ball;


	public override void UnlockEffect ()
	{
		manager.badBalls.Add(ball);
	}

}

