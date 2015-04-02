using UnityEngine;
using System.Collections;

public class ColorBallUnlocker : Unlockable
{

	public ArcadeManager manager;
	public GameObject ball;

	public override void UnlockEffect ()
	{
		manager.goodBallPrefabs.Add(ball);
	}

}

