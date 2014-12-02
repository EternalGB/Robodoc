using UnityEngine;
using System.Collections;

public class BadBallUnlocker : Unlockable
{

	public ArcadeManager manager;
	public GameObject ball;
	public ArcadeStats.StatKeys stat;
	public float unlockRequirement;

	public override bool ConditionMet ()
	{
		return ArcadeStats.GetStat(stat) >= unlockRequirement;
	}

	public override void UnlockEffect ()
	{
		manager.badBalls.Add(ball);
	}

}

